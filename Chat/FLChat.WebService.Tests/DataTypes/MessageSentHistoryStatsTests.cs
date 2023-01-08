using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class MessageSentHistoryStatsTests
    {
        [TestMethod]
        public void MessageSentHistoryStats_Serialize() {
            MessageSentHistoryStats stats = new MessageSentHistoryStats(new DAL.Model.MessageStatsGroupedView());
            string json = JsonConvert.SerializeObject(stats);
            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(
                new string[] { "recipient_count", "web_chat_count", "failed_count", "sent_count", "quequed_count",
                "cant_send_count", "web_chat_accepted_count", "sms_url_opened_count", "state"},
                jo.Properties().Select(p => p.Name).ToArray());
        }

        [TestMethod]
        public void MessageSentHistoryStats_Stats() {
            MessageSentHistoryStats stats = new MessageSentHistoryStats(new DAL.Model.MessageStatsGroupedView() {
                QuequedCount = 10
            });
            Assert.AreEqual(MessageSentHistoryStats.SendingState.Quequed, stats.State);
            JObject jo = JObject.Parse(JsonConvert.SerializeObject(stats));
            Assert.AreEqual("quequed", (string)jo["state"]);

            stats = new MessageSentHistoryStats(new DAL.Model.MessageStatsGroupedView() {
                QuequedCount = 10,
                SentCount = 1
            });
            Assert.AreEqual(MessageSentHistoryStats.SendingState.InProgress, stats.State);
            jo = JObject.Parse(JsonConvert.SerializeObject(stats));
            Assert.AreEqual("in_progress", (string)jo["state"]);

            stats = new MessageSentHistoryStats(new DAL.Model.MessageStatsGroupedView() {
                QuequedCount = 10,
                FailedCount = 1
            });
            Assert.AreEqual(MessageSentHistoryStats.SendingState.InProgress, stats.State);
            jo = JObject.Parse(JsonConvert.SerializeObject(stats));
            Assert.AreEqual("in_progress", (string)jo["state"]);

            stats = new MessageSentHistoryStats(new DAL.Model.MessageStatsGroupedView() {
                SentCount = 10
            });
            Assert.AreEqual(MessageSentHistoryStats.SendingState.Complete, stats.State);
            jo = JObject.Parse(JsonConvert.SerializeObject(stats));
            Assert.AreEqual("complete", (string)jo["state"]);

            stats = new MessageSentHistoryStats(new DAL.Model.MessageStatsGroupedView()
            {
                QuequedCount = 10,
                FailedCount = 1
            }, true);
            Assert.AreEqual(MessageSentHistoryStats.SendingState.Cancelled, stats.State);
            jo = JObject.Parse(JsonConvert.SerializeObject(stats));
            Assert.AreEqual("cancelled", (string)jo["state"]);
        }
    }
}
