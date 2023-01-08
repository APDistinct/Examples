using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.Viber.Client.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace FLChat.Viber.Client.Requests.Tests
{
    [TestClass]
    public class SendFileMessageRequestTests
    {
        [TestMethod]
        public void SendFileMessageRequest_Serialize() {
            Sender sender = new Types.Sender() {
                Name = "John McClane",
                Avatar = "http://avatar.example.com"
            };
            SendFileMessageRequest req = new SendFileMessageRequest(sender, "123", "www.url.com", 100, "name.txt");
            string json = JsonConvert.SerializeObject(req);
            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(
                new string[] { "receiver", "sender", "type", "media", "size", "file_name" },
                jo.Properties().Select(p => p.Name).ToArray());
            Assert.AreEqual("file", (string)jo["type"]);
        }

        [TestMethod]
        public void SendFileMessageRequest_LongFileName() {
            Sender sender = new Types.Sender() {
                Name = "John McClane",
                Avatar = "http://avatar.example.com"
            };
            string filename = "a" + new String('x', SendFileMessageRequest.MaxFileSize) + "z.txt";
            Assert.IsTrue(filename.Length > SendFileMessageRequest.FileNameMaxLength);

            SendFileMessageRequest req = new SendFileMessageRequest(sender, "123", "www.url.com", 100, filename);
            Assert.AreEqual(req.FileName.Length, SendFileMessageRequest.FileNameMaxLength);
            Assert.IsTrue(req.FileName.StartsWith("ax"));
            Assert.IsTrue(req.FileName.EndsWith(".txt"));
        }
    }
}
