using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class WebChatMessageResponseTests
    {
        [TestMethod]
        public void WebChatMessageResponse_InviteButtonsTransport() {
            WebChatReadResponse resp = new WebChatReadResponse() {
                Code = "",
                InviteButtons = new WebChatReadResponse.InviteLink[] {
                    new WebChatReadResponse.InviteLink() {
                        Transport = DAL.TransportKind.Telegram,
                        Url = "someurl"
                    }
                }
            };

            string jsonstr = JsonConvert.SerializeObject(resp);

            JObject json = JObject.Parse(jsonstr);
            Assert.AreEqual("Telegram", (string)json["invite_buttons"][0]["transport"]);
        }
    }
}
