using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Viber.Bot
{
    public static class StringExtentions
    {
        public static String CutFullName(this string fullName, int maxLength) {
            //if (fullName.Length <= maxLength)
            //    return fullName;
            while (fullName.Length > maxLength) {
                int index = fullName.LastIndexOf(' ');
                if (index > 0)
                    fullName = fullName.Substring(0, index);
                else
                    return String.Concat(fullName.Substring(0, maxLength - 3), "...");
            }
            return fullName;
        }
    }
}
