using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VKMessage = FLChat.VKBotClient.Types.Message;
using FLChat.DAL.Model;
using FLChat.Core;
using FLChat.DAL;
using FLChat.VKBotClient.Types;
using FLChat.VKBot.Routers;
using FLChat.FDAL.Model;
using FLChat.VKBotClient.Types.Attachments;
using FLChat.WebService.Utils;
using FLChat.Core.Buttons;

namespace FLChat.VKBot
{
    public class VKClient : IMessageSender
    {
        private readonly VKBotClient.VKBotClient _client;
        private readonly VKLogWritter _log;
        private readonly ITransportButtonsSource _buttons;
        private readonly IScenarioButtons _scenarioButtons;
        private readonly IVKPhotoChecker _checker;

        public VKBotClient.VKBotClient Client => _client;
        public VKLogWritter Log => _log;

        public VKClient(string token, IVKPhotoChecker checker = null, IScenarioButtons scenarioButtons = null)
        {
            _log = new VKLogWritter(true, TransportKind.VK);
            _client = new VKBotClient.VKBotClient(token);
            _buttons = new TransportButtonsSourceBuffered();
            _scenarioButtons = scenarioButtons ?? new ScenarioButtons();
            _client.MakingApiRequest += _log.Request;
            _client.ApiResponseReceived += _log.Response;
            _client.ApiRequestException += _log.Exception;
            _checker = checker ?? new VKFileChecker();
        }

        public async Task<SentMessageInfo> Send(MessageToUser msg, string msgText, CancellationToken ct) {
            VkKeyboard keyboard = GetReplyMarkup(msg);
            AttachmentFile file = GetFile(msg.Message.FileInfo);
            if (file != null) {
                _checker.PhotoCheck(file);
                file.Name = file.Name.CyrilicToLatin();
            }

            var timesent = DateTime.UtcNow;
            var m = await _client.SendTextMessageAsync(
                msg.ToTransport.TransportOuterId,
                msgText,//msg.Message.Message,                
                ct,
                keyboard?.GetJson(),
                file != null ? new AttachmentFile[] {file} : null
                );

            //  Проверка на отправку - пустое ли поле Error?
            if(m.Error != null)
            {
                //  Ошибка отправки, надо всё запротоколировать и прекратить работу
                throw new /*Exception*/AggregateException($"Ошибка {m.Error.Code} : {m.Error.Msg}");
            }

            if (m.Messages[0].Error != null)
            {
                //  Ошибка отправки, надо всё запротоколировать и прекратить работу
                throw new /*Exception*/AggregateException($"Ошибка {m.Messages[0].Error.Code} : {m.Messages[0].Error.Description}");
            }
            return new SentMessageInfo(m.Messages[0].MessageId.ToString(),  timesent /*m.Response.Date*/);
        }

        private AttachmentFile GetFile(FileInfo fi)
        {
            AttachmentFile file = null;
            using (FileEntities fileEntities = new FileEntities())
            {
                try
                {
                    FileData fd = fileEntities.FileData.Where(f => f.Id == fi.Id).FirstOrDefault();
                    if (fd != null)
                    {
                        file = new AttachmentFile
                        {
                            Name = fi.FileName,
                            Bytes = fd.Data,
                            Type = GetAttachmentType(fi.MediaType.MediaTypeGroupId)
                        };
                    }
                }
                catch(Exception ex)
                {
                    string s = ex.Message;
                }
            }
            return file;
        }
        
        private AttachmentType GetAttachmentType(int mtype)
        {
            AttachmentType type = AttachmentType.Doc;
            switch (mtype)
            {
                case (int)MediaGroupKind.Image:
                    type = AttachmentType.Photo;
                    break;
                //case (int)MediaGroupKind.Document:
                //    break;
                //case (int)MediaGroupKind.Audio:
                //    break;
                //case (int)MediaGroupKind.Video:
                //    break;
            }
            return type;
        }
    
        private VkKeyboard GetReplyMarkup(MessageToUser msg)
        {
            if (msg.Message.GetSpecificValue(VKBotCommandsRouter.SELECT_ADDRESSEE_MENU, out string value))
            {
                return GetReplyMarkupForSelectAddressee(msg);
            }
            else
            {
                List<VkKeyboardButton[]> kb = new List<VkKeyboardButton[]>();
                ITransportButtonsSource butt = _buttons;
                if (msg.Message.ScenarioStepId != null)
                    butt = _scenarioButtons.Adapt();
                //return GetMainReplyMarkup(msg, butt);
                //if (msg.ToTransport.User.IsTemporary == false) {
                var kbutt = butt.GetButtons(msg);
                if (kbutt.Count() > 0)
                kb.AddRange(butt
                        .GetButtons(msg)
                        .Select(r => r.Select(c => new VkKeyboardButton(new Text()
                        //{ Label = c.Caption, Payload = "{ \"button\" : \"" + $"{c.Command}" +"\" }" /*c.Command*/ }) {Color = "primary" }).ToArray())
                        { Label = c.Caption, Payload = (new VKPayloadConverter(c.Command)).GetJson() }) { Color = "primary" }).ToArray())
                            .ToArray());

                //}

                if (kb.Count > 0)
                    return new VkKeyboard(kb.ToArray()) { OneTime = false };
            }

            return null;
        }

        private VkKeyboard GetReplyMarkupForSelectAddressee(MessageToUser msg)
        {
            List<VkKeyboardButton[]> kb = new List<VkKeyboardButton[]>();
            using (ChatEntities entities = new ChatEntities())
            {
                DAL.Model.User[] users = entities.GetAddresseesForExternalTrans(msg.ToUserId);
                if (users.Length == 0)
                    return null;

                kb.AddRange(users.Select(u => new VkKeyboardButton[]
                {
                    new VkKeyboardButton(
                        new Text()
                        {
                            Label = u.FullName,
                            Payload = (new VKPayloadConverter(String.Concat(VKBotCommandsRouter.ADDRESSEE_SWITCH, u.Id.ToString())).GetJson())
                        })  {Color = "primary" }
                }).ToList());
            }
            kb.Add(
                new VkKeyboardButton[]
                {
                    new VkKeyboardButton
                    (
                        new Text()
                        {
                            Label = "Назад в меню",
                            Payload = (new VKPayloadConverter(VKBotCommandsRouter.CMD_BACK)).GetJson()
                        }
                    )  { Color = "primary" }
                });

            return new VkKeyboard(kb.ToArray());
        }
    }
}
