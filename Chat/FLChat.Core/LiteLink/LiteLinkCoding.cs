using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core.LiteLink
{
    /// <summary>
    /// Code/decode lite link
    /// </summary>
    public static class LiteLinkCoding
    {
        public const int AlphabetLength = ('9' - '0') + ('z' - 'a') + 2;
        public const int PKLength = 5;
        private const int MinNumLength = 10;
        private const string ConsNumberPrefix = "cn";
        private const string IdPrefix = "cid";

        static public string LiteLinkCode(this int number, string sk, string pk = null, Random rnd = null) {
            if (number < 0)
                throw new ArgumentException("Consultant number must be positive number");
            string n = number.ToString();
            if (n.Length < MinNumLength)
                n = new String('0', MinNumLength - n.Length) + n;
            return LiteLinkCode(ConsNumberPrefix + n, sk, pk, rnd);
        }

        static public string LiteLinkCode(this Guid guid, string sk, string pk = null, Random rnd = null) {
            string n = guid.ToString("N");
            if (n.Length < MinNumLength)
                n = new String('0', MinNumLength - n.Length) + n;
            return LiteLinkCode(IdPrefix + n, sk, pk, rnd);
        }

        static public string LiteLinkCode(this string n, string sk, string pk = null, Random rnd = null) {
            if (pk == null)
                pk = RandomPK(rnd);

            if (pk.Length != PKLength)
                throw new ArgumentException(String.Concat("Length of PK must be ", PKLength.ToString()));
            if (sk.Length != 10 + 26)
                throw new ArgumentException("Length of sk must be equal 36");
            pk = pk.ToLower();
            sk = sk.ToLower();
            if (!pk.IsLegalSymbols())
                throw new ArgumentException("pk contains illegal symbols");
            if (!sk.IsLegalSymbols())
                throw new ArgumentException("sk contains illegal symbols");

            StringBuilder sb = new StringBuilder(n);

            //shift symbols of origin string
            for (int i = 0; i < sb.Length; ++i)
                sb[i] = sb[i].Shift(pk[i % pk.Length].Index());

            //concatenate with pk
            sb.Insert(0, pk);

            //change symbols
            for (int i = 0; i < sb.Length; ++i)
                sb[i] = sk[sb[i].Index()];

            //add check summ
            AddCRC(sb);

            return sb.ToString();
        }

        static public string LiteLinkDecode(this string code, string sk) {
            if (sk.Length != 10 + 26)
                throw new ArgumentException("Length of sk must be equal 36");
            sk = sk.ToLower();
            if (!sk.IsLegalSymbols())
                throw new ArgumentException("sk contains illegal symbols");

            if (code.Length < PKLength + 2)
                throw new ArgumentException("Length of code too small");

            //check crc
            char crc = GetCRC(code, true);
            if (crc != code[code.Length - 1])
                throw new ArgumentException("CRC is invalid");

            StringBuilder sb = new StringBuilder(code.ToLower());
            //remove crc
            sb.Remove(sb.Length - 1, 1);

            for (int i = 0; i < sb.Length; ++i) {
                int pos = sk.IndexOf(sb[i]);
                if (pos == -1)
                    throw new ArgumentOutOfRangeException($"Symbol {sb[i]} in <{code}> at position {i.ToString()} does not contains in alhabet");
                sb[i] = '0'.Shift(pos);
            }

            for (int i = 0; i < sb.Length - PKLength; ++i)
                sb[i + PKLength] = sb[i + PKLength].Shift(AlphabetLength - sb[i % PKLength].Index());

            sb.Remove(0, PKLength);

            return sb.ToString();
        }

        static public int ExtractConsNumber(this string decoded) {
            if (decoded.StartsWith(ConsNumberPrefix))
                return int.Parse(decoded.Substring(ConsNumberPrefix.Length));
            else
                throw new ArgumentException(String.Concat(decoded, " does not contain consultant number"));
        }

        static public Guid ExtractGuid(this string decoded) {
            if (decoded.StartsWith(IdPrefix))
                return Guid.ParseExact(decoded.Substring(IdPrefix.Length), "N");
            else
                throw new ArgumentException(String.Concat(decoded, " does not contain consultant id"));
        }

        /// <summary>
        /// add check summ to code
        /// </summary>
        /// <param name="code"></param>
        static private void AddCRC(StringBuilder code) {
            int crc = 0;
            for (int i = 0; i < code.Length; ++i)
                crc += code[i].Index() * (i + 1);
            //code.Append('0'.Shift((crc / AlphabetLength) % AlphabetLength));
            code.Append('0'.Shift(crc % AlphabetLength));
        }

        /// <summary>
        /// get check summ for code
        /// </summary>
        /// <param name="code"></param>
        static public char GetCRC(string code, bool excludeCrc) {
            int crc = 0;
            for (int i = 0; i < code.Length - (excludeCrc ? 1 : 0); ++i)
                crc += code[i].Index() * (i + 1);
            return '0'.Shift(crc % AlphabetLength);
        }

        /// <summary>
        /// Generate new secret key by shuffle alpabet
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        static public string ShuffleAlpabet(Random rnd = null) {
            if (rnd == null)
                rnd = new Random();

            StringBuilder sb = new StringBuilder(AlphabetLength);
            for (int i = 0; i < AlphabetLength; ++i)
                sb.Append('0'.Shift(i));
            for (int i = 0; i < AlphabetLength; ++i) {
                char ch = sb[i];
                int pos = rnd.Next(AlphabetLength);
                sb[i] = sb[pos];
                sb[pos] = ch;
            }

            return sb.ToString();
        }

        /// <summary>
        /// return index of <paramref name="ch"/> in alphabet 0123456789abc....xyz
        /// Function don't verify is <paramref name="ch"/> contains in alphabet
        /// </summary>
        /// <param name="ch">symbol</param>
        /// <returns>index of <paramref name="ch"/> in alphabet</returns>
        static private int Index(this char ch) {
            if (Char.IsDigit(ch))
                return ch - '0';
            else
                return ch - 'a' + 10;
        }

        /// <summary>
        /// Make shift operation for <paramref name="ch"/> in alphabet 0123456789abc....xyz
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        static private char Shift(this char ch, int offset) {
            int newIndex = (ch.Index() + offset) % AlphabetLength;
            if (newIndex < 10)
                return (char)('0' + newIndex);
            else
                return (char)('a' + newIndex - 10);
        }

        /// <summary>
        /// check is all symbols in <paramref name="str"/> contains in alphabet 0123456789abc....xyz
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static private bool IsLegalSymbols(this string str) {
            for (int i = 0; i < str.Length; ++i) {
                if (!((str[i] >= '0' && str[i] <= '9') || (str[i] >= 'a' && str[i] <= 'z')))
                    return false;
            }
            return true;
        }

        static public string RandomPK(Random rnd = null) {
            if (rnd == null)
                rnd = new Random();
            StringBuilder sb = new StringBuilder(PKLength);
            for (int i = 0; i < PKLength; ++i)
                sb.Append('0'.Shift(rnd.Next(AlphabetLength)));
            return sb.ToString();
        }
    }
}
