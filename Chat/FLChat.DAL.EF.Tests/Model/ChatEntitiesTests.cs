using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.DAL.Model.Tests
{
    [TestClass]
    public class ChatEntitiesTests
    {
        ChatEntities entities;
        User from;
        User[] to;
        Guid active, passive;

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();

            from = entities.GetUser(
                u => u.Enabled && u.Transports.Where(t => t.TransportTypeId == 0 && t.Enabled == true).Any(),
                u => {
                    u.Enabled = true;
                    u.Transports.Add(new Transport() {
                        Enabled = true,
                        TransportTypeId = 0
                    });
                });
            to = entities.GetUsers(2,
                u => u.Enabled && u.Id != from.Id && u.Transports.Where(t => t.TransportTypeId == 0 && t.Enabled == true).Any(),
                u => {
                    u.Enabled = true;
                    u.Transports.Add(new Transport() {
                        Enabled = true,
                        TransportTypeId = 0
                    });
                });


            active = to[0].Id;
            passive = to[1].Id;
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        /// <summary>
        /// Create 3 messages to 2 users and test SetDelivered and SetRead procedures
        /// </summary>
        [TestMethod]
        public void ChatEntities_ExecuteMessageSetDeliveredAndRead() {
            List<Message> msgs = new List<Message>();
            for (int i = 0; i < 3; ++i)
                msgs.Add(entities.Message.Add(
                new Message() {
                    Kind = MessageKind.Personal,
                    Text = "Test message ExecuteMessageSetRead #" + i.ToString(),
                    FromUserId = from.Id,
                    FromTransportKind = TransportKind.FLChat,
                    ToUsers = to.Select(u => new MessageToUser() { ToUserId = u.Id, ToTransportTypeId = 0, IsSent = true }).ToArray()                    
                }));
            entities.SaveChanges();

            Reload(msgs);
            //not delivered and not read
            Check(msgs, false, false);

            //set 'delivered' flag for message to active user
            entities.ExecuteMessageSetDelivered(active, msgs.Select(m => m.Id));

            Reload(msgs);
            //delivered and not read
            Check(msgs, true, false);

            //set 'read' flag for message to active user
            entities.ExecuteMessageSetRead(active, msgs.Select(m => m.Id));

            Reload(msgs);
            //delivered and read
            Check(msgs, true, true);
        }

        [TestMethod]
        public void ChatEntities_SystemBot() {
            User bot = entities.SystemBot;
            Assert.IsNotNull(bot);
            Assert.AreEqual(Guid.Empty, bot.Id);
        }

        [TestMethod]
        public void ChatEntities_SystemBotTransport() {
            Transport t = entities.SystemBotTransport;
            Assert.IsNotNull(t);
            Assert.AreEqual(TransportKind.FLChat, t.Kind);
            Assert.AreEqual(Guid.Empty, t.UserId);
        }

        private void Reload(IEnumerable<Message> msgs) {
            //reload
            foreach (Message m in msgs) {
                entities.Entry(m).Reload();
                foreach (var mt in m.ToUsers)
                    entities.Entry(mt).Reload();
            }
        }

        private void Check(IEnumerable<Message> msgs, bool delivered, bool read) {
            foreach (Message m in msgs) {
                Assert.AreEqual(delivered, m.ToUsers.Where(t => t.ToUserId == active).Single().IsDelivered);
                Assert.AreEqual(read, m.ToUsers.Where(t => t.ToUserId == active).Single().IsRead);

                Assert.IsFalse(m.ToUsers.Where(t => t.ToUserId == passive).Single().IsRead);
                Assert.IsFalse(m.ToUsers.Where(t => t.ToUserId == passive).Single().IsDelivered);
            }
        }

        [TestMethod]
        public void ChatEntities_ExecuteSegmentUpdateMembers() {
            Random rnd = new Random();
            User[] users = entities.GetUsers(5, u => u.FLUserNumber != null, u => u.FLUserNumber = rnd.Next());
            Guid sid = entities.GetSegment(s => true, s => { }).Id;

            using (ChatEntities entities = new ChatEntities()) {
                entities.ExecuteSegmentUpdateMembers(sid, users.Take(3).Select(s => s.FLUserNumber.Value).ToArray());
                Segment segment = entities.Segment.Where(s => s.Id == sid).Single();
                CollectionAssert.AreEquivalent(
                    users.Take(3).Select(u => u.Id).ToArray(),
                    segment.Members.Select(u => u.Id).ToArray());
            }

            using (ChatEntities entities = new ChatEntities()) {
                entities.ExecuteSegmentUpdateMembers(sid, users.Skip(2).Take(3).Select(s => s.FLUserNumber.Value).ToArray());
                Segment segment = entities.Segment.Where(s => s.Id == sid).Single();
                CollectionAssert.AreEquivalent(
                    users.Skip(2).Take(3).Select(u => u.Id).ToArray(),
                    segment.Members.Select(u => u.Id).ToArray());
            }
        }

        [TestMethod]
        public void Update_OwnerUserId_By_ParentFLUserNumber_Test() {
            Random rnd = new Random();
            User user = entities.GetUserQ(
                where: w => w.Where(u => u.FLUserNumber != null && u.OwnerUserId != null
                    && u.OwnerUser.FLUserNumber != null && u.OwnerUser.OwnerUserId != null
                    && u.OwnerUser.OwnerUser.FLUserNumber != null),
                create: u => {
                    u.OwnerUser = new User() {
                        FLUserNumber = rnd.Next(),
                        OwnerUser = new User() {
                            FLUserNumber = rnd.Next()
                        }
                    };
                    u.FLUserNumber = rnd.Next();
                });
            user.ParentFLUserNumber = user.OwnerUser.FLUserNumber;
            user.OwnerUser.ParentFLUserNumber = user.OwnerUser.OwnerUser.FLUserNumber;
            entities.SaveChanges();

            User owner = user.OwnerUser.OwnerUser;
            User userMiddle = user.OwnerUser;
            int cnt = entities.Update_OwnerUserId_By_ParentFLUserNumber().FirstOrDefault().Value;
            entities.Entry(user).Reload();
            entities.Entry(userMiddle).Reload();
            entities.Entry(owner).Reload();

            user.ParentFLUserNumber = owner.FLUserNumber;
            entities.SaveChanges();
            cnt = entities.Update_OwnerUserId_By_ParentFLUserNumber().FirstOrDefault().Value;
            entities.Entry(user).Reload();
            Assert.AreEqual(owner.Id, user.OwnerUserId.Value);
            Assert.AreEqual(1, cnt);

            user.ParentFLUserNumber = null;
            entities.SaveChanges();
            cnt = entities.Update_OwnerUserId_By_ParentFLUserNumber().FirstOrDefault().Value;
            entities.Entry(user).Reload();
            Assert.IsNull(user.OwnerUserId);
            Assert.AreEqual(1, cnt);

            user.ParentFLUserNumber = userMiddle.FLUserNumber;
            entities.SaveChanges();
            cnt = entities.Update_OwnerUserId_By_ParentFLUserNumber().FirstOrDefault().Value;
            entities.Entry(user).Reload();
            Assert.AreEqual(userMiddle.Id, user.OwnerUserId.Value);
            Assert.AreEqual(1, cnt);
        }
    }
}
