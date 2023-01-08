using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core.InviteLink
{
    public static class InviteLinkCoding
    {
        private const string IdPrefix = "il";

        static public string InviteLinkCode(this Guid guid)
        {
            string n = guid.ToString("N");

            return IdPrefix + n;
        }

        static public string InviteLinkDecode(this string code)
        {
            return code;
        }

        static public Guid ExtractGuid(this string decoded)
        {
            if (decoded.StartsWith(IdPrefix))
                return Guid.ParseExact(decoded.Substring(IdPrefix.Length), "N");
            else
                throw new ArgumentException(String.Concat(decoded, " does not contain consultant id"));
        }
    }
}