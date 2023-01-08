using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core.MsgCompilers
{
    public static class StringExtentions
    {
        public static string EscapeSymbols(this string source, char[] chars) {
            if (source == null || chars == null)
                return source;
            StringBuilder sb = null;
            for (int i = 0; i < source.Length; ++i) {
                if (chars.Contains(source[i])) {
                    if (sb == null)
                        sb = new StringBuilder(source.Substring(0, i), source.Length * 110 / 100);
                    sb.Append('\\');
                    sb.Append(source[i]);
                } else {
                    if (sb != null)
                        sb.Append(source[i]);
                }
            }
            if (sb == null)
                return source;
            else
                return sb.ToString();
        }

        public static readonly char[] MarkdownChars = new char[] { '*', '_', '`' };
        public static string EscapeMarkdownSymbols(this string source) => EscapeSymbols(source, MarkdownChars);
    }
}
