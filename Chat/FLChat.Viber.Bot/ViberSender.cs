using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using FLChat.Core;
using FLChat.DAL.Model;
using FLChat.Viber.Client;
using FLChat.Viber.Client.Types;
using FLChat.Viber.Client.Requests;
using FLChat.DAL;
using FLChat.Core.Routers;
using FLChat.Viber.Bot.Routers;
using FLChat.Core.Buttons;

namespace FLChat.Viber.Bot
{
    public class ViberSender : IMessageSender, IDisposable
    {
        private readonly ViberClient _client;
        private readonly ViberLogWritter _log;
        private readonly ITransportButtonsSource _buttons;
        private readonly IScenarioButtons _scenarioButtons;

        public ViberClient Client => _client;
        public ViberLogWritter Log => _log;

        public ViberSender(string token, IScenarioButtons scenarioButtons = null)
        {
            _log = new ViberLogWritter(true);
            _client = new ViberClient(token);
            _buttons = new TransportButtonsSourceBuffered();
            _scenarioButtons = scenarioButtons ?? new ScenarioButtons();

            _client.MakingApiRequest += _log.Request;
            _client.ApiResponseReceived += _log.Response;
            _client.ApiRequestException += _log.Exception;
        }

        public void Dispose() {
            _client.Dispose();
        }

        public async Task<SentMessageInfo> Send(MessageToUser msg, string msgText, CancellationToken ct) {
            Keyboard kb = GetKeyboard(msg);

            var timesent = DateTime.UtcNow;
            if (msg.Message.FileId == null) {
                //send text message
                SendMessageResponse resp = await SendTextMessage(msg, msgText, kb, ct);
                return MakeResult(resp, timesent);
            } else {
                //check is file is jpeg and size
                if (msg.Message.FileInfo.IsViberPicture()) {
                    //message with file (only jpeg and file size limit)                    
                    if (msgText == null || msgText.Length < SendPictureMessageRequest.MaxTextLength) {
                        //text is short and can be sent as picture description
                        SendMessageResponse resp = await SendPictureMessage(msg, msgText, kb, ct);
                        return MakeResult(resp, timesent);
                    } else {
                        //text too lond and outh to send text and picture in two messages
                        SendMessageResponse resp1 = await SendTextMessage(msg, msgText, kb, ct);
                        SendMessageResponse resp2 = await SendPictureMessage(msg, "", kb, ct);
                        return MakeResult(resp1, resp2, timesent);
                    }
                } else if (msg.Message.FileInfo.FileLength < SendFileMessageRequest.MaxFileSize) {
                    //send text message and file message                    
                    SendMessageResponse resp1 = await SendTextMessage(msg, msgText, kb, ct);
                    SendMessageResponse resp2 = await SendFileMessage(msg, kb, ct);
                    return MakeResult(resp1, resp2, timesent);
                } else {
                    //send url message
                    SendMessageResponse resp1 = await SendTextMessage(msg, msgText, kb, ct);
                    SendMessageResponse resp2 = await SendUrlMessage(msg, kb, ct);
                    return MakeResult(resp1, resp2, timesent);
                }
            }            
        }

        private async Task<SendMessageResponse> SendTextMessage(MessageToUser msg, string msgText, Keyboard kb, CancellationToken ct) {
            if (msgText == null)
                return null;

            return await _client.SendTextMessage(
                msg.Message.FromTransport.User.FullName.CutFullName(Sender.NAME_MAX_LENGTH),
                msg.ToTransport.TransportOuterId,
                msgText,
                ct,
                keyboard: kb);
        }

        private async Task<SendMessageResponse> SendPictureMessage(MessageToUser msg, string msgText, Keyboard kb, CancellationToken ct) {
            return await _client.SendPictureMesage(
                msg.Message.FromTransport.User.FullName.CutFullName(Sender.NAME_MAX_LENGTH),
                msg.ToTransport.TransportOuterId,
                msgText,
                msg.Message.FileInfo.Url,
                ct,
                keyboard: kb);
        }

        private async Task<SendMessageResponse> SendFileMessage(MessageToUser msg, Keyboard kb, CancellationToken ct) {
            return await _client.SendFileMessage(
                msg.Message.FromTransport.User.FullName.CutFullName(Sender.NAME_MAX_LENGTH),
                msg.ToTransport.TransportOuterId,
                msg.Message.FileInfo.Url,
                msg.Message.FileInfo.FileLength,
                msg.Message.FileInfo.FileName,
                ct,
                keyboard: kb);
        }

