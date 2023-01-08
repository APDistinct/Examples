using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Viber.Client.Utils
{
    public static class StringExtentions
    {
        /// <summary>
        /// Remove spaces and new lines from string
        /// </summary>
        /// <param name="str">source string</param>
        /// <returns>new string without spaces and new lines</returns>
        public static string RemoveSpacesAndNewLines(this string str) {
            return str.Replace(" ", String.Empty).Replace("\r\n", String.Empty);
        }

        /// <summary>
        /// Calculate HMAC SHA 256
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] HMACSHA256(this string data, String key) {
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.ASCII.GetBytes(key))) {
                return hmac.ComputeHash(Encoding.ASCII.GetBytes(data));
            }
        }

        /// <summary>
        /// Calculate HMAC SHA 256
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string HMACSHA256String(this string data, string key) {
            return BitConverter.ToString(data.HMACSHA256(key)).Replace("-", "").ToLower();
        }

    }
}
