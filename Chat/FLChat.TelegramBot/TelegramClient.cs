using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

using MihaZupan;
using Telegram.Bot;
using Telegram.Bot.Types;
using TGMessage = Telegram.Bot.Types.Message;

using FLChat.DAL.Model;
using FLChat.Core;
using FLChat.DAL;
using Telegram.Bot.Types.ReplyMarkups;
using FLChat.TelegramBot.Adapters;
using FLChat.TelegramBot.Algorithms;
using FLChat.FDAL.Model;
using FLChat.Core.Buttons;
using FLChat.Core.Routers;

namespace FLChat.TelegramBot
{
    public class TelegramClient : TelegramBotClientHandler, IMessageSender
    {
        private readonly ITransportButtonsSource _buttons;
        private readonly IScenarioButtons _scenarioButtons;

        public const string SEND_BUTTONS_MENU = "UPD_MENU";

        private const int MaxTextLengthWithFile = 1024; //maximum length of text in message with file

        public TelegramClient(string token, IWebProxy proxy, IScenarioButtons scenarioButtons = null) : base(token, proxy)
        {
            _buttons = new TransportButtonsSourceBuffered();
            _scenarioButtons = scenarioButtons ?? new ScenarioButtons();
        }

        public TelegramClient(string token, string proxyAddr, int proxyPort, string proxyUser, string proxyPsw, IScenarioButtons scenarioButtons = null)
            : base(token, proxyAddr, proxyPort, proxyUser, proxyPsw)
        {
            _buttons = new TransportButtonsSourceBuffered();
            _scenarioButtons = scenarioButtons ?? new ScenarioButtons();
        }

        /// <summary>
        /// Send message wia telegram bot
        /// </summary>
        /// <param name="msg">Database message object</param>
        /// <param name="msgText">Message text</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Information about sent message</returns>
        public async Task<SentMessageInfo> Send(MessageToUser msg, string msgText, CancellationToken ct) {
            IReplyMarkup replyMarkup = GetReplyMarkup(msg);

            if (msg.Message.FileId == null) {
                TGMessage m = await SendTextMessage(msg, msgText, replyMarkup, ct);
                return MakeResult(m);
            } else {
                bool withText = msgText == null || msgText.Length <= MaxTextLengthWithFile;
                TGMessage textResult = null;
                TGMessage fileResult = null;
                if (withText == false)
                    textResult = await SendTextMessage(msg, msgText, replyMarkup, ct);
                if (msg.Message.FileInfo.MediaType.Kind == MediaGroupKind.Image)
                    fileResult = await SendPhotoMessage(msg, withText ? msgText : null, replyMarkup, ct);
                else
                    fileResult = await SendDocumentMessage(msg, withText ? msgText : null, replyMarkup, ct);
                return MakeResult(textResult, fileResult);
            }
        }

        private IReplyMarkup GetReplyMarkupForSelectAddressee(MessageToUser msg) {
            using (ChatEntities entities = new ChatEntities()) {
                DAL.Model.User[] users = entities.GetAddresseesForExternalTrans(msg.ToUserId);
                if (users.Length == 0)
                    return null;
                return new InlineKeyboardMarkup(users.Select(u => new InlineKeyboardButton[] { new InlineKeyboardButton() {
                    Text = u.FullName,
                    CallbackData = String.Concat(ICallbackDataExtentions.ADDRESSEE_SWITCH, u.Id.ToString())
                } }));
            }
        }

        private IReplyMarkup GetReplyMarkup(MessageToUser msg)
        {
            if (msg.Message.GetSpecificValue(Algorithms.TgBotCommandsRouter.SELECT_ADDRESSEE_MENU, out string value)) {
                return GetReplyMarkupForSelectAddressee(msg);
            } else {
                if (IsNeedToShowSwitchToReply(msg)) {
                    return GetSwitchToReplyMarkup(msg);
                } else {
                    ITransportButtonsSource butt = _buttons;
                    if (msg.Message.ScenarioStepId != null)
                        butt = _scenarioButtons.Adapt();
                    return GetMainReplyMarkup(msg, butt);
                    //List<KeyboardButton[]> kb = new List<KeyboardButton[]>();
                    ////if (msg.ToTransport.User.IsTemporary == false) {
                    //    kb.AddRange(_buttons
                    //        .GetButtons(msg)
                    //        .Select(r => r.Select(c => new KeyboardButton(c.Caption)).ToArray())
                    //        .ToArray());
                    ////}

                    //if (msg.Message.IsPhoneButton)
                    //    kb.Insert(0, new KeyboardButton[] { new KeyboardButton("Послать телефон") { RequestContact = true } });

                    //if (kb.Count > 0)
                    //    return new ReplyKeyboardMarkup(kb) { OneTimeKeyboard = true };
                }
            }

          //  return null;
        }