        private async Task<SendMessageResponse> SendUrlMessage(MessageToUser msg, Keyboard kb, CancellationToken ct) {
            return await _client.SendUrlMessage(
                msg.Message.FromTransport.User.FullName.CutFullName(Sender.NAME_MAX_LENGTH),
                msg.ToTransport.TransportOuterId,
                msg.Message.FileInfo.Url,
                ct,
                keyboard: kb);
        }

        private Keyboard GetKeyboard(MessageToUser msg) {            
            Keyboard kb = null;
            //if (msg.Message.FromTransport.User.IsTemporary == false) {
            if (msg.Message.ScenarioStepId != null)
                kb = GetMainKeyboard(_scenarioButtons.Adapt(), msg);
            //else
            {
                if (msg.Message.HasSpecificValue(ViberBotCommandsRouter.SELECT_ADDRESSEE_MENU))
                    kb = GetReplyMarkupForSelectAddressee(msg);
                if (kb == null)
                    kb = GetMainKeyboard(msg);
            }
            //}
            return kb;
        }

        private Keyboard GetMainKeyboard(MessageToUser mtu) => GetMainKeyboard(_buttons, mtu);

        public static Keyboard GetMainKeyboard(ITransportButtonsSource buttons, MessageToUser mtu) {
            Keyboard kb = new Keyboard();
            foreach (var row in buttons.GetButtons(mtu)) {
                int count = row.Count();
                foreach (var col in row) {
                    BotCommandsRouter.CommandsEnum? cmd = BotCommandsRouter.GetCommandType(col.Command, out string arg);
                    Button btn = new Button(col.Caption, col.Command) {
                        Columns = 6 / count,
                        Rows = 1
                    };
                    switch (cmd)
                    {
                        case BotCommandsRouter.CommandsEnum.Url:
                            ///if (cmd == BotCommandsRouter.CommandsEnum.Url) {
                            btn.ActionType = Button.ActionTypeEnum.OpenUrl;
                            btn.ActionBody = arg;
                            break;
                        case BotCommandsRouter.CommandsEnum.Phone:
                            //if (cmd == BotCommandsRouter.CommandsEnum.Phone)
                            btn.ActionType = Button.ActionTypeEnum.Phone;
                            break;
                    }
                    kb.Buttons.Add(btn);
                }
            }
            return kb;
        }

        private Keyboard GetReplyMarkupForSelectAddressee(MessageToUser msg) {
            using (ChatEntities entities = new ChatEntities()) {
                DAL.Model.User[] users = entities.GetAddresseesForExternalTrans(msg.ToUserId);
                if (users.Length == 0)
                    return null;
                List<Button> buttons = users
                        .Take(6)
                        .Select(u => new Button(u.FullName, ViberBotCommandsRouter.ADDRESSEE_SWITCH + u.Id.ToString()) { Rows = 1 })
                        .ToList();
                buttons.Add(new Button("Назад в меню", ViberBotCommandsRouter.CMD_BACK) { Rows = 1 });
                return new Keyboard() {
                    Buttons = buttons
                };
            }
        }


        //public static Keyboard GetSharePhoneKeyboard(/*ITransportButtonsSource buttons, MessageToUser mtu*/)
        //{
        //    var kb = new Keyboard();
        //    kb.Buttons.Add(GetSharePhoneButton());

        //    return kb;
        //}

        //private static Button GetSharePhoneButton()
        //{
        //    return new Button()
        //    {
        //        Text = "Share Phone",
        //        ActionType = Button.ActionTypeEnum.Phone,
        //        ActionBody = "cmd:share_phone",
        //        Columns = 1,
        //        Rows = 1
        //    };
        //}

        private SentMessageInfo MakeResult(SendMessageResponse resp, DateTime? timesent = null)
            => new SentMessageInfo(resp?.MessageToken.ToString(), timesent ?? DateTime.UtcNow);

        private SentMessageInfo MakeResult(SendMessageResponse resp1, SendMessageResponse resp2, DateTime? timesent = null)
        {
            if (resp1 != null && resp2 != null)
                return new SentMessageInfo(
                new string[] { resp1.MessageToken.ToString(), resp2.MessageToken.ToString() },
                timesent ?? DateTime.UtcNow);
            else
                return MakeResult(resp1 ?? resp2, timesent);
        }
    }
}
