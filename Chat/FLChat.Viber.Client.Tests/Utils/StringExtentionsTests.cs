using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Viber.Client.Utils
{
    [TestClass]
    public class StringExtentionsTests
    {
        [TestMethod]
        public void StringExtentions_RemoveSpacesAndNewLines() {
            Assert.AreEqual("123456789", ("123 456" + Environment.NewLine + "789").RemoveSpacesAndNewLines());
        }

        [TestMethod]
        public void StringExtentions_HMACSHA256String() {
            Assert.AreEqual("4e4feaea959d426155a480dc07ef92f4754ee93edbe56d993d74f131497e66fb", "1234".HMACSHA256String("1234"));
            Assert.AreEqual("7b403b17e5db50cfe2a9523190ab5cf405bdb669603c31ec523c9ee1d52e1ab9", "somedata".HMACSHA256String("ieHae0rietu0Okahs6eiR6quoh6eiyi0"));
        }

        //[TestMethod]
        //public void StringExtentions_ViberContentSignature() {
        //    string json = File.ReadAllText(".\\Json\\content_signature.txt");
        //    const string token = "4453b6ac12345678-e02c5f12174805f9-daec9cbb5448c51f";
        //    string hash = json.RemoveSpacesAndNewLines().HMACSHA256String(token);
        //    Assert.AreEqual("9d3941b33d45c165400d84dba9328ee0b687a5a18b347617091be0a56d", hash);
        //}
    }
}
