using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.DAL.Import.Tests
{
    [TestClass]
    public class StringExtentionsTests
    {
        [TestMethod]
        public void RemoveNonDigitsTest() {
            Assert.AreEqual("79153639939", "+7 (915) 3639939".RemoveNonDigits());
            Assert.AreEqual("99364143485", "993 (64) 143485".RemoveNonDigits());
            Assert.AreEqual("99366436799", "993 66436799".RemoveNonDigits());
            Assert.AreEqual("123", "123".RemoveNonDigits());
        }

        [TestMethod]
        public void RemoveNonDigitsTest_Same() {
            string s = "123";
            Assert.AreSame(s, s.RemoveNonDigits());
        }
    }
}
