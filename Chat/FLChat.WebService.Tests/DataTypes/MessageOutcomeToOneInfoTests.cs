using System;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class MessageOutcomeToOneInfoTests
    {
        /// <summary>
        /// Verify json fields
        /// </summary>
        [TestMethod]
        public void MessageOutcomeToOneInfo_Serialize() {
            MessageToUser to = new MessageToUser() {
                IsDelivered = false,
                IsFailed = false,
                IsRead = false,
                IsSent = false,
                ToTransportKind = DAL.TransportKind.FLChat,
                ToUserId = Guid.NewGuid(),
                Message = new Message() {
                    IsDeleted = false,
                    FromTransportKind = DAL.TransportKind.FLChat,
                    FromUserId = Guid.NewGuid(),
                    Kind = DAL.MessageKind.Personal,
                    Text = "test"
                }
            };
            MessageOutcomeToOneInfo info = new MessageOutcomeToOneInfo(to);
            string jsonString = JsonConvert.SerializeObject(info);
            JObject json = JObject.Parse(jsonString);
            Assert.AreEqual("quequed", (string)json["status"]);
            Assert.AreEqual(to.ToUserId, (Guid)json["to_user"]);
            Assert.AreEqual("FLChat", (string)json["to_transport"]);
            Assert.IsTrue(json.ContainsKey("tm"));
            Assert.AreEqual(to.Message.FromUserId, (Guid)json["from"]);
            Assert.AreEqual("FLChat", (string)json["transport"]);
            Assert.IsFalse((bool)json["incoming"]);
            Assert.AreEqual(to.Message.Text, (string)json["text"]);
            Assert.IsTrue(json.ContainsKey("id"));
        }
    }
}
