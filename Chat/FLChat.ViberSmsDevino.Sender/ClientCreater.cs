using FLChat.Core;
using FLChat.Core.MsgCompilers;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.ViberSmsDevino.Sender
{
    public class ClientCreater
    {
        public IMessageSender CreateClient(int num = 0)
        {
            string _pattern = GetPattern(num);
            CombiSender client = new CombiSender(
                           new DevinoViberSender(_pattern, ChangeParamTextCompiler.GetForwardMsg, num),
                           new DevinoSmsSender(),
                           ViberSmsChecker.Check);
            return client;
        }

        public IMessageSender CreateViberClient(int num = 0)
        {
            string _pattern = GetPattern(num);
            IMessageSender client = new DevinoViberSender(_pattern, ChangeParamTextCompiler.GetForwardMsg, num);
            return client;
        }

        private string GetPattern(int num)
        {
            string _pattern;
            switch (num)
            {
                case 1:

                    // Формирование ссылки по СМС-сообщению - на страницу выбора транспорта
                    _pattern = Settings.Values.GetValue("WEB_CHAT_DEEP_URL", "https://chat.faberlic.com/external/%code%");
                    int pos = _pattern.IndexOf('%');
                    if (pos < 0)
                        pos = _pattern.Length - 1;
                    _pattern = _pattern.Substring(0, pos);
                    pos = _pattern.Length - 1;
                    if (_pattern[pos] == '/')
                        _pattern = _pattern.Substring(0, pos);
                    break;
                case 0:
                default:
                    // Формирование ссылки по WebChatDeepLink с переадрессацией в Viber-bot
                    _pattern = GetPatternLink(TransportKind.Viber);
                    break;                    
            }
            return _pattern;
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
    }
}
