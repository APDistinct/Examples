using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Viber.Bot.Tests
{
    [TestClass]
    public class StringExtentionsTests
    {
        [TestMethod]
        public void StringExtentions_CutFullName() {
            Assert.AreEqual("Мельникова Светлана", "Мельникова Светлана Александровна".CutFullName(28));
            Assert.AreEqual("Мельникова Светлана", "Мельникова Светлана".CutFullName(28));
            Assert.AreEqual("МельниковаСветланаАлексан...", "МельниковаСветланаАлександровна".CutFullName(28));
            Assert.AreEqual("М", "М ельниковаСветланаАлександровна".CutFullName(28));
            Assert.AreEqual(" МельниковаСветланаАлекса...", " МельниковаСветланаАлександровна".CutFullName(28));
        }

        [TestMethod]
        public void StringExtentions_CutFullName_AreSame() {
            string str = "Мельникова Светлана";
            Assert.AreSame(str, str.CutFullName(28));
        }
    }
}
