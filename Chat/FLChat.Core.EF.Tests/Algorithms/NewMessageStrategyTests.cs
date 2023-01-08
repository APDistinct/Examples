using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;

using FLChat.DAL.Model;
using FLChat.DAL;
using FLChat.Core.Routers;
using FLChat.Core.Algorithms.WebChat;

namespace FLChat.Core.Algorithms.Tests
{
    [TestClass]
    public class NewMessageStrategyTests
    {    
        private ChatEntities entities;
        private Message lastMessage;
        private readonly WebChatCodeGenerator webChatGen = new WebChatCodeGenerator();

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Cleanup() {
            entities.Dispose();
        }

        public class ActionListener : NewMessageStrategy.IListener
        {
            private Action<ChatEntities, IOuterMessage, DeepLinkResult, Transport> _dla;
            private Action<ChatEntities, IOuterMessage, Transport> _nua;

            public ActionListener(
                Action<ChatEntities, IOuterMessage, DeepLinkResult, Transport> dla, 
                Action<ChatEntities, IOuterMessage, Transport> nua) {
                _dla = dla;
                _nua = nua;
            }

            public void BeforeAddTransport(ChatEntities entities, IDeepLinkData message, User user) {
                //throw new NotImplementedException();
            }

            public void DeepLinkAccepted(ChatEntities entities, IOuterMessage message, DeepLinkResult dlResult, Transport transport) {
                _dla(entities, message, dlResult, transport);
            }

            public void NewUserCreated(ChatEntities entities, IOuterMessage message, Transport transport) {
                _nua(entities, message, transport);
            }
        }

