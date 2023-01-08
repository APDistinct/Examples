using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core.MsgCompilers
{
    public class OwnerUserMsgTextCompiler : IMessageTextCompiler
    {
        public string _hashString => "OwnerUser";
        private TagReplaceTextCompiler compiller;
        
        public OwnerUserMsgTextCompiler()
        {
            Dictionary<string, Func<MessageToUser, string>> ReplaceDict
               = new Dictionary<string, Func<MessageToUser, string>>()
               {
                    { _hashString , mtu => mtu.ToTransport.User.OwnerUser?.FullName ?? "" },
               };

            compiller = new TagReplaceTextCompiler(ReplaceDict, true);
        }

        public string MakeText(MessageToUser mtu, string text)
        {
            return compiller.MakeText(mtu, text);
        }
    }
}
