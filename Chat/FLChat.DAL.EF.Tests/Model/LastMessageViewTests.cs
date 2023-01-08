using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace FLChat.DAL.Model.Tests
{
    [TestClass]
    public class LastMessageViewTests
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
        public void LastMessageViewTest() {
            User user = entities.GetUser(
                u => u.Enabled && u.Transports.Where(t =>
                    t.TransportTypeId == (int)TransportKind.FLChat && t.Enabled
                    && t.Messages.Where(m => m.IsDeleted == false && m.ToUsers.Select(mtu => mtu.ToUserId)
                        .Distinct().Count() > 1).Any()).Any(),
                u => throw new Exception("create user for this situation too hard, or maybe I am too lazy"));

            User[] to = user
                .Transports
                .Get(TransportKind.FLChat)
                .Messages
                .Where(m => m.IsDeleted == false)
                .SelectMany(m => m.ToUsers)
                .Select(mtu => mtu.ToTransport.User)
                .Distinct().Take(2)
                .ToArray();
            Guid[] toIds = to.Select(u => u.Id).ToArray();

            Assert.AreEqual(2, to.Length);

            Dictionary<Guid, MessageToUser> map = entities.GetLastMessages(user.Id, toIds);
            Assert.AreEqual(2, map.Count);
            CollectionAssert.AreEquivalent(toIds, map.Keys.ToArray());

            foreach(MessageToUser mtu in map.Values) {
                Assert.IsTrue(mtu.ToUserId == user.Id || mtu.Message.FromUserId == user.Id);
                Assert.IsTrue(toIds.Contains(mtu.ToUserId) || toIds.Contains(mtu.Message.FromUserId));
            }
        }

        [TestMethod]
        public void GetLastMessageViewForContact_Test() {
            User user = entities.GetUser(
                u => u.Enabled && u.Transports.Where(t =>
                    t.TransportTypeId == (int)TransportKind.FLChat && t.Enabled
                    && t.Messages.Where(m => m.IsDeleted == false && m.ToUsers.Select(mtu => mtu.ToUserId)
                        .Distinct().Count() > 1).Any()).Any(),
                u => throw new Exception("create user for this situation too hard, or maybe I am too lazy"));
            LastMessageView[] lmv = entities.GetLastMessageViewForContact(user.Id);
            Assert.IsTrue(lmv.Length > 0);
        }

        [TestMethod]
        public void GetLastMessageViewForContact_OffsetCount() {
            User user = entities.GetUser(
                u => u.Enabled && u.Transports.Where(t =>
                    t.TransportTypeId == (int)TransportKind.FLChat && t.Enabled
                    && t.Messages.Where(m => m.IsDeleted == false && m.ToUsers.Select(mtu => mtu.ToUserId)
                        .Distinct().Count() > 2).Any()).Any(),
                u => throw new Exception("create user for this situation too hard, or maybe I am too lazy"));
            LastMessageView[] lmv = entities.GetLastMessageViewForContact(user.Id);
            LastMessageView[] lmvLimited = entities.GetLastMessageViewForContact(user.Id, 0, 2);
            Assert.IsTrue(lmv.Length > 2);
            Assert.IsTrue(lmvLimited.Length <= 2);

            LastMessageView[] lmvLimited2 = entities.GetLastMessageViewForContact(user.Id, 2, 2);
            Assert.IsTrue(lmvLimited2.Length <= 2);

            Assert.AreEqual(0, lmvLimited.Select(l => l.MsgId).Intersect(lmvLimited2.Select(l => l.MsgId)).Count());
        }

        [TestMethod]
        public void GetLastMessageViewCountForContact_Test() {
            User user = entities.GetUser(
                u => u.Enabled && u.Transports.Where(t =>
                    t.TransportTypeId == (int)TransportKind.FLChat && t.Enabled
                    && t.Messages.Where(m => m.IsDeleted == false && m.ToUsers.Select(mtu => mtu.ToUserId)
                        .Distinct().Count() > 2).Any()).Any(),
                u => throw new Exception("create user for this situation too hard, or maybe I am too lazy"));
            int cnt = entities.GetLastMessageViewCountForContact(user.Id);
            Assert.IsNotNull(cnt);
        }

        [TestMethod]
        public void DeletedUser() {
            User user = entities.GetUserQ(enabled: false, transport: TransportKind.FLChat, hasOwner: true, ownerTransport: TransportKind.FLChat);
            Message msg = entities.SendMessage(user.OwnerUserId.Value, user.Id);

            LastMessageView[] msgs = entities.GetLastMessageViewForContact(user.OwnerUserId.Value);
            Assert.IsFalse(msgs.Where(m => m.UserOppId == user.Id).Any());
        }
    }
}
