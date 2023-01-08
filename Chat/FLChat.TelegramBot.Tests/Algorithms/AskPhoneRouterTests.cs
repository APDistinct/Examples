using System;
using System.Linq;
using FLChat.DAL;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.Core;

namespace FLChat.TelegramBot.Algorithms.Tests
{
    [TestClass]
    public class AskPhoneRouterTests
    {
        AskPhoneRouter router = new AskPhoneRouter();

        [TestMethod]
        public void AskPhoneRouter_NewUser() {
            using (ChatEntities entities = new ChatEntities()) {
                User from = entities.GetUser(
                    u => u.Enabled == true && u.IsTemporary == true && u.Phone == null,
                    u => {
                        u.IsTemporary = true;
                    },
                    DAL.TransportKind.Test);
                Message dbmsg = new Message() {
                    FromTransport = from.Transports.Get(DAL.TransportKind.Test)
                };

                FakeOuterMessage msg = new FakeOuterMessage() {
                    PhoneNumber = null
                };

                Assert.AreEqual(Global.SystemBotId, router.RouteMessage(entities, msg, dbmsg));
                var entity = entities.ChangeTracker.Entries<Message>().Single();

                Assert.AreEqual(System.Data.Entity.EntityState.Added, entity.State);
                Message newMsg = entity.Entity;
                Assert.AreEqual(MessageKind.Personal, newMsg.Kind);
                Assert.AreEqual(Global.SystemBotId, newMsg.FromUserId);
                Assert.AreEqual(TransportKind.FLChat, newMsg.FromTransportKind);
                Assert.IsFalse(newMsg.IsDeleted);
                //primary moment! has phone button
                Assert.IsTrue(newMsg.IsPhoneButton);

                MessageToUser to = newMsg.ToUsers.SingleOrDefault();
                Assert.IsNotNull(to);
                Assert.AreEqual(from.Id, to.ToUserId);
                Assert.AreEqual(from.Transports.Get(TransportKind.Test).TransportTypeId, to.ToTransportTypeId);
                Assert.IsFalse(to.IsSent);
                Assert.IsFalse(to.IsRead);
                Assert.IsFalse(to.IsDelivered);
                Assert.IsFalse(to.IsFailed);
            }
        }

        [TestMethod]
        public void AskPhoneRouter_OldUser() {
            using (ChatEntities entities = new ChatEntities()) {
                User from = entities.GetUser(
                    u => u.Enabled == true && u.IsTemporary == false && u.Phone == null,
                    u => {
                    },
                    DAL.TransportKind.Test);
                Message msg = new Message() {
                    FromTransport = from.Transports.Get(DAL.TransportKind.Test)
                };

                Assert.IsNull(router.RouteMessage(entities, null, msg));
                Assert.IsFalse(entities.ChangeTracker.HasChanges());
            }
        }

        [TestMethod]
        public void AskPhoneRouter_WithPhone() {
            using (ChatEntities entities = new ChatEntities()) {
                User from = entities.GetUser(
                    u => u.Enabled == true && u.IsTemporary == true && u.Phone != null,
                    u => {
                        u.IsTemporary = true;
                        u.Phone = new Random().Next().ToString();
                    },
                    DAL.TransportKind.Test);
                Message msg = new Message() {
                    FromTransport = from.Transports.Get(DAL.TransportKind.Test)
                };

                Assert.IsNull(router.RouteMessage(entities, null, msg));
                Assert.IsFalse(entities.ChangeTracker.HasChanges());
            }
        }

        [TestMethod]
        public void AskPhoneRouter_MsgWithKnownPhone() {
            using (ChatEntities entities = new ChatEntities()) {
                using (var trans = entities.Database.BeginTransaction()) {

                    User master = entities.GetUser(
                        u => u.Enabled == true && u.IsTemporary == false && u.Phone != null
                            && u.Transports.Where(t => t.TransportTypeId == (int)TransportKind.Test).Any() == false,
                        u => {
                            u.Phone = new Random().Next().ToString();
                        });
                    User from = entities.GetUser(
                        u => u.Enabled == true && u.IsTemporary == true && u.Phone == null,
                        u => {
                            u.IsTemporary = true;
                        },
                        DAL.TransportKind.Test);

                    Message dbmsg = new Message() {
                        FromTransport = from.Transports.Get(DAL.TransportKind.Test),
                        FromTransportKind = TransportKind.Test,
                        FromUserId = from.Id
                    };

                    FakeOuterMessage msg = new FakeOuterMessage() {
                        PhoneNumber = master.Phone
                    };

                    Guid? to = router.RouteMessage(entities, msg, dbmsg);
                    Assert.IsNull(to);
                    var msgentity = entities.ChangeTracker.Entries<Message>().SingleOrDefault();
                    Assert.IsNull(msgentity);

                    var dbtrans = entities.Transport.Where(t => t.UserId == master.Id && t.TransportTypeId == (int)TransportKind.Test).SingleOrDefault();
                    Assert.IsNotNull(dbtrans);

                    entities.Entry(from).Reload();
                    Assert.IsFalse(from.Enabled);
                    Assert.AreEqual(dbtrans, dbmsg.FromTransport);

                    trans.Rollback();
                }
            }
        }

        [TestMethod]
        public void AskPhoneRouter_MsgWithUnknownPhone() {
            using (ChatEntities entities = new ChatEntities()) {
                using (var trans = entities.Database.BeginTransaction()) {

                    User from = entities.GetUser(
                        u => u.Enabled == true && u.IsTemporary == true && u.Phone == null,
                        u => {
                            u.IsTemporary = true;
                        },
                        DAL.TransportKind.Test);

                    Random rnd = new Random();
                    string phone = rnd.Next().ToString();
                    while (entities.User.Where(u => u.Enabled && u.Phone == phone).SingleOrDefault() != null) {
                        phone = rnd.Next().ToString();
                    }

                    Message dbmsg = new Message() {
                        FromTransport = from.Transports.Get(DAL.TransportKind.Test),
                        FromTransportKind = TransportKind.Test,
                        FromUserId = from.Id
                    };

                    FakeOuterMessage msg = new FakeOuterMessage() {
                        PhoneNumber = phone
                    };

                    Guid? to = router.RouteMessage(entities, msg, dbmsg);
                    Assert.IsNull(to);

                    Assert.IsTrue(from.Enabled);
                    Assert.AreEqual(phone, from.Phone);

                    trans.Rollback();
                }
            }
        }
    }
}
