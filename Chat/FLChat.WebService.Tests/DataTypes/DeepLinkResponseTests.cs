using System;
using System.Linq;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class DeepLinkResponseTests
    {
        [TestMethod]
        public void GetStatus() {
            Assert.AreEqual(TransportStatus.Subscribed, DeepLinkResponse.GetStatus(true));
            Assert.AreEqual(TransportStatus.Unsubscribed, DeepLinkResponse.GetStatus(false));
            Assert.AreEqual(TransportStatus.None, DeepLinkResponse.GetStatus(null));
        }

        [TestMethod]
        public void FieldNames() {
            DeepLinkResponse resp = new DeepLinkResponse() {
                Code = "",
                User = null,
                InviteButtons = new DeepLinkResponse.InviteLink[] { } };

            string json = JsonConvert.SerializeObject(resp);
            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(
                new string[] { "code", "user", "invite_buttons" },
                jo.Properties().Select(p => p.Name).ToArray());
        }

        [TestMethod]
        public void InviteButtons_FieldNames() {
            DeepLinkResponse.InviteLink link = new DeepLinkResponse.InviteLink() {
                Status = TransportStatus.Subscribed,
                Transport = DAL.TransportKind.Telegram,
                Url = "someurl"
            };
            string json = JsonConvert.SerializeObject(link);
            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(
                new string[] { "transport", "url", "status" },
                jo.Properties().Select(p => p.Name).ToArray());

            Assert.AreEqual("Telegram", jo["transport"]);
            Assert.AreEqual("subscribed", jo["status"]);
        }

        [TestMethod]
        public void ToInviteLinks() {
            using (ChatEntities entities = new ChatEntities()) {
                User user = entities.GetUserQ(transport: DAL.TransportKind.Telegram);
                DeepLinkResponse.InviteLink[] links = user.ToInviteLinks(entities, "123");

                Assert.IsNotNull(links);
                Assert.AreEqual(
                    TransportStatus.Subscribed,
                    links.Where(t => t.Transport == DAL.TransportKind.Telegram).Select(t => t.Status).Single());
            }
        }

        [TestMethod]
        public void ToInviteLinks_NullUser() {
            using (ChatEntities entities = new ChatEntities()) {
                User user = null;
                DeepLinkResponse.InviteLink[] links = user.ToInviteLinks(entities, "123");

                Assert.IsNotNull(links);
                Assert.AreEqual(
                    TransportStatus.None,
                    links.Where(t => t.Transport == DAL.TransportKind.Telegram).Select(t => t.Status).Single());
            }
        }
    }
}
