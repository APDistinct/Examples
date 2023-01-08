using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Core.MsgCompilers.Tests
{
    [TestClass]
    public class StringExtentionsTests
    {
        [TestMethod]
        public void StringExtentions_Escape() {
            char[] symbols = new char[] { '_', '*' };

            string str = "123";
            Assert.AreSame(str, str.EscapeSymbols(symbols));
            Assert.AreEqual("123\\_456\\*789", "123_456*789".EscapeSymbols(symbols));
            Assert.AreEqual("\\_123", "_123".EscapeSymbols(symbols));
            Assert.AreEqual("123\\_", "123_".EscapeSymbols(symbols));
        }

        [TestMethod]
        public void StringExtentions_EscapeMarkdown() {
            Assert.AreEqual("1\\_2\\*3\\`4", "1_2*3`4".EscapeMarkdownSymbols());
        }
    }
}
