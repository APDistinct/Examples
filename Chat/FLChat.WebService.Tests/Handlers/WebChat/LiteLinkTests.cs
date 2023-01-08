using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FLChat.DAL.Model;
using System.Net;
using FLChat.WebService.DataTypes;

namespace FLChat.WebService.Handlers.WebChat.Tests
{
    [TestClass]
    public class LiteLinkTests
    {
        LiteLink handler = new LiteLink();
        ChatEntities entities;

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void InvalidCode()
        {
            string code = "123abc";
            var e = Assert.ThrowsException<ErrorResponseException>(() => handler.ProcessRequest(entities, null, code));
            Assert.AreEqual((int)HttpStatusCode.BadRequest, e.GetHttpCode());
            Assert.AreEqual(ErrorResponse.Kind.invalid_code, e.Error.Error);
        }

        [TestMethod]
        public void WithUser()
        {
            DAL.Model.User user = entities.GetUserQ(
                where: q => q.Where(u => u.FLUserNumber != null),
                create: u => u.FLUserNumber = new Random().Next(),
                transport: DAL.TransportKind.Telegram);
            
            string code = new Core.Algorithms.LiteDeepLinkStrategy().Generate(user);
            var resp = handler.ProcessRequest(entities, null, code);

            Assert.AreEqual(code, resp.Code);
            Assert.IsNotNull(resp.User);
            Assert.AreEqual(user.FLUserNumber, resp.User.FLUserNumber);
            foreach (var item in resp.InviteButtons)
                Assert.IsTrue(item.Url.Contains(code));
            CollectionAssert.AreEquivalent(
                entities.TransportType.Where(tt => tt.DeepLink != null).ToArray().Select(tt => tt.Kind).ToArray(),
                resp.InviteButtons.Select(i => i.Transport).ToArray());
            Assert.AreEqual(
                TransportStatus.Subscribed,
                resp.InviteButtons.Where(t => t.Transport == DAL.TransportKind.Telegram).Single().Status);
        }

        [TestMethod]
        public void WithoutUser() {
            int number = entities.User.Where(u => u.FLUserNumber != null).Select(u => u.FLUserNumber.Value).Max() + 1;

            string code = new Core.Algorithms.LiteDeepLinkStrategy().Generate(new DAL.Model.User() { FLUserNumber = number });
            var resp = handler.ProcessRequest(entities, null, code);

            Assert.AreEqual(code, resp.Code);
            Assert.IsNull(resp.User);

            Assert.AreEqual(
                TransportStatus.None,
                resp.InviteButtons.Select(t => t.Status).Distinct().Single());
        }
    }
}
