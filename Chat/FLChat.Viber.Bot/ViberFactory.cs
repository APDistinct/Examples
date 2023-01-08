using FLChat.Core;
using FLChat.Core.MsgCompilers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Viber.Bot
{
    public static class ViberFactory
    {
        public static IMessageTextCompiler CreateCompiler() {
            return new /*SimpleMsgTextCompiler*/OwnerUserMsgTextCompiler().UniteWithHashCompiler();
        }
    }
}
