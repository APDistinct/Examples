using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class MessageContentInfoTests
    {
        [TestMethod]
        public void MessageContentInfo_Serialize() {
            MessageContentInfo item = new MessageContentInfo(new DAL.Model.Message() { Kind = DAL.MessageKind.Broadcast });

            string json = JsonConvert.SerializeObject(item);
            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(
                new string[] { "id", "tm", "kind", "text", "file", "tm_started" },
                jo.Properties().Select(p => p.Name).ToArray());
            Assert.AreEqual("Broadcast", (string)jo["kind"]);
        }

        [TestMethod]
        public void MessageContentInfo_Constr() {
            DAL.Model.Message msg = new DAL.Model.Message() {
                Kind = DAL.MessageKind.Broadcast,
                PostTm = DateTime.UtcNow,
                DelayedStart = DateTime.UtcNow.AddDays(1),
                Id = Guid.NewGuid(),
                Text = "some text",
            };
            MessageContentInfo item = new MessageContentInfo(msg);
            Assert.AreEqual(msg.Id, item.Id);
            Assert.AreEqual(msg.PostTm, item.PostTm);
            Assert.AreEqual(msg.DelayedStart, item.DelayedStart);
            Assert.AreEqual(msg.Kind, item.Kind);
            Assert.AreEqual(msg.Text, item.Text);
            Assert.IsNull(item.File);
        }

        [TestMethod]
        public void MessageContentInfo_Constr_WithFile() {
            DAL.Model.FileInfo fi = new DAL.Model.FileInfo() {
                Id = Guid.NewGuid(),
                MediaType = new DAL.Model.MediaType()
            };
            MessageContentInfo item = new MessageContentInfo(
                new DAL.Model.Message() { FileId = fi.Id },
                fi: fi);
            Assert.IsNotNull(item.File);
        }
    }
}
