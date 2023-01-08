using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;

namespace FLChat.Viber.Client.Types.Tests
{
    [TestClass]
    public class MessageTests
    {
        [TestMethod]
        public void Message_Deserialize() {
            string json = File.ReadAllText(".\\Json\\message.json");
            Message msg = JsonConvert.DeserializeObject<Message>(json);
            Assert.AreEqual(MessageType.Text, msg.Type);
            Assert.AreEqual("a message to the service", msg.Text);
            Assert.AreEqual(@"http://example.com", msg.Media);
            Assert.IsNotNull(msg.Location);
            Assert.AreEqual("tracking data", msg.TrackingData);
            Assert.IsNull(msg.Contact);
            Assert.IsNull(msg.FileName);
            Assert.IsNull(msg.FileSize);
            Assert.IsNull(msg.Duration);
            Assert.IsNull(msg.StickerId);
        }
    }
}
