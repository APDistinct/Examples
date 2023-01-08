using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL.Model;
using FLChat.DAL;

namespace FLChat.Core.Algorithms.Tests
{
    [TestClass]
    public class MessageStatusChangedStrategyTests
    {
        private ChatEntities entities;
        private MessageStatusChangedStrategy strategy = new MessageStatusChangedStrategy();

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Cleanup() {
            entities.Dispose();
        }

        [TestMethod]
        public void MessageStatusChangedStrategy_Test() {
            User from = entities.GetUser(null, null, TransportKind.FLChat);
            User to = entities.GetUser(u => u.Id != from.Id, null, TransportKind.Test);
            Message msg = entities.SendMessage(from.Id, to.Id, TransportKind.FLChat, TransportKind.Test);
            MessageTransportId msgTId = entities.MessageTransportId.Add(new MessageTransportId() {
                TransportId = "id" + msg.Id.ToString(),
                MsgId = msg.Id,
                ToUserId = to.Id,
                TransportTypeId = (int)TransportKind.Test
            });
            entities.SaveChanges();

            FakeOuterMessageStatus msgStatus = new FakeOuterMessageStatus() {
                IsDelivered = true,
                UserId = to.Transports.Get(TransportKind.Test).TransportOuterId,
                MessageId = msgTId.TransportId
            };

            Assert.IsFalse(msg.ToUser.IsDelivered);
            Assert.IsFalse(msg.ToUser.IsRead);
            Assert.IsFalse(msg.ToUser.IsFailed);

            strategy.Process(entities, msgStatus);

            Assert.IsTrue(msg.ToUser.IsDelivered);
            Assert.IsFalse(msg.ToUser.IsRead);
            Assert.IsFalse(msg.ToUser.IsFailed);

            msgStatus.IsDelivered = false;
            msgStatus.IsRead = true;
            strategy.Process(entities, msgStatus);

            Assert.IsTrue(msg.ToUser.IsDelivered);
            Assert.IsTrue(msg.ToUser.IsRead);
            Assert.IsFalse(msg.ToUser.IsFailed);

            msgStatus.IsRead = false;
            msgStatus.IsFailed = true;
            msgStatus.FailureReason = "test reason";
            strategy.Process(entities, msgStatus);

            Assert.IsTrue(msg.ToUser.IsDelivered);
            Assert.IsTrue(msg.ToUser.IsRead);
            Assert.IsTrue(msg.ToUser.IsFailed);

            Assert.AreEqual("test reason", msg.ToUser.MessageError.First().Descr);
        }
    }
}
