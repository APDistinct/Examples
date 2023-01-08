using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using FLChat.Viber.Client.Types;

namespace FLChat.Viber.Client.Requests.Tests
{
    [TestClass]
    public class SendTextMessageRequestTests
    {
        [TestMethod]
        public void SendTextMessageRequest_Serialize_Full() {
            Sender sender = new Types.Sender() {
                Name = "John McClane",
                Avatar = "http://avatar.example.com"
            };
            SendTextMessageRequest req = new SendTextMessageRequest(sender, "01234567890A=", "Hello world!") {
                MinApiVersion = 1,
                TrackingData = "tracking data",
            };
            Assert.AreEqual(HttpMethod.Post, req.Method);

            string json = JsonConvert.SerializeObject(req);
            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(
                new string[] { "receiver", "min_api_version", "sender", "tracking_data", "type", "text" },
                jo.Properties().Select(p => p.Name).ToArray());
            CollectionAssert.AreEquivalent(
                new string[] { "name", "avatar" }, 
                (jo["sender"] as JObject).Properties().Select(p => p.Name).ToArray());

            Assert.AreEqual("text", (string)jo["type"]);
        }

        [TestMethod]
        public void SendTextMessageRequest_Serialize_Short() {
            Sender sender = new Types.Sender() {
                Name = "John McClane"
            };
            SendTextMessageRequest req = new SendTextMessageRequest(sender, "01234567890A=", "Hello world!");
            string json = JsonConvert.SerializeObject(req);
            JObject jo = JObject.Parse(json);

            CollectionAssert.AreEquivalent(
                new string[] { "receiver", "sender", "type", "text" },
                jo.Properties().Select(p => p.Name).ToArray());
        }

        [TestMethod]
        public void SendTextMessageRequest_Serialize_WithoutReceiver() {
            Sender sender = new Types.Sender() {
                Name = "John McClane"
            };
            SendTextMessageRequest req = new SendTextMessageRequest(sender, null, "Hello world!");
            string json = JsonConvert.SerializeObject(req);
            JObject jo = JObject.Parse(json);

            CollectionAssert.AreEquivalent(
                new string[] { "sender", "type", "text" },
                jo.Properties().Select(p => p.Name).ToArray());
        }
    }
}
