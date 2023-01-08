using Devino.Viber;
using FLChat.Core;
using FLChat.Core.MsgCompilers;
using FLChat.DAL.Model;
using FLChat.Devino;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devino.Logger;
using FLChat.DAL;
using Devino;

namespace FLChat.Transport
{
    public class DevinoViberSender : DevinoSender
    {
        //private readonly IMessageTextCompiler MsgCompiler;
        private readonly string _pattern;
        private readonly string ButtonCaption = "Ответить";        
        private readonly string _code = "%code%";
        private int _patternNum;

        Func<MessageToUser, MessageToUser> _func = null;

        protected override DAL.TransportKind transportKind { get; } = TransportKind.WebChat;

        public DevinoViberSender(string pattern = null, 
            Func<MessageToUser, MessageToUser> func = null, 
            int patternNum = 0, 
            IDevinoProvider sender = null, 
            DevinoSettings settings = null) : base(sender, settings)
        {
            _pattern = pattern;
            _func = func ?? (u => u);
            _patternNum = patternNum;
    }

        public override string GetAddressee(MessageToUser mtu)
        {
            return mtu.ToTransport.User.Phone;
        }
       
        public override async Task<SentMessageInfo> Send(MessageToUser msg, string addressees, string text)
        {
            string url = GetUrl(msg);

            //// _func(msg).WebChatDeepLink.Where(wc => wc.ToUserId == msg.ToUserId).Single().Link;
            ////   Cfg.TransportType            
            //var mtu = _func(msg);
            ////  Что делать если null?
            //var code = mtu.WebChatDeepLink.Where(wc => wc.ToUserId == msg.ToUserId).Single().Link
            //    ?? throw new ArgumentNullException($"Webchat link code is null in MessageToUser idx {msg.Idx}");
            //string url = _pattern?.Replace(_code, code);

            //// Формирование ссылки по СМС-сообщению - на страницу выбора транспорта
            ////string str = msg.Message.Text ?? "";
            ////string url = FindPattern(str, _pattern);
            

            string avatarUrl = null;
            var newmsg = _func(msg);
            var mtype = newmsg.Message.FileInfo?.MediaType?.Kind;
            if (mtype != null && MediaGroupKind.Image == mtype.Value)
            {
                avatarUrl = newmsg.Message.FileInfo.Url;
            }
            else
            {
                var userFrom = _func(msg).Message.FromTransport.User;
                if (userFrom?.AvatarUploadDate != null)
                {
                    avatarUrl = userFrom.UserAvatar.Url;
                    //senderavatar = avatarname.Replace(_senderid, senderid);                
                }
            }

            if (string.IsNullOrEmpty( url))
            {
                throw new Exception("Url for web-chat devino-viber message has not found");
            }

            var vm = new ViberMessageInfo()
            {
                Text = text,  // Текст сообщения - предварительно обработан
                Caption = ButtonCaption,  // Подпись на кнопке
                Action = url,  // ссылка для перехода
                ImageUrl = avatarUrl,  // Адрес картинки                
                Address = addressees,
                SmsText = msg.Message.Text,             
            };
            var ret = await Sender.ViberSend(vm);
            return new SentMessageInfo(ret.ProviderId, ret.SentTime);
            //return Sender.ViberSend(vm);
        }

        private string GetUrl(MessageToUser msg)
        {
            string url = null;
            switch (_patternNum)
            {
                case 1:
                    // Формирование ссылки по СМС-сообщению - на страницу выбора транспорта
                    string str = msg.Message.Text ?? "";
                    url = FindPattern(str, _pattern);
                    break;
                case 0:
                default:
                    // _func(msg).WebChatDeepLink.Where(wc => wc.ToUserId == msg.ToUserId).Single().Link;
                    //   Cfg.TransportType            
                    var mtu = _func(msg);
                    //  Что делать если null?
                    var code = mtu.WebChatDeepLink.Where(wc => wc.ToUserId == msg.ToUserId).Single().Link
                        ?? throw new ArgumentNullException($"Webchat link code is null in MessageToUser idx {msg.Idx}");
                    url = _pattern?.Replace(_code, code);
                    break;
            }
            return url;
        }

        public string FindPattern(string source, string find)
        {
            char[] delimiter = new char[] { ' ', ';', ',' };
            int in1 = source.IndexOf(find);
            if (in1 < 0)
                return null;
            int in2 = source/*.Substring(in1)*/.IndexOfAny(delimiter, in1 + find.Length);
            if (in2 < 0)
                in2 = source.Length;// - in1; //.Count();
            string ret = source.Substring(in1, in2-in1);
            return ret;
        }
    }
}