        [TestMethod]
        public void NewMessageStrategy_CheckRouterParams() {
            //get user
            User sender = entities.GetUser(
                u => u.Enabled,
                null,
                TransportKind.Test);

            //make message
            FakeOuterMessage msg = new FakeOuterMessage() {
                FromId = sender.Transports.Get(TransportKind.Test).TransportOuterId,
                FromName = sender.FullName,
                Text = "NewMessageStrategy_RouteFailed message"
            };

            bool called = false;
            ActionRouter router = new ActionRouter((ce, fm, dbmsg) => {
                called = true;
                Assert.IsNotNull(dbmsg);
                Assert.IsNotNull(dbmsg.FromTransport);
                Assert.IsNotNull(dbmsg.FromTransport.User);
                Assert.AreEqual(sender.Transports.Get(TransportKind.Test).Kind, dbmsg.FromTransport.Kind);
                Assert.AreEqual(sender.Id, dbmsg.FromTransport.User.Id);

                Assert.AreSame(entities, ce);
                Assert.AreSame(fm, msg);
                return null;
            });

            NewMessageStrategy strategy = new NewMessageStrategy(router, new ActionTransportIdSaver(), new WebChatDeepLinkStrategy());
            strategy.Process(entities, msg);

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void NewMessageStrategy_CheckSaverParams() {
            //get user
            User sender = entities.GetUser(
                u => u.Enabled,
                null,
                TransportKind.Test);

            //make message
            FakeOuterMessage msg = new FakeOuterMessage() {
                FromId = sender.Transports.Get(TransportKind.Test).TransportOuterId,
                MessageId = Guid.NewGuid().ToString(),
            };

            bool called = false;
            ActionTransportIdSaver saver = new ActionTransportIdSaver((ce, id, dbmsg) => {
                called = true;
                Assert.IsNotNull(dbmsg);

                Assert.AreSame(entities, ce);
                Assert.AreEqual(msg.MessageId, id);
            });

            NewMessageStrategy strategy = new NewMessageStrategy(new ActionRouter((Guid?)null), saver, new WebChatDeepLinkStrategy());
            strategy.Process(entities, msg);

            Assert.IsTrue(called);
        }

        private NewMessageStrategy CreateStrategy(Guid? addressee, NewMessageStrategy.IListener listener = null) {
            ActionTransportIdSaver saver = new ActionTransportIdSaver((ce, id, dbm) => lastMessage = dbm);
            ActionRouter router = new ActionRouter(addressee);
            return new NewMessageStrategy(router, saver, deepLink: new WebChatDeepLinkStrategy(), listener: listener);
        }

        [TestMethod]
        public void NewMessageStrategy_RouteSuccess() {
            User sender = entities.GetUser(u => true, null, TransportKind.Test);
            User addressee = entities.GetUser(u => u.Id != sender.Id, null, TransportKind.FLChat);

            ActionListener listener = new ActionListener(
                (ce, m, dlr, t) => Assert.Fail("Deep link listener will not call"),
                (ce, m, t) => Assert.Fail("New user listener will not call"));

            NewMessageStrategy strategy = CreateStrategy(addressee.Id, listener);

            //make message
            FakeOuterMessage msg = new FakeOuterMessage() {
                FromId = sender.Transports.Get(TransportKind.Test).TransportOuterId,
                FromName = sender.FullName,
                Text = "NewMessageStrategy_KnownUser_WithOwnerId message"
            };

            //handle new incoming message
            lastMessage = null;
            strategy.Process(entities, msg);

            //checks
            Assert.IsNotNull(lastMessage);
            Assert.AreNotEqual(Guid.Empty, lastMessage.Id);

            //check message main part
            Message dbmsg = entities.Message.Where(m => m.Id == lastMessage.Id).Single();
            Assert.AreEqual(sender.Id, dbmsg.FromUserId);
            Assert.AreEqual(TransportKind.Test, dbmsg.FromTransportKind);
            Assert.AreEqual(msg.Text, dbmsg.Text);
            Assert.AreEqual(MessageKind.Personal, dbmsg.Kind);
            Assert.IsNull(dbmsg.AnswerToId);
            Assert.IsFalse(dbmsg.IsDeleted);

            //check message ToUser part
            MessageToUser dbmsgto = dbmsg.ToUsers.SingleOrDefault();
            Assert.IsNotNull(dbmsgto);
            Assert.AreEqual(addressee.Id, dbmsgto.ToUserId);
            Assert.AreEqual(TransportKind.FLChat, dbmsgto.ToTransportKind);
            Assert.IsFalse(dbmsgto.IsFailed);
            Assert.IsTrue(dbmsgto.IsSent);
            Assert.IsFalse(dbmsgto.IsDelivered);
            Assert.IsFalse(dbmsgto.IsRead);
        }

        [TestMethod]
        public void NewMessageStrategy_RouteFailed() {
            //search user with test transport
            User sender = entities.GetUser(
                u => u.Enabled,
                null,
                TransportKind.Test);
            NewMessageStrategy strategy = CreateStrategy(null);

            //make message
            FakeOuterMessage msg = new FakeOuterMessage() {
                FromId = sender.Transports.Get(TransportKind.Test).TransportOuterId,
                FromName = sender.FullName,
                Text = "NewMessageStrategy_RouteFailed message"
            };

            //handle new incoming message
            lastMessage = null;
            strategy.Process(entities, msg);

            //check message main part
            Message dbmsg = entities.Message.Where(m => m.Id == lastMessage.Id).Single();
            Assert.AreEqual(sender.Id, dbmsg.FromUserId);
            Assert.AreEqual(TransportKind.Test, dbmsg.FromTransportKind);
            Assert.AreEqual(msg.Text, dbmsg.Text);
            Assert.AreEqual(MessageKind.Personal, dbmsg.Kind);
            Assert.IsNull(dbmsg.AnswerToId);
            Assert.IsFalse(dbmsg.IsDeleted);

            //check message ToUser part
            MessageToUser dbmsgto = dbmsg.ToUsers.SingleOrDefault();
            Assert.IsNull(dbmsgto);
        }

        private void CheckIsNewUser(ChatEntities ce, IOuterMessage om, Message dbm, string transOuterId) {
            Assert.IsNotNull(om);
            Assert.IsNotNull(dbm);
            Assert.IsNotNull(dbm.FromTransport);
            Assert.AreEqual(TransportKind.Test, dbm.FromTransportKind);
            Assert.AreEqual(transOuterId, dbm.FromTransport.TransportOuterId);
            Assert.IsNotNull(dbm.FromTransport.User);
            Assert.IsTrue(dbm.FromTransport.User.IsTemporary);

            Assert.AreEqual(EntityState.Added, ce.Entry(dbm.FromTransport.User).State);
        }

        [TestMethod]
        public void NewMessageStrategy_FromNewUser() {
            bool performed = false;
            string transOuterId = Guid.NewGuid().ToString();

            bool listenerCalled = false;
            ActionListener listener = new ActionListener(
                (ce, m, dlr, t) => Assert.Fail("Deep link listener will not call"),
                (ce, m, t) => {
                    listenerCalled = true;
                    Assert.AreEqual(transOuterId, t.TransportOuterId);
                    Assert.AreEqual(TransportKind.Test, t.Kind);
                    Assert.IsNotNull(ce);
                    Assert.IsNotNull(m);
                    Assert.IsNotNull(t);
                });

            //search user with test transport
            NewMessageStrategy strategy = new NewMessageStrategy(new ActionRouter(
                (ce, om, dbm) => {
                    CheckIsNewUser(ce, om, dbm, transOuterId);
                    performed = true;
                    return null;
                }),
                new ActionTransportIdSaver(), new WebChatDeepLinkStrategy(), listener: listener);

            //make message
            string name = "Test";
            FakeOuterMessage msg = new FakeOuterMessage() {
                FromId = transOuterId,
                TransportKind = TransportKind.Test,
                FromName = name,
                Text = "NewMessageStrategy_FromNewUser message"
            };

            //handle new incoming message
            using (ChatEntities ent = new ChatEntities()) {
                strategy.Process(ent, msg);
            }

            //check message main part
            Assert.IsTrue(performed);
            Assert.IsTrue(listenerCalled);

            Transport transport = entities
                .Transport
                .Where(t => t.TransportTypeId == (int)TransportKind.Test && t.TransportOuterId == transOuterId)
                .SingleOrDefault();
            Assert.IsNotNull(transport);
            User user = transport.User;
            entities.Entry(transport).Reload();            

            Message dbmsg = entities.Message.Where(m => m.FromUserId == transport.UserId).SingleOrDefault();
            Assert.IsNotNull(dbmsg);
            Assert.AreEqual(user.FullName, name);

            entities.Entry(dbmsg).State = System.Data.Entity.EntityState.Deleted;
            entities.Entry(transport).State = System.Data.Entity.EntityState.Deleted;
            entities.Entry(user).State = System.Data.Entity.EntityState.Deleted;
        }

        /// <summary>
        /// Message from new user, but his transport id already exists in database in deleted state
        /// </summary>
        [TestMethod]
        public void NewMessageStrategy_FromNewUser_ButExistDisabledTransport() {
            bool performed = false;
            User from = entities.GetUser(
                u => u.Enabled && u.Transports.Where(t => t.Enabled == false && t.TransportTypeId == (int)TransportKind.Test).Any(),
                u => {
                    u.Transports.Add(new Transport() {
                        Enabled = false,
                        Kind = TransportKind.Test,
                        TransportOuterId = Guid.NewGuid().ToString()
                    });
                });
            string transOuterId = from.Transports.Get(TransportKind.Test).TransportOuterId;

            //search user with test transport
            NewMessageStrategy strategy = new NewMessageStrategy(new ActionRouter(
                (ce, om, dbm) => {
                    CheckIsNewUser(ce, om, dbm, transOuterId);
                    performed = true;
                    return null;
                }),
                new ActionTransportIdSaver(),
                new WebChatDeepLinkStrategy());

            //make message
            FakeOuterMessage msg = new FakeOuterMessage() {
                FromId = transOuterId,
                TransportKind = TransportKind.Test,
                FromName = "Test",
                Text = "NewMessageStrategy_FromNewUser message"
            };

            //handle new incoming message
            using (ChatEntities ent = new ChatEntities()) {
                var trans = ent.Database.BeginTransaction();
                strategy.Process(ent, msg);
                trans.Rollback();
            }

            //check message main part
            Assert.IsTrue(performed);
        }

        /// <summary>
        /// Accept message with deep-link and add to user new transport
        /// </summary>
        [TestMethod]
        public void NewMessageStrategy_DeepLink_NewTransport() {
            //user who will send message from outer transport
            User from = entities.GetUser(
                u => u.Enabled && u.Transports.Where(t => t.TransportTypeId == (int)TransportKind.Test).Any() == false,
                null);
            //check user from has't transport of that type
            Assert.IsNull(entities.Transport.Where(t => t.UserId == from.Id && t.TransportTypeId == (int)TransportKind.Test).FirstOrDefault());

            //user who make deep-link invite
            User inv = entities.GetUser(u => u.Enabled, null, TransportKind.FLChat);
            //invited message
            Message wcmsg = new Message() {
                Kind = MessageKind.Personal,
                FromTransport = inv.Transports.Get(TransportKind.FLChat),
                Text = "invite",
                ToUsers = new MessageToUser[] {
                    new MessageToUser() {
                        ToTransport = from.Transports.Get(TransportKind.WebChat)
                    }
                }
            };
            entities.Message.Add(wcmsg);
            entities.SaveChanges();
            webChatGen.Gen(wcmsg.ToUser);

            //seek deep-link
            WebChatDeepLink wcdl = entities.WebChatDeepLink.Where(wc => wc.MsgId == wcmsg.Id).Single();

            //make message
            FakeOuterMessage msg = new FakeOuterMessage() {
                FromId = from.Id.ToString(),
                FromName = from.FullName,
                Text = "Message with deep link",
                DeepLink = wcdl.Link
            };

            bool called = false;
            ActionRouter router = new ActionRouter((ce, fm, dbmsg) => {
                called = true;
                Assert.AreEqual(msg.FromId, dbmsg.FromTransport.TransportOuterId);
                Assert.AreEqual(TransportKind.Test, dbmsg.FromTransport.Kind);
                Assert.AreEqual(from.Id, dbmsg.FromTransport.User.Id);
                Assert.IsTrue(dbmsg.FromTransport.Enabled);
                Assert.AreEqual(wcmsg.Id, dbmsg.AnswerToId);
                return null;
            });

            bool listenerCalled = false;
            ActionListener listener = new ActionListener(
                (ce, m, dlr, t) => {
                    listenerCalled = true;
                    Assert.AreEqual(DeepLinkResultStatus.Accepted, dlr.Status);
                    Assert.AreEqual(msg.FromId, t.TransportOuterId);
                    Assert.IsNotNull(ce);
                    Assert.IsNotNull(m);
                    Assert.IsNotNull(dlr);
                    Assert.IsNotNull(t);
                },
                (ce, m, t) => Assert.Fail("New user listener will not call"));

            NewMessageStrategy strategy = new NewMessageStrategy(router, new ActionTransportIdSaver(), 
                new WebChatDeepLinkStrategy(), listener: listener);
            strategy.Process(entities, msg);

            Assert.IsTrue(called);
            Assert.IsTrue(listenerCalled);

            entities.Entry(wcdl).Collection(w => w.AcceptedTransportType).Load();
            Assert.IsTrue(wcdl.AcceptedTransportKind.Contains(TransportKind.Test));

            //seek for event
            Event dlEvent = entities.Event
                .Where(e =>
                    e.EventTypeId == (int)EventKind.DeepLinkAccepted
                    && e.CausedByUserId == from.Id
                    && e.CausedByUserTransportTypeId == (int)TransportKind.Test)
                .OrderByDescending(e => e.Id)
                .FirstOrDefault();
            Assert.IsNotNull(dlEvent);
            Assert.AreEqual(inv.Id, dlEvent.ToUsers.First().Id);
        }

        /// <summary>
        /// Accept message with deep-link and update user's transport
        /// </summary>
        [TestMethod]
        public void NewMessageStrategy_DeepLink_UpdateTransport() {
            //user who will send message from outer transport
            User from = entities.GetUser(
                u => u.Enabled && u.Transports.Where(t => t.Enabled == false && t.TransportTypeId == (int)TransportKind.Test).Any(),
                u => {
                    u.Transports.Add(new Transport() {
                        Enabled = false,
                        Kind = TransportKind.Test,
                        TransportOuterId = Guid.NewGuid().ToString()
                    });
                });
            Transport tfrom = from.Transports.Get(TransportKind.Test);
            tfrom.TransportOuterId = Guid.NewGuid().ToString();
            entities.SaveChanges();

            //check user from has transport of that type
            Assert.IsNotNull(entities.Transport
                .Where(t => t.Enabled == false && t.UserId == from.Id && t.TransportTypeId == (int)TransportKind.Test)
                .FirstOrDefault());

            //user who make deep-link invite
            User inv = entities.GetUser(u => u.Enabled, null, TransportKind.FLChat);
            //invited message
            Message wcmsg = new Message() {
                Kind = MessageKind.Personal,
                FromTransport = inv.Transports.Get(TransportKind.FLChat),
                Text = "NewMessageStrategy_DeepLink_UpdateTransport",
                ToUsers = new MessageToUser[] {
                    new MessageToUser() {
                        ToTransportKind = TransportKind.WebChat,
                        ToUserId = from.Id
                        //ToTransport = from.Transports.Get(TransportKind.WebChat)
                    }
                }
            };
            entities.Message.Add(wcmsg);
            entities.SaveChanges();

            webChatGen.Gen(wcmsg.ToUser);

            //seek deep-link
            WebChatDeepLink wcdl = entities.WebChatDeepLink.Where(wc => wc.MsgId == wcmsg.Id).Single();

            //make message
            FakeOuterMessage msg = new FakeOuterMessage() {
                FromId = tfrom.TransportOuterId,//.Id.ToString(),
                FromName = from.FullName,
                Text = "Message with deep link",
                DeepLink = wcdl.Link
            };

            bool called = false;
            ActionRouter router = new ActionRouter((ce, fm, dbmsg) => {
                called = true;
                Assert.AreEqual(msg.FromId, dbmsg.FromTransport.TransportOuterId);
                Assert.AreEqual(TransportKind.Test, dbmsg.FromTransport.Kind);
                Assert.AreEqual(from.Id, dbmsg.FromTransport.User.Id);
                Assert.IsTrue(dbmsg.FromTransport.Enabled);
                Assert.AreEqual(wcmsg.Id, dbmsg.AnswerToId);
                return null;
            });

            NewMessageStrategy strategy = new NewMessageStrategy(router, new ActionTransportIdSaver(),
                new WebChatDeepLinkStrategy());
            strategy.Process(entities, msg);

            Assert.IsTrue(called);

            entities.Entry(wcdl).Collection(w => w.AcceptedTransportType).Load();
            Assert.IsTrue(wcdl.AcceptedTransportKind.Contains(TransportKind.Test));

            //seek for event
            Event dlEvent = entities.Event
                .Where(e =>
                    e.EventTypeId == (int)EventKind.DeepLinkAccepted
                    && e.CausedByUserId == from.Id
                    && e.CausedByUserTransportTypeId == (int)TransportKind.Test)
                .OrderByDescending(e => e.Id)
                .FirstOrDefault();
            Assert.IsNotNull(dlEvent);
            Assert.AreEqual(inv.Id, dlEvent.ToUsers.First().Id);
        }

        /// <summary>
        /// Accept message with deep-link and add to user new transport
        /// </summary>
        [TestMethod]
        public void NewMessageStrategy_DeepLink_SetAddressee() {
            //user who will send message from outer transport
            User from = entities.GetUser(
                u => u.Enabled && u.Transports.Where(t => t.TransportTypeId == (int)TransportKind.Test).Any() == false,
                null);
            //check user from has't transport of that type
            Assert.IsNull(entities.Transport.Where(t => t.UserId == from.Id && t.TransportTypeId == (int)TransportKind.Test).FirstOrDefault());

            //user who make deep-link invite
            User inv = entities.GetUser(null, null, TransportKind.FLChat);
            //invited message
            Message wcmsg = new Message() {
                Kind = MessageKind.Personal,
                FromTransport = inv.Transports.Get(TransportKind.FLChat),
                Text = "invite",
                ToUsers = new MessageToUser[] {
                    new MessageToUser() {
                        ToTransport = from.Transports.Get(TransportKind.WebChat)
                    }
                }
            };
            entities.Message.Add(wcmsg);
            entities.SaveChanges();
            webChatGen.Gen(wcmsg.ToUser);

            //seek deep-link
            WebChatDeepLink wcdl = entities.WebChatDeepLink.Where(wc => wc.MsgId == wcmsg.Id).Single();

            //make message
            FakeOuterMessage msg = new FakeOuterMessage() {
                FromId = from.Id.ToString(),
                FromName = from.FullName,
                Text = "Message with deep link",
                DeepLink = wcdl.Link
            };

            NewMessageStrategy strategy = new NewMessageStrategy(new ActionRouter(Guid.Empty), 
                new ActionTransportIdSaver(), new WebChatDeepLinkStrategy());
            strategy.Process(entities, msg);

            //check new user's messages addresse
            Assert.IsTrue(from.Transports.Get(TransportKind.Test).IsAddressee(inv));
        }

        /// <summary>
        /// Accept message with deep-link and add to user new transport
        /// </summary>
        [TestMethod]
        public void NewMessageStrategy_DeepLink_SetAddresseeOwner() {
            //user who will send message from outer transport
            User from = entities.GetUser(
                u => u.Transports.Where(t => t.TransportTypeId == (int)TransportKind.Test).Any() == false
                    && u.OwnerUserId != null 
                    && u.OwnerUser.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any(),
                u => u.OwnerUser = entities.GetUser(null, null, TransportKind.FLChat));
            //check user from has't transport of that type
            Assert.IsNull(entities.Transport.Where(t => t.UserId == from.Id && t.TransportTypeId == (int)TransportKind.Test).FirstOrDefault());

            //user who make deep-link invite
            User inv = from.OwnerUser;
            //invited message
            Message wcmsg = new Message() {
                Kind = MessageKind.Personal,
                FromTransport = inv.Transports.Get(TransportKind.FLChat),
                Text = "invite",
                ToUsers = new MessageToUser[] {
                    new MessageToUser() {
                        ToTransport = from.Transports.Get(TransportKind.WebChat)
                    }
                }
            };
            entities.Message.Add(wcmsg);
            entities.SaveChanges();
            webChatGen.Gen(wcmsg.ToUser);

            //seek deep-link
            WebChatDeepLink wcdl = entities.WebChatDeepLink.Where(wc => wc.MsgId == wcmsg.Id).Single();

            //make message
            FakeOuterMessage msg = new FakeOuterMessage() {
                FromId = from.Id.ToString(),
                FromName = from.FullName,
                Text = "Message with deep link",
                DeepLink = wcdl.Link
            };

            NewMessageStrategy strategy = new NewMessageStrategy(new ActionRouter(Guid.Empty), 
                new ActionTransportIdSaver(), new WebChatDeepLinkStrategy());
            strategy.Process(entities, msg);

            //check new user's messages addresse
            Assert.IsNull(from.Transports.Get(TransportKind.Test).MsgAddressee);
        }

        /// <summary>
        /// Test deep link strategy
        /// </summary>
        [TestMethod]
        public void NewMessageStrategy_DeepLinkStrategy() {
            //user who will send message from outer transport
            User from = entities.GetUser(
                u => u.Enabled && u.Transports.Where(t => t.TransportTypeId == (int)TransportKind.Test).Any() == false,
                null);
            //check user from has't transport of that type
            Assert.IsNull(entities.Transport.Where(t => t.UserId == from.Id && t.TransportTypeId == (int)TransportKind.Test).FirstOrDefault());

            //user who make deep-link invite
            User inv = entities.GetUser(u => u.Enabled, null, TransportKind.FLChat);
            //invited message
            Message wcmsg = new Message() {
                Kind = MessageKind.Personal,
                FromTransport = inv.Transports.Get(TransportKind.FLChat),
                Text = "invite",
                ToUsers = new MessageToUser[] {
                    new MessageToUser() {
                        ToTransport = from.Transports.Get(TransportKind.WebChat)
                    }
                }
            };
            entities.Message.Add(wcmsg);
            entities.SaveChanges();
            webChatGen.Gen(wcmsg.ToUser);

            //seek deep-link
            WebChatDeepLink wcdl = entities.WebChatDeepLink.Where(wc => wc.MsgId == wcmsg.Id).Single();

            //make message
            FakeOuterMessage msg = new FakeOuterMessage() {
                FromId = from.Id.ToString(),
                FromName = from.FullName,
                Text = "Message with deep link",
                DeepLink = wcdl.Link,
                IsTransportEnabled = false
            };

            NewMessageStrategy strategy = new NewMessageStrategy(
                new ActionRouter(Guid.Empty), 
                new ActionTransportIdSaver(),
                new WebChatDeepLinkStrategy());
            strategy.Process(entities, msg);

            //check user transport created in disabled state
            entities.Entry(from).Collection(u => u.Transports);
            Assert.IsFalse(from.Transports.Where(t => t.TransportTypeId == (int)TransportKind.Test).Single().Enabled);
        }

        [TestMethod]
        public void NewMessageStrategy_SaveMessageId() {
            User sender = entities.GetUser(null, null, TransportKind.Test);

            FakeOuterMessage msg = new FakeOuterMessage() {
                FromId = sender.Transports.Get(TransportKind.Test).TransportOuterId,
                MessageId = Guid.NewGuid().ToString(),
                Text = "test " + DateTime.Now.ToString()
            };
            NewMessageStrategy strategy = new NewMessageStrategy(new ActionRouter(Guid.Empty), new WebChatDeepLinkStrategy());
            strategy.Process(entities, msg);
            Message dbmsg = entities.Message.Where(m => m.FromUserId == sender.Id).OrderByDescending(m => m.Idx).FirstOrDefault();
            MessageTransportId mid = entities.MessageTransportId.Where(m => m.MsgId == dbmsg.Id).FirstOrDefault();

            Assert.IsNotNull(mid);
            Assert.AreEqual(msg.MessageId, mid.TransportId);
        }

        //[TestMethod]
        //public void NewMessageStrategy_AsnwerTo() {
        //    User user = entities.GetUser(null, null, TransportKind.Test);  //outer user, who replies on message
        //    User inner = entities.GetUser(u => u.Id != user.Id, null, TransportKind.FLChat); //user, who sends initial message

        //    Message initialMsg = entities.SendMessage(inner.Id, user.Id, tot: TransportKind.Test);
        //    MessageTransportId initialId = new MessageTransportId() {
        //        MsgId = initialMsg.Id,
        //        ToUserId = user.Id,
        //        TransportTypeId = (int)TransportKind.Test,
        //        TransportId = Guid.NewGuid().ToString()
        //    };
        //    entities.MessageTransportId.Add(initialId);
        //    entities.SaveChanges();

        //    NewMessageStrategy<FakeOuterMessage> strategy = new NewMessageStrategy<FakeOuterMessage>(new AnswerRouter());

        //    //make message
        //    FakeOuterMessage msg = new FakeOuterMessage() {
        //        FromId = user.Transports.Get(TransportKind.Test).TransportOuterId,
        //        FromName = user.FullName,
        //        Text = "NewMessageStrategy_KnownUser_WithOwnerId message",
        //        ReplyToMessageId = initialId.TransportId
        //    };

        //    //handle new incoming message
        //    strategy.Process(entities, msg);

        //    //get last inserted message
        //    Message dbmsg = entities.Message.OrderByDescending(m => m.Idx).First();
        //    Assert.IsNotNull(dbmsg.AnswerToId);
        //    Assert.AreEqual(initialMsg.Id, dbmsg.AnswerToId);            

        //    //check message ToUser part
        //    MessageToUser dbmsgto = dbmsg.ToUsers.SingleOrDefault();
        //    Assert.IsNotNull(dbmsgto);
        //    Assert.AreEqual(inner.Id, dbmsgto.ToUserId);
        //    Assert.AreEqual(TransportKind.FLChat, dbmsgto.ToTransportKind);
        //}

        [TestMethod]
        public void NewMessageStrategy_EnableTransportOnIncommingMessage() {
            User user = entities.GetUser(
                u => u.Transports.Where(t => t.Enabled == false && t.TransportTypeId == (int)TransportKind.Test).Any(),
                u => u.Transports.Add(new Transport() { Enabled = false, Kind = TransportKind.Test, TransportOuterId = Guid.NewGuid().ToString() }));
            Transport transport = user.Transports.Get(TransportKind.Test);
            transport.TransportOuterId = Guid.NewGuid().ToString();
            entities.SaveChanges();
            Assert.IsFalse(transport.Enabled);

            FakeOuterMessage msg = new FakeOuterMessage() {
                FromId = transport.TransportOuterId,
                Text = "123"
            };

            bool called = false;
            ActionRouter router = new ActionRouter((e, m, dm) => {
                called = true;
                Assert.IsTrue(dm.FromTransport.Enabled);
                Assert.AreEqual(user.Id, dm.FromUserId);
                Assert.AreEqual(user.Id, dm.FromTransport.UserId);
                Assert.AreEqual(TransportKind.Test, dm.FromTransport.Kind);
                Assert.AreEqual(transport.TransportOuterId, dm.FromTransport.TransportOuterId);
                //entities.Entry(transport).Reload();
                return Guid.Empty;
            });

            NewMessageStrategy strategy = new NewMessageStrategy(router, new WebChatDeepLinkStrategy()) { EnableTransportOnIncommingMessage = true };
            strategy.Process(entities, msg);

            Assert.IsTrue(called);

            entities.Entry(transport).Reload();
            Assert.IsTrue(transport.Enabled);

        }

        [TestMethod]
        public void NewMessageStrategy_EnableTransportOnIncommingMessage_DeepLink() {
            User user = entities.GetUser(
                u => u.Transports.Where(t => t.Enabled == false
                    && t.TransportOuterId != ""
                    && t.TransportTypeId == (int)TransportKind.Test).Any(),
                u => u.Transports.Add(new Transport() { Enabled = false, Kind = TransportKind.Test, TransportOuterId = Guid.NewGuid().ToString() }));
            Transport transport = user.Transports.Get(TransportKind.Test);
            FakeOuterMessage msg = new FakeOuterMessage() {
                FromId = transport.TransportOuterId,
                Text = "123",
                DeepLink = "2222"
            };

            bool called = false;
            ActionRouter router = new ActionRouter((e, m, dm) => {
                called = true;
                Assert.IsFalse(dm.FromTransport.Enabled);
                Assert.AreEqual(user.Id, dm.FromUserId);
                Assert.AreEqual(user.Id, dm.FromTransport.UserId);
                Assert.AreEqual(TransportKind.Test, dm.FromTransport.Kind);
                Assert.AreEqual(transport.TransportOuterId, dm.FromTransport.TransportOuterId);
                entities.Entry(transport).Reload();
                return Guid.Empty;
            });

            NewMessageStrategy strategy = new NewMessageStrategy(router, new WebChatDeepLinkStrategy()) { EnableTransportOnIncommingMessage = true };
            strategy.Process(entities, msg);

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void NewMessageStrategy_DeepLink_Temporary() {
            //temporary user
            User tmp = entities.GetUser(
                u => u.Enabled && u.IsTemporary,
                u => { u.IsTemporary = true; },
                TransportKind.Test );

            //user who will accept deep link
            User from = entities.GetUser(
                u => u.Enabled && u.Transports.Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.Test).Any() == false,
                null);

            //Transport tfrom = from.Transports.Get(TransportKind.Test);
            //tfrom.TransportOuterId = Guid.NewGuid().ToString();
            //entities.SaveChanges();

            //check tmp user has transport of that type
            Assert.IsNotNull(entities.Transport
                .Where(t => t.Enabled == true && t.UserId == tmp.Id && t.TransportTypeId == (int)TransportKind.Test)
                .SingleOrDefault());
            string transportOuterId = tmp.Transports.Get(TransportKind.Test).TransportOuterId;

            //user who make deep-link invite
            User inv = entities.GetUser(u => u.Enabled, null, TransportKind.FLChat);
            //invited message
            Message wcmsg = new Message() {
                Kind = MessageKind.Personal,
                FromTransport = inv.Transports.Get(TransportKind.FLChat),
                Text = "NewMessageStrategy_DeepLink_Temporary",
                ToUsers = new MessageToUser[] {
                    new MessageToUser() {
                        ToTransportKind = TransportKind.WebChat,
                        ToUserId = from.Id
                    }
                }
            };
            entities.Message.Add(wcmsg);
            entities.SaveChanges();

            webChatGen.Gen(wcmsg.ToUser);

            //seek deep-link
            WebChatDeepLink wcdl = entities.WebChatDeepLink.Where(wc => wc.MsgId == wcmsg.Id).Single();

            //make message
            FakeOuterMessage msg = new FakeOuterMessage() {
                FromId = transportOuterId,
                FromName = from.FullName,
                Text = "Message with deep link",
                DeepLink = wcdl.Link
            };

            NewMessageStrategy strategy = new NewMessageStrategy(new ActionRouter(DAL.Global.SystemBotId), 
                new ActionTransportIdSaver(),
                new WebChatDeepLinkStrategy());
            strategy.Process(entities, msg);

            //check tmp user has't transport anymore
            Assert.IsNull(entities.Transport
                .Where(t => t.Enabled == true && t.UserId == tmp.Id && t.TransportTypeId == (int)TransportKind.Test)
                .SingleOrDefault());
            //check from user already has transport anymore
            Transport transport = entities.Transport
                .Where(t => t.Enabled == true && t.UserId == from.Id && t.TransportTypeId == (int)TransportKind.Test)
                .SingleOrDefault();
            Assert.IsNotNull(transport);
            Assert.AreEqual(transportOuterId, transport.TransportOuterId);
        }

        /// <summary>
        /// Test outer parameter DeepLinkResult
        /// </summary>
        [TestMethod]
        public void NewMessageStrategy_DeepLink_DeepLinkResult() {
            //user who will send message from outer transport
            User from = entities.GetUser(
                u => u.Enabled && u.Transports.Where(t => t.TransportTypeId == (int)TransportKind.Test).Any() == false,
                null);
            //check user from has't transport of that type
            Assert.IsNull(entities.Transport.Where(t => t.UserId == from.Id && t.TransportTypeId == (int)TransportKind.Test).FirstOrDefault());

            //user who make deep-link invite
            User inv = entities.GetUser(u => u.Id != from.Id && u.Enabled, null, TransportKind.FLChat);
            //invited message
            Message wcmsg = entities.SendMessage(inv.Id, from.Id, TransportKind.FLChat, TransportKind.WebChat);

            //seek deep-link
            WebChatDeepLink wcdl = entities.WebChatDeepLink.Where(wc => wc.MsgId == wcmsg.Id).Single();

            NewMessageStrategy strategy = new NewMessageStrategy(new ActionRouter(Global.SystemBotId),                 
                new ActionTransportIdSaver(),
                new WebChatDeepLinkStrategy());

            DeepLinkResult dlResult;

            //first call
            strategy.Process(entities, new FakeOuterMessage() {
                FromId = from.Id.ToString(),
                FromName = from.FullName,
                Text = "Message with deep link",
                DeepLink = wcdl.Link
            }, out dlResult);

            Assert.AreEqual(DeepLinkResultStatus.Accepted, dlResult.Status);
            Assert.AreEqual(wcdl.Id, (dlResult.Context as WebChatDeepLinkStrategy.Context).WebChatId);
            Assert.AreEqual(wcdl.MsgId, (dlResult.Context as WebChatDeepLinkStrategy.Context).MessageId);

            //second call from same user
            strategy.Process(entities, new FakeOuterMessage() {
                FromId = from.Id.ToString(),
                FromName = from.FullName,
                Text = "Message with deep link #2",
                DeepLink = wcdl.Link
            }, out dlResult);

            //Assert.AreEqual(DeepLinkResultStatus.AcceptedEarly, dlResult.Status);
            //Assert.AreEqual(wcdl.Id, dlResult.WebChatId.Value);
            //Assert.AreEqual(wcdl.MsgId, dlResult.MessageId.Value);
            Assert.IsNull(dlResult);

            //call from another user
            strategy.Process(entities, new FakeOuterMessage() {
                FromId = Guid.NewGuid().ToString(),
                FromName = "fake user",
                Text = "Message with deep link #3",
                DeepLink = wcdl.Link
            }, out dlResult);

            Assert.AreEqual(DeepLinkResultStatus.Rejected, dlResult.Status);
            Assert.AreEqual(wcdl.Id, (dlResult.Context as WebChatDeepLinkStrategy.Context).WebChatId);
            Assert.AreEqual(wcdl.MsgId, (dlResult.Context as WebChatDeepLinkStrategy.Context).MessageId);

            //call with bad code
            strategy.Process(entities, new FakeOuterMessage() {
                FromId = Guid.NewGuid().ToString(),
                FromName = "fake user",
                Text = "Message with deep link #3",
                DeepLink = Guid.NewGuid().ToString()
            }, out dlResult);

            Assert.AreEqual(DeepLinkResultStatus.Unknown, dlResult.Status);
            Assert.IsNull(dlResult.Context);
        }

        [TestMethod]
        public void NewMessageStrategy_File() {
            User sender = entities.GetUser(u => true, null, TransportKind.Test);
            User addressee = entities.GetUser(u => u.Id != sender.Id, null, TransportKind.FLChat);

            string mediaTypeName = "text/txt";
            byte[] fileData = new byte[] { 1, 2, 3 };
            NewMessageStrategy strategy = new NewMessageStrategy(
                new ActionRouter(Guid.Empty),
                new ActionTransportIdSaver(),
                new ActionDeepLinkStrategy(),
                uploadFile: new ActionFileLoader((f) => {
                    return new DownloadFileResult(f, mediaTypeName, fileData);
                }));

            //make message
            FakeOuterMessage msg = new FakeOuterMessage() {
                FromId = sender.Transports.Get(TransportKind.Test).TransportOuterId,
                FromName = sender.FullName,
                Text = null,
                File = new FakeInputFile() {
                    FileName = Guid.NewGuid().ToString() + ".txt",
                    Type = MediaGroupKind.Document,
                    Media = "www.someurl.net"
                }
            };

            //handle new incoming message
            strategy.Process(entities, msg);

            Message dbmsg = entities.Message.OrderByDescending(m => m.Idx).Take(1).Single();
            Assert.IsNotNull(dbmsg.FileId);
            FileInfo fi = dbmsg.FileInfo;
            Assert.AreEqual(sender.Id, fi.FileOwnerId);
            Assert.AreEqual(msg.File.FileName, fi.FileName);
            Assert.AreEqual(mediaTypeName, fi.MediaType.Name);
            Assert.AreEqual(fileData.Length, fi.FileLength);
        }

        [TestMethod]
        public void NewMessageStrategy_FileError() {
            User sender = entities.GetUser(u => true, null, TransportKind.Test);
            User addressee = entities.GetUser(u => u.Id != sender.Id, null, TransportKind.FLChat);

            string exceptionText = "text exception " + Guid.NewGuid().ToString();
            NewMessageStrategy strategy = new NewMessageStrategy(
                new ActionRouter(Guid.Empty),
                new ActionTransportIdSaver(),
                new ActionDeepLinkStrategy(),
                uploadFile: new ActionFileLoader((f) => {
                    throw new Exception(exceptionText);
                }));

            //make message
            FakeOuterMessage msg = new FakeOuterMessage() {
                FromId = sender.Transports.Get(TransportKind.Test).TransportOuterId,
                FromName = sender.FullName,
                Text = null,
                File = new FakeInputFile() {
                    FileName = Guid.NewGuid().ToString() + ".txt",
                    Type = MediaGroupKind.Document,
                    Media = "www.someurl.net"
                }
            };

            //handle new incoming message
            strategy.Process(entities, msg);

            Message dbmsg = entities.Message.OrderByDescending(m => m.Idx).Take(1).Single();
            Assert.IsNull(dbmsg.FileId);
            Assert.IsTrue(dbmsg.MessageError.Any());
            Assert.IsTrue(dbmsg.MessageError.Where(me => me.Descr == exceptionText).Any());
        }
    }
}
