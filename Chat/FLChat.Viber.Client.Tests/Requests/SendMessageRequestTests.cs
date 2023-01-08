using System;
using FLChat.Viber.Client.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace FLChat.Viber.Client.Requests.Tests
{
    [TestClass]
    public class SendMessageRequestTests
    {
        public class TestSendMessageRequest : SendMessageRequest
        {
            public TestSendMessageRequest(Sender sender, string receiver, MessageType type = MessageType.Text) : base(sender, receiver, type) {
            }
        }

        [TestMethod]
        public void SendMessageRequest_Serialize_Full() {
            Sender sender = new Types.Sender() {
                Name = "John McClane",
                Avatar = "http://avatar.example.com"
            };
            TestSendMessageRequest req = new TestSendMessageRequest(sender, "01234567890A=") {
                MinApiVersion = 1,
                TrackingData = "tracking data",
            };
            Assert.AreEqual(HttpMethod.Post, req.Method);

            string json = JsonConvert.SerializeObject(req);
            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(
                new string[] { "receiver", "min_api_version", "sender", "tracking_data", "type" },
                jo.Properties().Select(p => p.Name).ToArray());
            Assert.AreEqual("text", (string)jo["type"]);

            CollectionAssert.AreEquivalent(
                new string[] { "name", "avatar" },
                (jo["sender"] as JObject).Properties().Select(p => p.Name).ToArray());
        }

        [TestMethod]
        public void SendMessageRequest_Serialize_Short() {
            Sender sender = new Types.Sender() {
                Name = "John McClane"
            };
            TestSendMessageRequest req = new TestSendMessageRequest(sender, "01234567890A=");
            string json = JsonConvert.SerializeObject(req);
            JObject jo = JObject.Parse(json);

            CollectionAssert.AreEquivalent(
                new string[] { "receiver", "sender", "type" },
                jo.Properties().Select(p => p.Name).ToArray());
        }

        [TestMethod]
        public void SendMessageRequest_Serialize_WithoutReceiver() {
            Sender sender = new Types.Sender() {
                Name = "John McClane"
            };
            TestSendMessageRequest req = new TestSendMessageRequest(sender, null);
            string json = JsonConvert.SerializeObject(req);
            JObject jo = JObject.Parse(json);

            CollectionAssert.AreEquivalent(
                new string[] { "sender", "type" },
                jo.Properties().Select(p => p.Name).ToArray());
        }

        [TestMethod]
        public void SendMessageRequest_Keyboard() {
            TestSendMessageRequest req = new TestSendMessageRequest(new Sender("na"), "1111") {
                Keyboard = new Keyboard()
            };
            string json = JsonConvert.SerializeObject(req);
            JObject jo = JObject.Parse(json);

            Assert.IsTrue(jo.ContainsKey("keyboard"));
        }
    }
}
