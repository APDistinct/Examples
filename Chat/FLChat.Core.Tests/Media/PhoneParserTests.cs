using System;
using FLChat.Core.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Core.Media.Tests
{
    [TestClass]
    public class PhoneParserTests
    {
        readonly PhoneParser _parser = new PhoneParser();

        [DataTestMethod]
        [DataRow("+79155048340")]
        [DataRow("79155048340")]
        [DataRow("89155048340")]
        [DataRow("+7(915)5048340")]
        [DataRow("7(915)5048340")]
        [DataRow("8(915)5048340")]
        [DataRow("+7915-504-83-40")]
        [DataRow("7915-504-83-40")]
        [DataRow("8915-504-83-40")]
        public void Parse_RussianPhones(string phone)
        {
            var isValid = _parser.TryParse(phone, out var result);

            Assert.IsTrue(isValid);
            Assert.AreEqual("79155048340", result);
        }

        [DataTestMethod]
        [DataRow("+3759155048340")]
        [DataRow("3759155048340")]
        [DataRow("+375(915)5048340")]
        [DataRow("375(915)5048340")]
        [DataRow("+375915-504-83-40")]
        [DataRow("375915-504-83-40")]
        public void Parse_BelarusPhones(string phone)
        {
            var isValid = _parser.TryParse(phone, out var result);

            Assert.IsTrue(isValid);
            Assert.AreEqual("3759155048340", result);
        }

        [DataTestMethod]
        [DataRow("+3809155048340")]
        [DataRow("3809155048340")]
        [DataRow("+380(915)5048340")]
        [DataRow("380(915)5048340")]
        [DataRow("+380915-504-83-40")]
        [DataRow("380915-504-83-40")]
        public void Parse_UkrainePhones(string phone)
        {
            var isValid = _parser.TryParse(phone, out var result);

            Assert.IsTrue(isValid);
            Assert.AreEqual("3809155048340", result);
        }

        [DataTestMethod]
        [DataRow("+9929155048340")]
        [DataRow("9929155048340")]
        [DataRow("+992(915)5048340")]
        [DataRow("992(915)5048340")]
        [DataRow("+992915-504-83-40")]
        [DataRow("992915-504-83-40")]

        [DataRow("+3599155048340")]
        [DataRow("3599155048340")]
        [DataRow("+359(915)5048340")]
        [DataRow("359(915)5048340")]
        [DataRow("+359915-504-83-40")]
        [DataRow("359915-504-83-40")]

        [DataRow("+9949155048340")]
        [DataRow("9949155048340")]
        [DataRow("+994(915)5048340")]
        [DataRow("994(915)5048340")]
        [DataRow("+994915-504-83-40")]
        [DataRow("994915-504-83-40")]

        [DataRow("+9959155048340")]
        [DataRow("9959155048340")]
        [DataRow("+995(915)5048340")]
        [DataRow("995(915)5048340")]
        [DataRow("+995915-504-83-40")]
        [DataRow("995915-504-83-40")]

        [DataRow("+3749155048340")]
        [DataRow("3749155048340")]
        [DataRow("+374(915)5048340")]
        [DataRow("374(915)5048340")]
        [DataRow("+374915-504-83-40")]
        [DataRow("374915-504-83-40")]

        [DataRow("+3719155048340 ")]
        [DataRow("3719155048340")]
        [DataRow("+371(915)5048340")]
        [DataRow("371(915)5048340")]
        [DataRow("+371915-504-83-40")]
        [DataRow("371915-504-83-40")]
        public void Parse_Phones(string phone)
        {
            var isValid = _parser.TryParse(phone, out var result);

            Assert.IsTrue(isValid);
            Assert.IsTrue(result.EndsWith("9155048340"));

            //Assert.AreEqual("3809155048340", result);
        }

        [DataTestMethod]
        [DataRow("+380w9155048340")]
        [DataRow("3809155e048340")]
        [DataRow("+3@80(915)5048340")]
        [DataRow("3110(915)5+-048345645645640")]
        [DataRow("2+380915-504-83-40")]
        public void Parse_WrongPhones(string phone)
        {
            var isValid = _parser.TryParse(phone, out var result);

            Assert.IsFalse(isValid);
        }
    }
}
