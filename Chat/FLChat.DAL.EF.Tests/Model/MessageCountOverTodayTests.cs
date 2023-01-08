using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.DAL.Model.Tests
{
    [TestClass]
    public class MessageCountOverTodayTests
    {
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
        public void MessageCountForTodayExtentions_GetLimitInfo() {
            MessageCountOverToday record = entities
                .MessageCountOverToday
                .Where(c => c.Count > 0 && c.MessageTypeId == (int)MessageKind.Broadcast)
                .FirstOrDefault();
            if (record == null) {
                User []users = entities.GetUsers(2, u => u.Enabled, null, TransportKind.FLChat);
                entities.SendMessage(users[0].Id, users[1].Id, kind: MessageKind.Broadcast);
                record = entities
                    .MessageCountOverToday
                    .Where(c => c.Count > 0 && c.MessageTypeId == (int)MessageKind.Broadcast)
                    .FirstOrDefault();
            }
            Assert.IsNotNull(record);

            var li = entities.GetLimitInfo(record.FromUserId, MessageKind.Broadcast);
            Assert.IsNotNull(li);
            Assert.AreEqual(MessageKind.Broadcast, li.MessageType.Kind);
            Assert.AreEqual(record.MessageTypeId, li.MessageType.Id);
            Assert.AreEqual(record.MessageTypeId, li.SentCount.MessageTypeId);
            Assert.AreEqual(record.FromUserId, li.SentCount.FromUserId);
            Assert.AreEqual(record.Count, li.SentCount.Count);
        }

        [TestMethod]
        public void MessageCountForTodayExtentions_GetLimitInfo_Empty() {
            var m = entities.MessageCountOverToday.Select(r => r.FromUserId);
            var q = (from u in entities.User
                     where m.Contains(u.Id) == false
                     select u.Id);
            Guid? userId = q.FirstOrDefault();
            if (userId == null)
                userId = entities.GetUser(u => false, null).Id;

            var li = entities.GetLimitInfo(userId.Value, MessageKind.Broadcast);
            Assert.IsNotNull(li);
            Assert.IsNull(li.SentCount);
            Assert.IsNotNull(li.MessageType);
            Assert.AreEqual(MessageKind.Broadcast, li.MessageType.Kind);
        }
    }
}
