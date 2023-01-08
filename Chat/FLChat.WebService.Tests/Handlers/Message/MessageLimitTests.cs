using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;

namespace FLChat.WebService.Handlers.Message.Tests
{
    [TestClass]
    public class MessageLimitTests
    {
        ChatEntities entities;
        MessageLimit handler = new MessageLimit();

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void MessageLimit_Test() {
            MessageCountOverToday record = entities
                .MessageCountOverToday
                .Where(c => c.Count > 0 && c.MessageTypeId == (int)MessageKind.Broadcast)
                .FirstOrDefault();
            if (record == null) {
                DAL.Model.User[] users = entities.GetUsers(2, null, null, TransportKind.FLChat);
                entities.SendMessage(users[0].Id, users[1].Id, kind: MessageKind.Broadcast);
                record = entities
                    .MessageCountOverToday
                    .Where(c => c.Count > 0 && c.MessageTypeId == (int)MessageKind.Broadcast)
                    .FirstOrDefault();
            }
            DAL.Model.User user = entities.User.Where(u => u.Id == record.FromUserId).Single();
            DAL.Model.User addr = entities.GetUser(u => u.Id != user.Id, null, TransportKind.FLChat);

            LimitInfo resp = handler.ProcessRequest(entities, user, new DataTypes.SendMessageLimitRequest() {
                Type = MessageKind.Broadcast,
                Selection = new DataTypes.UserSelection() {
                    Include = new System.Collections.Generic.List<Guid>() { addr.Id }
                }
            });

            MessageType messageType = entities.MessageType.Where(mt => mt.Id == (int)MessageKind.Broadcast).Single();

            Assert.IsNotNull(resp);
            Assert.AreEqual(MessageKind.Broadcast, resp.Type);
            Assert.AreEqual(messageType.LimitForDay, resp.LimitForDay);
            Assert.AreEqual(messageType.LimitForOnce, resp.LimitForOnce);
            Assert.AreEqual(record.Count, resp.AlreadySent);
            Assert.AreEqual(1, resp.SelectionCount);
        }
    }
}
