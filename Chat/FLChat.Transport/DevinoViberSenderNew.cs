using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devino;
using Devino.Logger;
using Devino.Viber;
using FLChat.Core;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.Devino;

using FLChat.Core.Algorithms.WebChat;

namespace FLChat.Transport
{
    public class DevinoViberSenderNew : DevinoSender
    {
        private readonly string _pattern;
        private const string ButtonCaption = "Ответить";
        private const string _code = "%code%";
        private readonly int _patternNum;
        protected override DAL.TransportKind transportKind { get; } = TransportKind.WebChat;
        private readonly IWebChatCodeGenerator _codeGenerator;


        //protected override IDevinoProvider Sender { get; }

        public DevinoViberSenderNew(
            int patternNum = 0, 
            IDevinoProvider sender = null, 
            DevinoSettings settings = null,
            IWebChatCodeGenerator codeGenerator = null) 
            : base(sender, settings)
        {
            _pattern = GetPatternLink(TransportKind.Viber);            
            _patternNum = patternNum;
            _codeGenerator = codeGenerator ?? new WebChatCodeGenerator();
            //Sender  = sender ?? new DevinoProvider(DevinoLogger.GetLogger(DAL.TransportKind.WebChat));
        }

        public override string GetAddressee(MessageToUser mtu)
        {
            return mtu.ToTransport.User.Phone;
        }

        public override async Task<SentMessageInfo> Send(MessageToUser msg, string addressees, string text)
        {
            //  Получить код
            var code =  _codeGenerator.Gen(msg) //msg.WebChatDeepLink.Where(wc => wc.ToUserId == msg.ToUserId).Single().Link
                        ?? throw new ArgumentNullException($"Webchat link code is null in MessageToUser idx {msg.Idx}");
            //  Получить url для СМС
            string url = Settings.Values.GetValue("WEB_CHAT_DEEP_URL", "https://chat.faberlic.com/external/%code%").Replace(_code, code);
            //  Получить текст СМС
            string smsText = Settings.Values.GetValue("WEB_CHAT_SMS", 
                "%sender_name% приглашает Вас в официальный чат компании Faberlic. Продолжите общение в удобном мессенджере: %url%")
                .Replace("%sender_name%", msg.Message.FromTransport.User.FullName ?? "")
                .Replace("%url%", url);
            //  Получить url для сообщения в зависимости от номера. Точнее - переделать его, если надо
            if(_patternNum != 1)
            {
                url = _pattern?.Replace(_code, code); 
            }

            if (string.IsNullOrEmpty(url))
            {
                throw new Exception("Url for web-chat devino-viber message has not found");
            }

            string avatarUrl = null;            
            var mtype = msg.Message.FileInfo?.MediaType?.Kind;
            if (mtype != null && MediaGroupKind.Image == mtype.Value)
            {
                avatarUrl = msg.Message.FileInfo.Url;
            }
            else
            {
                var userFrom = msg.Message.FromTransport.User;
                if (userFrom?.AvatarUploadDate != null)
                {
                    avatarUrl = userFrom.UserAvatar.Url;
                    //senderavatar = avatarname.Replace(_senderid, senderid);                
                }
            }
            
            var vm = new ViberMessageInfo()
            {
                Text = text,  // Текст сообщения - предварительно обработан
                Caption = ButtonCaption,  // Подпись на кнопке
                Action = url,  // ссылка для перехода
                ImageUrl = avatarUrl,  // Адрес картинки                
                Address = addressees,
                SmsText = smsText,
            };
            var ret = await Sender.ViberSend(vm);
            return new SentMessageInfo(ret.ProviderId, ret.SentTime);
            //return Sender.ViberSend(vm);
        }

        private string GetPatternLink(TransportKind kind)
        {
            string ret = null;
            using (ChatEntities entities = new ChatEntities())
            {
                ret = entities.TransportType.Where(t => t.Id == (int)kind).Select(x => x.DeepLink).FirstOrDefault();
            }
            return ret;
        }

        //private string GetUrl(MessageToUser msg)
        //{
        //    string url = null;
        //    switch (_patternNum)
        //    {
        //        case 1:
        //            // Формирование ссылки по СМС-сообщению - на страницу выбора транспорта
        //            string str = msg.Message.Text ?? "";
        //            url = FindPattern(str, _pattern);
        //            break;
        //        case 0:
        //        default:
        //            // _func(msg).WebChatDeepLink.Where(wc => wc.ToUserId == msg.ToUserId).Single().Link;
        //            //   Cfg.TransportType            
        //            var mtu = msg;
        //            //  Что делать если null?
        //            var code = mtu.WebChatDeepLink.Where(wc => wc.ToUserId == msg.ToUserId).Single().Link
        //                ?? throw new ArgumentNullException($"Webchat link code is null in MessageToUser idx {msg.Idx}");
        //            url = _pattern?.Replace(_code, code);
        //            break;
        //    }
        //    return url;
        //}

        //public string FindPattern(string source, string find)
        //{
        //    char[] delimiter = new char[] { ' ', ';', ',' };
        //    int in1 = source.IndexOf(find);
        //    if (in1 < 0)
        //        return null;
        //    int in2 = source/*.Substring(in1)*/.IndexOfAny(delimiter, in1 + find.Length);
        //    if (in2 < 0)
        //        in2 = source.Length;// - in1; //.Count();
        //    string ret = source.Substring(in1, in2 - in1);
        //    return ret;
        //}
    }
}
