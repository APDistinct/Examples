using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Core.MsgCompilers.Tests
{
    [TestClass]
    public class TTEscapeTests
    {
        [TestMethod]
        public void TTEscape_Test() {
            string s = "123_*";
            TTEscape tt = new TTEscape(new char[] { '*', '_' });
            Assert.AreEqual("123\\_\\*", tt.Transform(s));
        }
    }
}
