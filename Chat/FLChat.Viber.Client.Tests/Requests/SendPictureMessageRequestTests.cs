using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.Viber.Client.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace FLChat.Viber.Client.Requests.Tests
{
    [TestClass]
    public class SendPictureMessageRequestTests
    {
        [TestMethod]
        public void SendPictureMessageRequest_Serialize() {
            Sender sender = new Types.Sender() {
                Name = "John McClane",
                Avatar = "http://avatar.example.com"
            };
            SendPictureMessageRequest req = new SendPictureMessageRequest(sender, "123", "descr", "www.url.com");
            string json = JsonConvert.SerializeObject(req);
            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(
                new string[] { "receiver", "sender", "type", "text", "media" },
                jo.Properties().Select(p => p.Name).ToArray());
            Assert.AreEqual("picture", (string)jo["type"]);

            req.Thumbnail = "www.url.com/sub";
            json = JsonConvert.SerializeObject(req);
            jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(
                new string[] { "receiver", "sender", "type", "text", "media", "thumbnail" },
                jo.Properties().Select(p => p.Name).ToArray());
        }
    }
}
