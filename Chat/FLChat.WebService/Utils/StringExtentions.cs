using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.Utils
{
    public static class StringExtentions
    {
        public static string ComputeMD5(this string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        public static T? GetValue<T>(this string name) where T : struct, IConvertible
        {
            T? result = //(T)Enum.Parse(typeof(T), name, true);
            Enum.TryParse(name, true, out T val) ? val : (T?)null;
            return result;
        }

        public static string MediaTypeExtention(this string mediaType)
        {
            int pos = mediaType.IndexOf("/");
            if (pos == -1)
                return null;
            return mediaType.Substring(pos + 1);
        }

        public static int WordsCount(this string source, string strfind)
        {
            String[] temp = source.Split(new[] { strfind }, StringSplitOptions.None);
            return temp.Length - 1;
        }

        static string[] CyrilicToLatinL =
            "a,b,v,g,d,e,zh,z,i,j,k,l,m,n,o,p,r,s,t,u,f,kh,c,ch,sh,sch,j,y,j,e,yu,ya".Split(',');
        static string[] CyrilicToLatinU =
            "A,B,V,G,D,E,Zh,Z,I,J,K,L,M,N,O,P,R,S,T,U,F,Kh,C,Ch,Sh,Sch,J,Y,J,E,Yu,Ya".Split(',');

        public static string CyrilicToLatin(this string s)
        {
            var sb = new StringBuilder((int)(s.Length * 1.5));
            foreach (char c in s)
            {
                if (c >= '\x430' && c <= '\x44f') sb.Append(CyrilicToLatinL[c - '\x430']);
                else if (c >= '\x410' && c <= '\x42f') sb.Append(CyrilicToLatinU[c - '\x410']);
                else if (c == '\x401') sb.Append("Yo");
                else if (c == '\x451') sb.Append("yo");
                else sb.Append(c);
            }
            return sb.ToString();
        }

    }
}
