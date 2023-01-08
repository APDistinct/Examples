using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace FLChat.DAL.Model.Tests
{
    [TestClass]
    public class MessageTests
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
        public void Message_Create() {
            User user = entities.GetUser(
                u => u.Enabled && u.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any(),
                u => {
                    u.Enabled = true;
                    u.Transports.Add(new Transport() { Enabled = true, Kind = TransportKind.FLChat });
                });
            Message m = new Message() {
                FromUserId = user.Id,                
                FromTransportKind = TransportKind.FLChat
            };
            entities.Message.Add(m);
            entities.SaveChanges();

            Assert.AreNotEqual(Guid.Empty, m.Id);
            Assert.IsTrue(Math.Abs((m.PostTm - DateTime.UtcNow).TotalMinutes) < 1);

            entities.Entry(m).State = System.Data.Entity.EntityState.Deleted;
            entities.SaveChanges();
        }

        [TestMethod]
        public void MessageToUser_Create() {
            User user = entities.GetUserQ(transport: TransportKind.FLChat);
            User to = entities.GetUserQ(where: (w) => w.Where(u => u.Id != user.Id), transport: TransportKind.FLChat);
            Message m = new Message() {
                FromUserId = user.Id,
                FromTransportKind = TransportKind.FLChat
            };
            MessageToUser mtu = new MessageToUser() {
                ToUserId = to.Id,
                ToTransportKind = TransportKind.FLChat,
                IsSent = true
            };
            m.ToUsers.Add(mtu);
            entities.Message.Add(m);
            entities.SaveChanges();

            entities.Entry(mtu).Reload();

            Assert.IsFalse(mtu.IsWebChatGreeting);

            entities.Entry(mtu).State = System.Data.Entity.EntityState.Deleted;
            entities.Entry(m).State = System.Data.Entity.EntityState.Deleted;
            entities.SaveChanges();
        }

        [TestMethod]
        public void Message_ToUser() {
            Message msg = new Message();
            Assert.IsNull(msg.ToUser);

            MessageToUser mtu = new MessageToUser();
            msg.ToUser = mtu;

            Assert.AreSame(mtu, msg.ToUser);
            Assert.AreSame(mtu, msg.ToUsers.Single());
            Assert.AreEqual(1, msg.ToUsers.Count);

            Assert.ThrowsException<Exception>(() => msg.ToUser = mtu);
        }

        [TestMethod]
        public void Message_ToTransport() {
            Message msg = new Message();
            Assert.IsNull(msg.ToTransport);

            Transport t = new Transport() { Kind = TransportKind.FLChat };
            msg.ToTransport = t;

            Assert.AreSame(t, msg.ToTransport);
            Assert.AreSame(t, msg.ToUser.ToTransport);
            Assert.AreSame(t, msg.ToUsers.Single().ToTransport);
            Assert.AreEqual(1, msg.ToUsers.Count);
            Assert.IsTrue(msg.ToUser.IsSent);

            Assert.ThrowsException<Exception>(() => msg.ToTransport = t);

            msg = new Message();
            msg.ToTransport = new Transport() { Kind = TransportKind.Test };
            Assert.IsFalse(msg.ToUser.IsSent);
        }

        [TestMethod]
        public void Message_IsPhoneButton() {
            Assert.IsFalse(new Message().IsPhoneButton);
            Assert.IsTrue(new Message() { IsPhoneButton = true }.IsPhoneButton);
            Assert.IsFalse(new Message() { IsPhoneButton = false }.IsPhoneButton);
        }

        [TestMethod]
        public void Message_CountOfUnreadMessages() {
            User[] users = entities.GetUsers(3, u => u.Enabled, null, TransportKind.FLChat);
            Guid user = users[0].Id;
            Guid[] from = users.Skip(1).Select(u => u.Id).ToArray();

            //initial count
            Dictionary<Guid, int> initCount = entities.CountOfUnreadMessages(user, from);
            Assert.IsTrue(initCount.Count <= 2);

            //send first message
            Message msg1 = entities.SendMessage(from[0], user);
            Dictionary<Guid, int> count = entities.CountOfUnreadMessages(user, from); ;
            Assert.AreEqual(initCount.GetOrZero(from[0]) + 1, count.GetOrZero(from[0]));
            Assert.AreEqual(initCount.GetOrZero(from[1]), count.GetOrZero(from[1]));

            //send second message
            entities.SendMessage(from[0], user);
            count = entities.CountOfUnreadMessages(user, from); ;
            Assert.AreEqual(initCount.GetOrZero(from[0]) + 2, count.GetOrZero(from[0]));
            Assert.AreEqual(initCount.GetOrZero(from[1]), count.GetOrZero(from[1]));

            //set read
            msg1.ToUsers.Single().IsRead = true;
            entities.SaveChanges();

            count = entities.CountOfUnreadMessages(user, from);
            Assert.AreEqual(initCount.GetOrZero(from[0]) + 1, count.GetOrZero(from[0]));
            Assert.AreEqual(initCount.GetOrZero(from[1]), count.GetOrZero(from[1]));
        }

        [TestMethod]
        public void Message_CountOfUnreadMessages_OuterTransport() {
            Guid user = entities.GetUser(u => u.Enabled, null, TransportKind.Test).Id;
            Guid[] from = entities.GetUsers(1, u => u.Enabled, null, TransportKind.FLChat).Select(u => u.Id).ToArray();

            //initial count
            Dictionary<Guid, int> initCount = entities.CountOfUnreadMessages(user, from);

            //send first message
            Message msg1 = entities.SendMessage(from[0], user, tot: TransportKind.Test);
            Dictionary<Guid, int> count = entities.CountOfUnreadMessages(user, from); ;
            Assert.AreEqual(initCount.GetOrZero(from[0]), count.GetOrZero(from[0]));
        }
    }

    public static class DictionaryExtentions
    {
        public static int GetOrZero(this Dictionary<Guid, int> dict, Guid guid) {
            if (dict.TryGetValue(guid, out int value))
                return value;
            else
                return 0;
        }
    }
}
