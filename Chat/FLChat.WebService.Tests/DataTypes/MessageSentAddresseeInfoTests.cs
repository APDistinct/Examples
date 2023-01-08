using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FLChat.DAL;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class MessageSentAddresseeInfoTests
    {
        [TestMethod]
        public void MessageSentAddresseeInfo_Serialize() {
            MessageSentAddresseeInfo info = new MessageSentAddresseeInfo(new DAL.Model.MessageStatsRowsView() {
                ToTransportTypeId = (int)TransportKind.WebChat,
                ToUserId = Guid.NewGuid()
            });
            string json = JsonConvert.SerializeObject(info);
            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(
                new string[] { "user", "transport", "is_web_chat", "is_failed", "is_sent", "is_quequed",
                    "cant_send", "is_web_chat_accepted", "is_web_form_requested" },
                jo.Properties().Select(p => p.Name).ToArray());

            Assert.AreEqual("WebChat", (string)jo["transport"]);
        }
    }
}
