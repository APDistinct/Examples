using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.Core.MsgCompilers
{
    public class AuthorMsgSimpleTextCompiler : IMessageTextCompiler
    {
        //private readonly ITextTransform _tt;

        public AuthorMsgSimpleTextCompiler(/*ITextTransform tt = null*/)
        {
            //_tt = tt ?? new TTEscape(StringExtentions.MarkdownChars);
        }
        public string MakeText(MessageToUser mtu, string text)
        {
            return MakeText(mtu.Message, text);
        }

        public string MakeText(Message mtu, string text)
        {
            if (mtu.FromTransport.User.IsBot)
                return text;
            else
                return String.Concat(
                     mtu.FromTransport.User.FullName,
                    " : ", text
                    );
        }
    }
}
