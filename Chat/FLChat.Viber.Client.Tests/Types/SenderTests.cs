using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.Viber.Client.Types.Tests
{
    [TestClass]
    public class SenderTests
    {
        [TestMethod]
        public void Sender_Serialize() {
            Sender sender = new Sender() {
                Name = "John McClane",
                Avatar = "http://avatar.example.com"
            };
            string json = JsonConvert.SerializeObject(sender);
            JObject jo = JObject.Parse(json);

            CollectionAssert.AreEquivalent(
               new string[] { "name", "avatar" },
               jo.Properties().Select(p => p.Name).ToArray());
        }

        [TestMethod]
        public void Sender_Serialize_Short() {
            Sender sender = new Sender() {
                Name = "John McClane"
            };
            string json = JsonConvert.SerializeObject(sender);
            JObject jo = JObject.Parse(json);

            CollectionAssert.AreEquivalent(
               new string[] { "name" },
               jo.Properties().Select(p => p.Name).ToArray());
        }
    }
}