        private IReplyMarkup GetMainReplyMarkup(MessageToUser msg, ITransportButtonsSource butt)
        {
            List<KeyboardButton[]> kb = new List<KeyboardButton[]>();
            ////if (msg.ToTransport.User.IsTemporary == false) {

            //kb.AddRange(butt
            //    .GetButtons(msg)
            //    .Select(r => r.Select(c => new KeyboardButton(c.Caption)).ToArray())
            //    .ToArray());
            ////}

            foreach (var row in butt.GetButtons(msg))
            {
                int count = row.Count();
                KeyboardButton[] keyboardButtons = new KeyboardButton[count];
                int i = 0;
                foreach (var col in row)
                {
                    BotCommandsRouter.CommandsEnum? cmd = BotCommandsRouter.GetCommandType(col.Command, out string arg);
                    KeyboardButton btn = new KeyboardButton(col.Caption);
                    switch (cmd)
                    {                       
                        case BotCommandsRouter.CommandsEnum.Phone:
                            btn.RequestContact = true;
                            break;
                    }
                    keyboardButtons[i] = btn;
                    i++;
                }
                kb.Add(keyboardButtons);
            }

            if (msg.Message.IsPhoneButton)
                kb.Insert(0, new KeyboardButton[] { new KeyboardButton("Послать телефон") { RequestContact = true } });

            if (kb.Count > 0)
                return new ReplyKeyboardMarkup(kb,true) { OneTimeKeyboard = false };
            return null;
        }

        /// <summary>
        /// Is need to show reply to user buttom
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool IsNeedToShowSwitchToReply(MessageToUser msg) {
            DAL.Model.User from = msg.Message.FromTransport.User;
            return msg.ToTransport.User.IsTemporary == false
                && from.IsBot == false
                && msg.ToTransport.IsAddressee(from) == false;
        }

        /// <summary>
        /// Get Keyboard reply for switch messages' addresse to message sender
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private IReplyMarkup GetSwitchToReplyMarkup(MessageToUser msg) {
            return new InlineKeyboardMarkup(new InlineKeyboardButton() {
                Text = Settings.Values.GetValue("TEXT_TG_SWITCH_TO_SENDER", "Написать ответ"),
                CallbackData = String.Concat(ICallbackDataExtentions.ADDRESSEE_SWITCH, msg.Message.FromUserId.ToString())
            });
        }

        /// <summary>
        /// Send text message
        /// </summary>
        /// <param name="msg">database message object</param>
        /// <param name="msgText">message text</param>
        /// <param name="replyMarkup">keyboard</param>
        /// <param name="ct">cancellation token</param>
        /// <returns>Telegram send message responce</returns>
        private async Task<TGMessage> SendTextMessage(
            MessageToUser msg, 
            string msgText, 
            IReplyMarkup replyMarkup, 
            CancellationToken ct) {
            return await Client.SendTextMessageAsync(
                    new ChatId(int.Parse(msg.ToTransport.TransportOuterId)),
                    msgText,
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                    replyMarkup: replyMarkup,
                    cancellationToken: ct);
        }

        /// <summary>
        /// Send message with photo
        /// </summary>
        private async Task<TGMessage> SendPhotoMessage(
            MessageToUser msg,
            string msgText,
            IReplyMarkup replyMarkup,
            CancellationToken ct) {
            return await Client.SendPhotoAsync(
                    new ChatId(int.Parse(msg.ToTransport.TransportOuterId)),
                    new Telegram.Bot.Types.InputFiles.InputOnlineFile(msg.Message.FileInfo.Url),
                    caption: msgText,
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                    replyMarkup: replyMarkup,
                    cancellationToken: ct);
        }

        /// <summary>
        /// Send message with document
        /// </summary>
        private async Task<TGMessage> SendDocumentMessage(
            MessageToUser msg,
            string msgText,
            IReplyMarkup replyMarkup,
            CancellationToken ct) {

            Stream stream = GetFileData(msg);
            if (stream == null)
                return null;

            return await Client.SendDocumentAsync(
                new ChatId(int.Parse(msg.ToTransport.TransportOuterId)),
                //new Telegram.Bot.Types.InputFiles.InputOnlineFile(msg.Message.FileInfo.Url),
                new Telegram.Bot.Types.InputFiles.InputOnlineFile(stream, msg.Message.FileInfo.FileName),
                caption: msgText,
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                replyMarkup: replyMarkup,
                cancellationToken: ct);
        }

        private SentMessageInfo MakeResult(TGMessage m) => new SentMessageInfo(m.MessageId.ToString(), m.Date);

        private SentMessageInfo MakeResult(TGMessage m1, TGMessage m2) {
            if (m1 != null && m2 != null)
                return new SentMessageInfo(
                    new string[] { m1.MessageId.ToString(), m2.MessageId.ToString() },
                    m2.Date);
            else
                return MakeResult(m1 ?? m2);
        }

        private Stream GetFileData(MessageToUser msg) {
            using (FileEntities files = new FileEntities()) {
                FileData fileData = files.FileData.Where(fd => fd.Id == msg.Message.FileId.Value).SingleOrDefault();
                if (fileData == null)
                    return null;
                return new MemoryStream(fileData.Data);
            }
        }
    }
}
