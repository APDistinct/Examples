using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

using Newtonsoft.Json;

using FLChat.DAL;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class SendMessageRequestTests
    {
        [TestMethod]
        public void SendMessageRequest_Serialize()
        {
            SendMessageRequest stats = new SendMessageRequest();
            string json = JsonConvert.SerializeObject(stats);
            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(
                new string[] { "type", "selection", "to_user", "to_segment", "to_transport",
                "to_users", "to_segments", "text", "file", "file_id", "delayed_start", "to_phones", "to_phone_list"},
                jo.Properties().Select(p => p.Name).ToArray());
        }

        [TestMethod]
        public void SendMessageRequest_Deserialize_WithoutTransport() {
            const string json = @"{type: 'personal', text: 'test', to_user: 'CF4034FF-FE46-E911-82E7-1C1B0DAFBCAE'}";
            SendMessageRequest msg = JsonConvert.DeserializeObject<SendMessageRequest>(json);
            Assert.AreEqual(MessageKind.Personal, msg.Type);
            Assert.AreEqual("test", msg.Text);
            Assert.AreEqual(Guid.Parse("CF4034FF-FE46-E911-82E7-1C1B0DAFBCAE"), msg.ToUser);
        }

        [TestMethod]
        public void SendMessageRequest_Deserialize_WithTransport() {
            const string json = @"{type: 'group', text: 'test', to_user: 'CF4034FF-FE46-E911-82E7-1C1B0DAFBCAE', to_transport: 'FLChat'}";
            SendMessageRequest msg = JsonConvert.DeserializeObject<SendMessageRequest>(json);
            Assert.AreEqual(MessageKind.Group, msg.Type);
            Assert.AreEqual("test", msg.Text);
            Assert.AreEqual(TransportKind.FLChat, msg.ToTransport);
        }

        [TestMethod]
        public void SendMessageRequest_Deserialize_Broadcast() {
            SendMessageRequest req = JsonConvert.DeserializeObject<SendMessageRequest>(
                File.ReadAllText("./json/SendMessageRequest_Broadcast.json"));
            Assert.AreEqual(MessageKind.Broadcast, req.Type);
            Assert.AreEqual(1, req.ToSegments.Count());
            Assert.AreEqual(3, req.ToUsers.Count());
            Assert.AreEqual(TransportKind.FLChat, req.ToUsers.First().ToTransport);
            Assert.IsNotNull(req.ToUsers.First().ToUser);
        }

        [TestMethod]
        public void SendMessageRequest_Deserialize_DelayedStart()
        {
            string json = @"{type: 'personal', text: 'test', to_user: 'CF4034FF-FE46-E911-82E7-1C1B0DAFBCAE'}";
            SendMessageRequest msg = JsonConvert.DeserializeObject<SendMessageRequest>(json);
            Assert.AreEqual(null, msg.DelayedStart);
            var ds = DateTime.UtcNow.AddDays(1);
            var delayedStartString = JsonConvert.SerializeObject(ds);
            json = @"{type : 'personal', text: 'test', to_user: 'CF4034FF-FE46-E911-82E7-1C1B0DAFBCAE' ,'delayed_start' : " + delayedStartString + "}";
            msg = JsonConvert.DeserializeObject<SendMessageRequest>(json);
            Assert.AreEqual(ds, msg.DelayedStart);            
        }

        [TestMethod]
        public void SendMessageRequest_Deserialize_DelayedStart_Format()
        {
            string json = @"{type: 'personal', text: 'test', to_user: 'CF4034FF-FE46-E911-82E7-1C1B0DAFBCAE', 'delayed_start' : '2020-04-12T12:53Z'}";
            SendMessageRequest msg = JsonConvert.DeserializeObject<SendMessageRequest>(json);            
            Assert.IsNotNull(msg.DelayedStart);
        }
    }
}
