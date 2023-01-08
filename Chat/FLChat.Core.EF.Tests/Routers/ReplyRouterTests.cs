using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL.Model;
using FLChat.DAL;

namespace FLChat.Core.Routers.Tests
{
    [TestClass]
    public class ReplyRouterTests
    {
        ChatEntities entities;
        ReplyRouter router = new ReplyRouter();

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void ReplyRouter_Reply_Test() {
            User user = entities.GetUser(null, null, tk: TransportKind.Test);
            User inner = entities.GetUser(u => u.Id != user.Id, null, tk: TransportKind.FLChat);

            Message initialMsg = entities.SendMessage(inner.Id, user.Id, tot: TransportKind.Test);
            MessageTransportId initialId = new MessageTransportId() {
                MsgId = initialMsg.Id,
                ToUserId = user.Id,
                TransportTypeId = (int)TransportKind.Test,
                TransportId = Guid.NewGuid().ToString()
            };
            entities.MessageTransportId.Add(initialId);
            entities.SaveChanges();

            FakeOuterMessage fakeOuterMessage = new FakeOuterMessage() {
                FromId = user.Transports.Get(TransportKind.Test).TransportOuterId,
                ReplyToMessageId = initialId.TransportId
            };
            Message dbmessage = new Message();

            Guid? addresse = router.RouteMessage(entities, fakeOuterMessage, dbmessage);

            Assert.IsNotNull(addresse);
            Assert.AreEqual(inner.Id, addresse.Value);
            Assert.IsNotNull(dbmessage.AnswerTo);
            Assert.IsNotNull(dbmessage.AnswerToId);
            Assert.AreEqual(initialMsg.Id, dbmessage.AnswerToId);
            Assert.AreEqual(initialMsg.Id, dbmessage.AnswerTo.Id);
        }

        [TestMethod]
        public void ReplyRouter_WithoutReply_Test() {
            FakeOuterMessage fakeOuterMessage = new FakeOuterMessage() {
            };

            Guid? addresse = router.RouteMessage(null, fakeOuterMessage, null);
            Assert.IsNull(addresse);
        }

        [TestMethod]
        public void ReplyRouter_ReplyNotFount_Test() {
            FakeOuterMessage fakeOuterMessage = new FakeOuterMessage() {
                ReplyToMessageId = Guid.NewGuid().ToString()
            };

            Guid? addresse = router.RouteMessage(entities, fakeOuterMessage, null);
            Assert.IsNull(addresse);
        }

    }
}
