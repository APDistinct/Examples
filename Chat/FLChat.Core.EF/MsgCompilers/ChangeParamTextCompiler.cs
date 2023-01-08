using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.Core.MsgCompilers
{
    public class ChangeParamTextCompiler : IMessageTextCompiler
    {
        private IMessageTextCompiler _compiler;
        private readonly Func<MessageToUser, MessageToUser> _func;

        public ChangeParamTextCompiler(IMessageTextCompiler compiler = null, Func<MessageToUser, MessageToUser> func = null)
        {
            _compiler = compiler ?? new SimpleMsgTextCompiler();
            _func = func ?? (u => u);
        }

        public string MakeText(MessageToUser mtu, string text)
        {
            var mtuu = _func(mtu);
            return _compiler.MakeText(mtuu, text);            
        }

        public static MessageToUser GetForwardMsg(MessageToUser mtu)
        {
            return mtu.Message.ForwardMsg.ToUsers.Where(x => x.ToUserId == mtu.ToUserId).Single();
        }
    }
}
