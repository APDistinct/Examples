using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class MessageSentInfoResponseTests
    {
        [TestMethod]
        public void MessageSentInfoResponse_Serialize() {
            MessageSentInfoResponse resp = new MessageSentInfoResponse(
                new DAL.Model.MessageStatsGroupedView(),
                new DAL.Model.MessageStatsRowsView[] {},
                new DAL.Model.Message() { Kind = DAL.MessageKind.Broadcast });
            string json = JsonConvert.SerializeObject(resp);
            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(
                new string[] { "id", "tm", "kind", "text", "file", "stats", "recipients", "tm_started" },
                jo.Properties().Select(p => p.Name).ToArray());
            Assert.AreEqual("Broadcast", (string)jo["kind"]);
        }
    }
}
