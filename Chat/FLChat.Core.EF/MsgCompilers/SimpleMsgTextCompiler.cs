using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.Core.MsgCompilers
{
    public class SimpleMsgTextCompiler : IMessageTextCompiler, IMessageBulkTextCompiler
    {
        public string MakeText(MessageToUser mtu, string text)
        {
            return text;
        }

        public string MakeText(Message mtu, string text)
        {
            return text;
        }
    }
}
