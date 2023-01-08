using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.Core.MsgCompilers
{
    public class AuthorMsgTextCompiler : IMessageTextCompiler
    {
        private readonly ITextTransform _tt;

        public AuthorMsgTextCompiler(ITextTransform tt = null) {
            _tt = tt ?? new TTEscape(StringExtentions.MarkdownChars);
        }

        public string MakeText(MessageToUser mtu, string text)
        {
            return MakeText(mtu.Message, text);
        }

        public string MakeText(Message mtu, string text)
        {
            if (mtu.FromTransport.User.IsBot)
                return _tt.Transform(text);
            else
                return String.Concat(
                    "*", _tt.Transform(mtu.FromTransport.User.FullName), 
                    "*: ", _tt.Transform(text)
                    );
        }
    }
}
