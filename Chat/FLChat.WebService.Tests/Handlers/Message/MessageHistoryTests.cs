using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.WebService.Handlers.Message.Tests
{
    [TestClass]
    public class MessageHistoryTests
    {
        ChatEntities entities;
        MessageHistory handler;
        DAL.Model.User from;
        DAL.Model.User to;
        List<DAL.Model.Message> msgs;

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
            handler = new MessageHistory();

            DAL.Model.User []users = entities.GetUsers(2,
                u => u.Enabled && u.Transports.Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.FLChat).Any(),
                u => {
                    u.Enabled = true;
                    u.Transports.Add(new DAL.Model.Transport() {
                        TransportTypeId = (int)TransportKind.FLChat,
                        Enabled = true
                    });
                });
            from = users[0];
            to = users[1];

            //make "dialog"
            msgs = new List<DAL.Model.Message>();
            for (int i = 0; i < 10; ++i) {
                msgs.Add(entities.Message.Add(new DAL.Model.Message() {
                    FromTransportKind = TransportKind.FLChat,
                    FromUserId = i % 2 == 0 ? from.Id : to.Id,
                    Kind = MessageKind.Personal,
                    Text = "Message #" + i.ToString(),
                    ToUsers = new MessageToUser[] { new MessageToUser() {
                        ToTransportKind = TransportKind.FLChat,
                        IsSent = true,
                        ToUserId = i % 2 == 1 ? from.Id : to.Id
                    } }
                }));
                entities.SaveChanges();
                Thread.Sleep(50);
            }
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void MessageHistory_AllBackwardFT() {
            MessageHistoryResponse response = handler.ProcessRequest(entities, from, 
                new MessageHistoryRequest() { Count = msgs.Count, UserId = to.Id });
            Assert.IsNotNull(response);
            Assert.IsFalse(response.Forward);
            Assert.AreEqual(response.Order, OrderEnum.Descending);
            Assert.AreEqual(handler.MaxCount, response.MaxCount);
            Assert.AreEqual(to.Id, response.UserId);
            Assert.AreEqual(msgs[0].Id, response.LastId);
            Assert.AreEqual(msgs.Count, response.Messages.Count());

            CollectionAssert.AreEqual(
                (msgs as IEnumerable<DAL.Model.Message>).Reverse().ToArray(),
                response.Messages.ToArray(),
                new MsgComparer(from.Id, to.Id));
        }

        /// <summary>
        /// Sender and Addressee reverse test
        /// </summary>
        [TestMethod] 
        public void MessageHistory_AllBackwardTF() {
            MessageHistoryResponse response = handler.ProcessRequest(entities, to,
                new MessageHistoryRequest() { Count = msgs.Count, UserId = from.Id });
            Assert.IsNotNull(response);
            Assert.IsFalse(response.Forward);
            Assert.AreEqual(response.Order, OrderEnum.Descending);
            Assert.AreEqual(handler.MaxCount, response.MaxCount);
            Assert.AreEqual(from.Id, response.UserId);
            Assert.AreEqual(msgs[0].Id, response.LastId);
            Assert.AreEqual(msgs.Count, response.Messages.Count());

            CollectionAssert.AreEqual(
                (msgs as IEnumerable<DAL.Model.Message>).Reverse().ToArray(),
                response.Messages.ToArray(),
                new MsgComparer(to.Id, from.Id));
        }

        [TestMethod]
        public void MessageHistory_Forward() {
            const int index = 4;

            MessageHistoryResponse response = handler.ProcessRequest(entities, from,
                new MessageHistoryRequest() { Count = msgs.Count, UserId = to.Id, MessageId = msgs[index].Id });
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Forward);
            Assert.AreEqual(response.Order, OrderEnum.Ascending);
            Assert.AreEqual(msgs[msgs.Count - 1].Id, response.LastId);
            Assert.AreEqual(msgs.Count - index - 1, response.Messages.Count());

            CollectionAssert.AreEqual(
                (msgs as IEnumerable<DAL.Model.Message>).Skip(index + 1).ToArray(),
                response.Messages.ToArray(),
                new MsgComparer(from.Id, to.Id));
        }


        [TestMethod]
        public void MessageHistory_Backward() {
            const int index = 4;

            MessageHistoryResponse response = handler.ProcessRequest(entities, from,
                new MessageHistoryRequest() { Count = -msgs.Count, UserId = to.Id, MessageId = msgs[index].Id });
            Assert.IsNotNull(response);
            Assert.IsFalse(response.Forward);
            Assert.AreEqual(response.Order, OrderEnum.Descending);
            //Assert.AreEqual(msgs[msgs.Count - 1].Id, response.LastId);
            Assert.IsTrue(response.Messages.Count() >= 4);

            CollectionAssert.AreEqual(
                (msgs as IEnumerable<DAL.Model.Message>).Take(index).Reverse().ToArray(),
                response.Messages.Take(index).ToArray(),
                new MsgComparer(from.Id, to.Id));
        }


        [TestMethod]
        public void MessageHistory_ForwardCnt1() {
            MessageHistoryResponse response = handler.ProcessRequest(entities, from,
                new MessageHistoryRequest() { Count = 1, UserId = to.Id, MessageId = msgs[0].Id });
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Forward);
            Assert.AreEqual(msgs[msgs.Count - 1].Id, response.LastId);
            Assert.AreEqual(msgs.Count - 1, response.Messages.Count());

            CollectionAssert.AreEqual(
                (msgs as IEnumerable<DAL.Model.Message>).Skip(1).ToArray(),
                response.Messages.ToArray(),
                new MsgComparer(from.Id, to.Id));
        }

        [TestMethod]
        public void MessageHistory_BackwardCnt1() {
            MessageHistoryResponse response = handler.ProcessRequest(entities, from,
                new MessageHistoryRequest() { Count = -1, UserId = to.Id, MessageId = msgs[msgs.Count - 1].Id });
            Assert.IsNotNull(response);
            Assert.IsFalse(response.Forward);
            //Assert.AreEqual(msgs[msgs.Count - 1].Id, response.LastId);
            Assert.IsTrue(response.Messages.Count() >= msgs.Count - 1);

            CollectionAssert.AreEqual(
                (msgs as IEnumerable<DAL.Model.Message>).Take(msgs.Count - 1).Reverse().ToArray(),
                response.Messages.Take(msgs.Count - 1).ToArray(),
                new MsgComparer(from.Id, to.Id));
        }

        [TestMethod]
        public void MessageHistory_MessageBecomeDelivered() {
            DAL.Model.User[] users = entities.GetUsers(2, u => u.Enabled, null, TransportKind.FLChat);
            DAL.Model.User from = users[0];
            DAL.Model.User to = users[1];

            //make messages
            List<DAL.Model.Message> msgs = new List<DAL.Model.Message>();
            for (int i = 0; i < 3; ++i) {
                msgs.Add(entities.Message.Add(new DAL.Model.Message() {
                    FromTransportKind = TransportKind.FLChat,
                    FromUserId = from.Id,
                    Kind = MessageKind.Personal,
                    Text = "Message #" + i.ToString(),
                    ToUsers = new MessageToUser[] { new MessageToUser() {
                        ToTransportKind = TransportKind.FLChat,
                        IsSent = true,
                        ToUserId = to.Id
                    } }
                }));
            }
            entities.SaveChanges();

            //check delivered before
            Assert.IsFalse(msgs.Select(m => m.ToUsers.Single().IsDelivered).Distinct().Single());

            MessageHistoryResponse resp = handler.ProcessRequest(entities, to, new MessageHistoryRequest() { Count = msgs.Count, UserId = from.Id });

            //check all our messages was returned
            Assert.AreEqual(msgs.Count(), resp.Messages.Select(m => m.Id).Intersect(msgs.Select(m => m.Id)).Count());

            //reload
            foreach (DAL.Model.Message msg in msgs)
                entities.Entry(msg.ToUsers.Single()).Reload();

            //check delivered after
            Assert.IsTrue(msgs.Select(m => m.ToUsers.Single().IsDelivered).Distinct().Single());
        }

        /// <summary>
        /// Test returned LastId in response without messages
        /// </summary>
        [TestMethod]
        public void MessageHistory_LastId_in_EmptyResponse() {
            DAL.Model.Message msg = entities.Message.OrderByDescending(m => m.Idx).Take(1).Single();
            MessageHistoryResponse resp = handler.ProcessRequest(entities, msg.FromTransport.User, new MessageHistoryRequest() {
                Count = 1,
                MessageId = msg.Id,
                UserId = msg.ToUsers.First().ToUserId
            });
            Assert.AreEqual(0, resp.Messages.Count());
            Assert.IsNotNull(resp.LastId);
            Assert.AreEqual(msg.Id, resp.LastId.Value);
        }

        public class MsgComparer : System.Collections.IComparer
        {
            private Guid _sender;
            private Guid _to;

            public MsgComparer(Guid sender, Guid to) {
                _sender = sender;
                _to = to;
            }

            public int Compare(object x, object y) {
                DAL.Model.Message msg = x as DAL.Model.Message;
                MessageInfo info = y as MessageInfo;

                Assert.AreEqual(msg.Id, info.Id);
                Assert.AreEqual(msg.FromUserId, info.FromUserId);
                Assert.AreEqual(msg.FromTransportKind, info.FromTransport);
                Assert.AreEqual(msg.PostTm, info.PostTm);
                Assert.AreEqual(msg.Text, info.Text);
                if (_sender == msg.FromUserId) {
                    Assert.IsFalse(info.Incoming);
                    MessageOutcomeToOneInfo info2 = y as MessageOutcomeToOneInfo;
                    Assert.IsNotNull(info2);
                    Assert.AreEqual(_to, info2.ToUserId);
                    MessageToUser toUser = msg.ToUsers.Where(t => t.ToUserId == _to).Single();
                    Assert.AreEqual(toUser.ToTransportKind, info2.ToTransport);
                    Assert.AreEqual(toUser.GetMessageStatus(), info2.Status);
                } else {
                    Assert.IsTrue(info.Incoming);
                    Assert.IsNotInstanceOfType(y, typeof(MessageOutcomeToOneInfo));
                    Assert.IsInstanceOfType(y, typeof(MessageIncomeInfo));
                }

                return 0;
            }
        }

        [TestMethod]
        public void MessageHistory_OrderAsc() {
            MessageHistoryResponse response = handler.ProcessRequest(entities, from,
                new MessageHistoryRequest() { Count = msgs.Count, UserId = to.Id, Order = OrderEnum.Ascending });
            Assert.IsNotNull(response);
            Assert.IsFalse(response.Forward);
            Assert.AreEqual(response.Order, OrderEnum.Ascending);

            CollectionAssert.AreEqual(
                (msgs as IEnumerable<DAL.Model.Message>).Select(m => m.Id).ToArray(),
                response.Messages.Select(m => m.Id).ToArray());
        }

        [TestMethod]
        public void MessageHistory_OrderDesc() {
            MessageHistoryResponse response = handler.ProcessRequest(entities, from,
                new MessageHistoryRequest() { Count = msgs.Count, UserId = to.Id, Order = OrderEnum.Descending });
            Assert.IsNotNull(response);
            Assert.IsFalse(response.Forward);
            Assert.AreEqual(response.Order, OrderEnum.Descending);

            CollectionAssert.AreEqual(
                (msgs as IEnumerable<DAL.Model.Message>).Reverse().Select(m => m.Id).ToArray(),
                response.Messages.Select(m => m.Id).ToArray());
        }

        /// <summary>
        /// Check message history returns only inner transports messages
        /// </summary>
        [TestMethod]
        public void MessageHistory_OuterChannelsIsOmitting() {
            //get users
            DAL.Model.User[] users = entities.GetUsers(2, 
                u => u.Enabled && u.Id != from.Id && u.Id != to.Id 
                    && u.Transports.Where(t => t.TransportTypeId == (int)TransportKind.FLChat && t.Enabled).Any()
                    && u.Transports.Where(t => t.TransportTypeId == (int)TransportKind.Test && t.Enabled).Any(),
                u => {
                    u.Transports.Add(new DAL.Model.Transport() { Enabled = true, Kind = TransportKind.FLChat });
                    u.Transports.Add(new DAL.Model.Transport() { Enabled = true, Kind = TransportKind.Test, TransportOuterId = Guid.NewGuid().ToString() });
                });
            DAL.Model.User u1 = users[0];
            DAL.Model.User u2 = users[1];

            //make messages
            List<DAL.Model.Message> list = new List<DAL.Model.Message>();

            list.Add(entities.Message.Add(new DAL.Model.Message() {
                FromUserId = u1.Id,
                FromTransportKind = TransportKind.FLChat,
                Kind = MessageKind.Personal,
                Text = "flchat -> flchat",
                ToUsers = new MessageToUser[] {
                    new MessageToUser() {
                        ToUserId = u2.Id,
                        ToTransportKind = TransportKind.FLChat,                        
                        IsSent = true
                    }

                }
            }));
            entities.SaveChanges();

            list.Add(entities.Message.Add(new DAL.Model.Message() {
                FromUserId = u1.Id,
                FromTransportKind = TransportKind.FLChat,
                Kind = MessageKind.Personal,
                Text = "flchat -> test",
                ToUsers = new MessageToUser[] {
                    new MessageToUser() {
                        ToUserId = u2.Id,
                        ToTransportKind = TransportKind.Test,
                        IsSent = true
                    }

                }
            }));
            entities.SaveChanges();

            list.Add(entities.Message.Add(new DAL.Model.Message() {
                FromUserId = u1.Id,
                FromTransportKind = TransportKind.Test,
                Kind = MessageKind.Personal,
                Text = "test -> flchat",
                ToUsers = new MessageToUser[] {
                    new MessageToUser() {
                        ToUserId = u2.Id,
                        ToTransportKind = TransportKind.FLChat,
                        IsSent = true
                    }

                }
            }));
            entities.SaveChanges();

            {
                //get last 3 messages for dialog u1 -> u2
                MessageHistoryResponse resp = handler.ProcessRequest(entities, u1, new MessageHistoryRequest() { Count = 3, UserId = u2.Id });
                Guid[] ids = resp.Messages.Select(m => m.Id).ToArray();
                CollectionAssert.Contains(ids, list[0].Id);  //contains flchat->flchat
                CollectionAssert.Contains(ids, list[1].Id);  //contains flchat->test
                CollectionAssert.DoesNotContain(ids, list[2].Id);//does not contain test->flchat
            }

            {
                //get last 3 messages for dialog u2 -> u1
                MessageHistoryResponse resp = handler.ProcessRequest(entities, u2, new MessageHistoryRequest() { Count = 3, UserId = u1.Id });
                Guid[] ids = resp.Messages.Select(m => m.Id).ToArray();
                CollectionAssert.Contains(ids, list[0].Id);  //contains flchat->flchat
                CollectionAssert.DoesNotContain(ids, list[1].Id);  //does not contain test->flchat
                CollectionAssert.Contains(ids, list[2].Id);//contains flchat->test
            }

        }

        /// <summary>
        /// Check message history not returns Mailing messages
        /// </summary>
        [TestMethod]
        public void MessageHistory_MailingIsOmitting()
        {
            //get users
            DAL.Model.User[] users = entities.GetUsers(2,
                u => u.Enabled && u.Id != from.Id && u.Id != to.Id && u.Email != null
                    && u.Transports.Where(t => t.TransportTypeId == (int)TransportKind.FLChat && t.Enabled).Any(),
                    //&& u.Transports.Where(t => t.TransportTypeId == (int)TransportKind.Test && t.Enabled).Any(),
                u => {
                    u.Enabled = true;                    
                    u.Email = Guid.NewGuid() + "@ya.com";
                    u.Transports.Add(new DAL.Model.Transport() { Enabled = true, Kind = TransportKind.FLChat });
                    //u.Transports.Add(new DAL.Model.Transport() { Enabled = true, Kind = TransportKind.Test, TransportOuterId = Guid.NewGuid().ToString() });
                });
            DAL.Model.User u1 = users[0];
            DAL.Model.User u2 = users[1];
            entities.SaveChanges();

            //make messages
            List<DAL.Model.Message> list = new List<DAL.Model.Message>();

            list.Add(entities.Message.Add(new DAL.Model.Message()
            {
                FromUserId = u1.Id,
                FromTransportKind = TransportKind.FLChat,
                Kind = MessageKind.Mailing,
                Text = "flchat -> Email",
                ToUsers = new MessageToUser[] {
                    new MessageToUser() {
                        ToUserId = u2.Id,
                        ToTransportKind = TransportKind.Email,
                        IsSent = true
                    }

                }
            }));
            entities.SaveChanges();

            list.Add(entities.Message.Add(new DAL.Model.Message()
            {
                FromUserId = u1.Id,
                FromTransportKind = TransportKind.FLChat,
                Kind = MessageKind.Mailing,
                Text = "flchat -> test(Mailing)",
                ToUsers = new MessageToUser[] {
                    new MessageToUser() {
                        ToUserId = u2.Id,
                        ToTransportKind = TransportKind.Test,
                        IsSent = true
                    }

                }
            }));
            entities.SaveChanges();
            
            {
                //get last 2 messages for dialog u1 -> u2
                MessageHistoryResponse resp = handler.ProcessRequest(entities, u1, new MessageHistoryRequest() { Count = 2, UserId = u2.Id });
                Guid[] ids = resp.Messages.Select(m => m.Id).ToArray();
                CollectionAssert.DoesNotContain(ids, list[0].Id);  //contains flchat->flchat
                CollectionAssert.DoesNotContain(ids, list[1].Id);  //contains flchat->test                
            }

            {
                //get last 2 messages for dialog u2 -> u1
                MessageHistoryResponse resp = handler.ProcessRequest(entities, u2, new MessageHistoryRequest() { Count = 2, UserId = u1.Id });
                Guid[] ids = resp.Messages.Select(m => m.Id).ToArray();
                CollectionAssert.DoesNotContain(ids, list[0].Id);  //contains flchat->flchat
                CollectionAssert.DoesNotContain(ids, list[1].Id);  //does not contain test->flchat                
            }

        }
    }
}
