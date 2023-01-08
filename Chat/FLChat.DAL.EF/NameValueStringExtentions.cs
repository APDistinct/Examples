using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL
{
    public static class NameValueStringExtentions
    {
        /// <summary>
        /// Convert string like 'name1=val1;name2;name3=val3' to dictionary
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="separator"></param>
        /// <returns>Dictionary with values or null if origin string is null or empty</returns>
        public static Dictionary<string, string> ToDictionary(this string origin, char separator = ';') {
            if (String.IsNullOrEmpty(origin))
                return null;

            return origin.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(value => value.Split('='))
                .ToDictionary(pair => pair[0], pair => pair.Length > 1 ? pair[1] : null);
        }
    }
}
