using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Core.LiteLink.Tests
{
    [TestClass]
    public class LiteLinkCodingTests
    {
        struct Data
        {
            public string Source;
            public int? Number;
            public string PK;
            public string SK;
            public string Result;

            public Data(string source, string pK, string sK, string result) {
                Source = source;
                PK = pK;
                SK = sK;
                Result = result;
                Number = null;
            }

            public Data(int number, string pK, string sK, string result) {
                Source = null;
                PK = pK;
                SK = sK;
                Result = result;
                Number = number;
            }
        }

        Data[] values;

        [TestInitialize]
        public void Init() {
            values = new Data[] {
                new Data("1234567890123", "00000", "0123456789abcdefghijklmnopqrstuvwxyz", "0000012345678901232"),
                new Data("1234567890123", "11111", "0123456789abcdefghijklmnopqrstuvwxyz", "1111123456789a1234t"),
                new Data("1234567890123", "z0000", "0123456789abcdefghijklmnopqrstuvwxyz", "z000002345578900234"),
                new Data("1234567890123", "00000", "0a234567891bcdefghijklmnopqrstuvwxyz", "00000a234567890a23k"),
                new Data("1234567890123", "z0000", "0123456789abcdefghijklmnopqrstuvwzyx", "x000002345578900232"),
                new Data(741800, "ipfj5", "e2nu9z4akwchfiyqg1b5rlx78jdv6ptm03so","bjq5ztfq5zb05ribjh"),
                new Data(3026511, "uzth5", "s32ljfp98ow5rkmhutx1cqday70enbv64giz", "vzbtfpdbtfgz6aw6sf"),
                new Data(415239, "ggaqx", "aod0vu2fzmhc5ny7wlxti6qsj31rgb8k9e4p", "wwh1eg0h1ewickpt3c"),
            };
        }    

        [TestMethod]
        public void LiteLinkCoding_AlphabetLength() {
            Assert.AreEqual(36, LiteLinkCoding.AlphabetLength);
        }

        [TestMethod]
        public void LiteLinkCoding_LiteLinkCode() {
            foreach (var d in values)
                if (d.Source != null)
                    Assert.AreEqual(d.Result, d.Source.LiteLinkCode(d.SK, d.PK));
                else
                    Assert.AreEqual(d.Result, d.Number.Value.LiteLinkCode(d.SK, d.PK));
        }

        [TestMethod]
        public void LiteLinkCoding_LiteLinkDecode() {
            foreach (var d in values) {
                if (d.Source != null)
                    Assert.AreEqual(d.Source, d.Result.LiteLinkDecode(d.SK));
                else
                    Assert.AreEqual(d.Number, d.Result.LiteLinkDecode(d.SK).ExtractConsNumber());
            }
            foreach (var d in values) {
                if (d.Source != null)
                    Assert.AreEqual(d.Source.ToUpper(), d.Result.LiteLinkDecode(d.SK));
                else
                    Assert.AreEqual(d.Number, d.Result.LiteLinkDecode(d.SK).ExtractConsNumber());
            }
        }

        [TestMethod]
        public void LiteLinkCoding_LiteLink() {
            Random rnd = new Random();
                 
            for (int i = 0; i < 1000; ++i) {
                int number = rnd.Next(9999999);
                //string source = number.ToString();
                string sk = LiteLinkCoding.ShuffleAlpabet(rnd);
                string pk = LiteLinkCoding.RandomPK(rnd);
                string code = number.LiteLinkCode(sk, pk);
                string decode = code.LiteLinkDecode(sk);
                Assert.AreNotEqual(code, decode);
                Assert.IsFalse(code.Contains(number.ToString()));
                int result = decode.ExtractConsNumber(); //int.Parse(decode);
                Assert.AreEqual(number, result);
            }
        }

        [TestMethod]
        public void LiteLinkCoding_LiteLinkGuid() {
            Random rnd = new Random();

            for (int i = 0; i < 1000; ++i) {
                Guid guid = Guid.NewGuid();
                //string source = number.ToString();
                string sk = LiteLinkCoding.ShuffleAlpabet(rnd);
                string pk = LiteLinkCoding.RandomPK(rnd);
                string code = guid.LiteLinkCode(sk, pk);
                string decode = code.LiteLinkDecode(sk);
                Assert.AreNotEqual(code, decode);
                Assert.IsFalse(code.Contains(guid.ToString("N")));
                Guid result = decode.ExtractGuid(); //int.Parse(decode);
                Assert.AreEqual(guid, result);
            }
        }

        [TestMethod]
        public void LiteLinkCoding_ShuffleAlpabet() {
            Random rnd = new Random();
            for (int i = 0; i < 1000; ++i) {
                string sk = LiteLinkCoding.ShuffleAlpabet(rnd);
                Assert.AreEqual(LiteLinkCoding.AlphabetLength, sk.ToCharArray().Distinct().Count());
            }
        }

        [TestMethod]
        public void LiteLinkCoding_CheckSummError() {
            Assert.ThrowsException<ArgumentException>(() => "000001234567890123z".LiteLinkDecode("0123456789abcdefghijklmnopqrstuvwxyz"));
        }

        [TestMethod]
        public void LiteLinkCoding_ExtractConsNumber() {
            Assert.ThrowsException<ArgumentException>(() => "1234".ExtractConsNumber());
        }
    }
}
