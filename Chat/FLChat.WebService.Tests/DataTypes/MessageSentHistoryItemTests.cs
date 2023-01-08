using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class MessageSentHistoryItemTests
    {
        [TestMethod]
        public void MessageSentHistoryItem_Serialize() {
            MessageSentHistoryItem item = new MessageSentHistoryItem(new DAL.Model.MessageStatsGroupedView(),
                new DAL.Model.Message() { Kind = DAL.MessageKind.Broadcast });

            string json = JsonConvert.SerializeObject(item);
            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(
                new string[] { "id", "tm", "kind", "text", "file", "stats", "tm_started" },
                jo.Properties().Select(p => p.Name).ToArray());
            Assert.AreEqual("Broadcast", (string)jo["kind"]);

            item.User = new UserInfoAdmin(new DAL.Model.User());

            json = JsonConvert.SerializeObject(item);
            jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(
                new string[] { "id", "tm", "kind", "text", "file", "stats", "tm_started", "user" },
                jo.Properties().Select(p => p.Name).ToArray());
        }

        [TestMethod]
        public void MessageSentHistoryItem_Constr() {
            DAL.Model.Message msg = new DAL.Model.Message() {
                Kind = DAL.MessageKind.Broadcast,
                PostTm = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                Text = "some text",
            };
            MessageSentHistoryItem item = new MessageSentHistoryItem(new DAL.Model.MessageStatsGroupedView(), msg);
            Assert.AreEqual(msg.Id, item.Id);
            Assert.AreEqual(msg.PostTm, item.PostTm);
            Assert.AreEqual(msg.Kind, item.Kind);
            Assert.AreEqual(msg.Text, item.Text);
            Assert.IsNotNull(item.Stats);
            Assert.IsNull(item.File);
        }

        [TestMethod]
        public void MessageSentHistoryItem_Constr_WithFile() {
            DAL.Model.FileInfo fi = new DAL.Model.FileInfo() {
                Id = Guid.NewGuid(),
                MediaType = new DAL.Model.MediaType()
            };
            MessageSentHistoryItem item = new MessageSentHistoryItem(new DAL.Model.MessageStatsGroupedView(),
                new DAL.Model.Message() { FileId = fi.Id }, 
                fi: fi);
            Assert.IsNotNull(item.File);
        }
    }
}
