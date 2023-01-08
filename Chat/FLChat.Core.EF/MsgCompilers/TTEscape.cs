using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core.MsgCompilers
{
    public class TTEscape : ITextTransform
    {
        private readonly char[] chars;

        public TTEscape(char[] chars) {
            this.chars = chars;
        }

        public string Transform(string sourse) => sourse.EscapeSymbols(chars);
    }
}
