using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.Viber.Client.Types;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace FLChat.Viber.Client.Requests.Tests
{
    [TestClass]
    public class SendUrlMessageRequestTests
    {
        [TestMethod]
        public void SendUrlMessageRequest_Serialize() {
            Sender sender = new Types.Sender() {
                Name = "John McClane",
                Avatar = "http://avatar.example.com"
            };
            SendUrlMessageRequest req = new SendUrlMessageRequest(sender, "123", "www.url.com");
            string json = JsonConvert.SerializeObject(req);
            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(
                new string[] { "receiver", "sender", "type", "media" },
                jo.Properties().Select(p => p.Name).ToArray());
            Assert.AreEqual("url", (string)jo["type"]);
        }
    }
}
