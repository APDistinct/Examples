using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.Core.MsgCompilers
{
    public class DevinoViberSmsCompiler : IMessageTextCompiler
    {
        private readonly string _pattern;        

        public DevinoViberSmsCompiler(string pattern = null)
        {
            _pattern = pattern ??"#sendername, ваш личный консультант Faberlic, отправил вам сообщение:";
        }

        public string MakeText(MessageToUser mtu, string text)
        {
            string str = _pattern + "\n\n";            
            str += mtu.Message.ForwardMsg.Text ?? "";
            return str;
        }
    }
}
