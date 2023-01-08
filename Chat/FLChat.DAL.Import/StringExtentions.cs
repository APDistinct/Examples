using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Import
{
    public static class StringExtentions
    {
        public static string RemoveNonDigits(this string rawPhone) {
            if (String.IsNullOrEmpty(rawPhone))
                return null;

            StringBuilder sb = null;
            for (int i = 0; i < rawPhone.Length; ++i) {
                if (Char.IsDigit(rawPhone[i])) {
                    if (sb != null)
                        sb.Append(rawPhone[i]);
                } else {
                    if (sb == null)
                        sb = new StringBuilder(rawPhone.Substring(0, i), rawPhone.Length);
                }
            }

            if (sb == null)
                return rawPhone;
            else if (sb.Length > 0)
                return sb.ToString();
            else
                return null;
        }
    }
}
