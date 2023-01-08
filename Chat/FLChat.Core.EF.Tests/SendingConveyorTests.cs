using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Threading;
using System.Collections.Concurrent;

using FLChat.DAL.Model;
using FLChat.DAL;

namespace FLChat.Core.Tests
{
    [TestClass]
    public class SendingConveyorTests
    {
        ChatEntities entities;
        User from;

        [TestInitialize]
        public void Init()
        {
            entities = new ChatEntities();
            from = entities.GetUser(u => u.Enabled, null, TransportKind.FLChat);
        }

        [TestCleanup]
        public void Cleanup()
        {
            entities.Dispose();
        }

        [TestMethod]
        public void SendingConveyor_SendMany_PeriodTest()
        {
            ConcurrentBag<SentMessageInfo> sent = new ConcurrentBag<SentMessageInfo>();
            TestIdSaver idSaver = new TestIdSaver();

            SendingConveyor sender = new SendingConveyor(
                new ActionMessageSender((m, t) =>
                {
                    SentMessageInfo inf = new SentMessageInfo(m.MsgId.ToString(), DateTime.UtcNow);
                    sent.Add(inf);
                    Assert.AreEqual(m.Message.Text, t);
                    return inf;
                }),
                TransportKind.Test,
                idSaver)
            {
                MessagePerSecond = 2
            };

            Message[] msgs = CreateMessagesToDifferentUsers(5);

            //check flags
            Assert.IsFalse(msgs.Select(m => m.ToUsers.Single().IsSent).Distinct().Single());
            Assert.IsFalse(msgs.Select(m => m.ToUsers.Single().IsFailed).Distinct().Single());

            sender.Send(CancellationToken.None);

            //check all outs messages has sent status
            foreach (Message m in msgs)
            {
                //entities.Entry(m).Reload();                
                entities.Entry(m.ToUsers.Single()).Reload();
                Assert.IsTrue(m.ToUsers.Single().IsSent);
                Assert.IsFalse(m.ToUsers.Single().IsFailed);
                Assert.IsFalse(entities.MessageError.Where(me => me.MsgId == m.Id).Any());
            }


            //check sent messages include ours
            var msgIds = msgs.Select(m => m.Id).Select(g => g.ToString()).ToArray();
            Assert.AreEqual(msgs.Length, sent.Select(i => i.MessageId).Intersect(msgIds).Count());

            //check distance beetwen messages
            SentMessageInfo[] sentarray = sent.OrderBy(s => s.SentTime).ToArray();
            int index = 0;
            while (index + sender.MessagePerSecond < sentarray.Length)
            {
                double tm = (sentarray[index + sender.MessagePerSecond].SentTime - sentarray[index].SentTime).TotalSeconds;
                Assert.IsTrue(tm > 0.95, $"Time {tm.ToString()} between {(index + sender.MessagePerSecond).ToString()} and {index.ToString()} less then second");
                Assert.IsTrue(tm < 1.25, $"Time {tm.ToString()} between {(index + sender.MessagePerSecond).ToString()} and {index.ToString()} too much");
                index += 1;
            }

            //check saved id
            Assert.IsTrue(msgs.Length <= idSaver.Ids.Count());
            Assert.AreEqual(msgs.Length, idSaver.Ids.Select(t => t.Item1).Intersect(msgs.Select(m => m.Id.ToString())).Count());
            foreach (var sid in idSaver.Ids)
            {
                Assert.AreEqual(sid.Item2.Message.Id.ToString(), sid.Item1);
            }
        }

        [TestMethod]
        public void SendingConveyor_SendFail()
        {
            SendingConveyor sender = new SendingConveyor(
                new ActionMessageSender((m, t) =>
                {
                    throw new Exception("test");
                }),
                TransportKind.Test,
                new TestIdSaver())
            {
                MessagePerSecond = 2
            };

            Message[] msgs = CreateMessagesToDifferentUsers(5);

            //check flags
            Assert.IsFalse(msgs.Select(m => m.ToUsers.Single().IsSent).Distinct().Single());
            Assert.IsFalse(msgs.Select(m => m.ToUsers.Single().IsFailed).Distinct().Single());

            sender.Send(CancellationToken.None);

            //check all outs messages has failed status
            foreach (Message m in msgs)
            {
                //entities.Entry(m).Reload();                
                entities.Entry(m.ToUsers.Single()).Reload();
                Assert.IsFalse(m.ToUsers.Single().IsSent);
                Assert.IsTrue(m.ToUsers.Single().IsFailed);
                MessageError e = entities.MessageError.Where(me => me.MsgId == m.Id).Single();
                Assert.AreEqual(e.Type, nameof(Exception));
                Assert.AreEqual(e.Descr, "test");
                Assert.IsNotNull(e.Trace);
            }
        }

        [TestMethod]
        public void SendingConveyor_MessageCompiler_Default()
        {
            int count = 0;

            SendingConveyor sender = new SendingConveyor(
                new ActionMessageSender((m, t) =>
                {
                    count += 1;
                    Assert.AreEqual(m.Message.Text, t);
                    return new SentMessageInfo(m.MsgId.ToString(), DateTime.UtcNow);
                }),
                TransportKind.Test,
                new TestIdSaver())
            {
                MessagePerSecond = 2
            };

            Message[] msgs = CreateMessagesToDifferentUsers(1);
            sender.Send(CancellationToken.None);

            //check flags
            Assert.IsTrue(count > 0);
        }

        private class FakeMessageCompiler : IMessageTextCompiler
        {
            public string MakeText(MessageToUser mtu, string text)
            {
                return "fake one " + text;
            }
        }

        [TestMethod]
        public void SendingConveyor_MessageCompiler_Assign()
        {
            int count = 0;

            SendingConveyor sender = new SendingConveyor(
                new ActionMessageSender((m, t) =>
                {
                    count += 1;
                    Assert.AreEqual("fake one " + m.Message.Text, t);
                    return new SentMessageInfo(m.MsgId.ToString(), DateTime.UtcNow);
                }),
                TransportKind.Test,
                idSaver: new TestIdSaver(),
                msgCompiler: new FakeMessageCompiler())
            {
                MessagePerSecond = 2
            };

            Message[] msgs = CreateMessagesToDifferentUsers(1);
            sender.Send(CancellationToken.None);

            //check flags
            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void SendingConveyor_SaveManyIds()
        {
            string[] ids = null;
            TestIdSaver saver = new TestIdSaver();
            SendingConveyor sender = new SendingConveyor(
                new ActionMessageSender((m, t) =>
                {
                    ids = new string[] { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };
                    return new SentMessageInfo(ids, DateTime.UtcNow);
                }),
                TransportKind.Test,
                idSaver: saver,
                msgCompiler: null);

            Message[] msgs = CreateMessagesToDifferentUsers(1);
            sender.Send(CancellationToken.None);

            Assert.IsNotNull(ids);
            CollectionAssert.AreEquivalent(ids, saver.LastIds);
        }
        
        private Message[] CreateMessagesToDifferentUsers(int cnt)
        {
            User[] to = entities.GetUsers(cnt, u => u.Enabled, null, TransportKind.Test);
            Message[] result = new Message[cnt];
            for (int i = 0; i < cnt; ++i)
            {
                Message msg = new Message()
                {
                    Kind = DAL.MessageKind.Personal,
                    FromUserId = from.Id,
                    FromTransportKind = TransportKind.FLChat,
                    Text = "Test message #" + i.ToString(),
                    ToUsers = new MessageToUser[] { new MessageToUser() {
                        ToUserId = to[i].Id,
                        ToTransportKind = TransportKind.Test
                    } }
                };
                result[i] = msg;
                entities.Message.Add(msg);
            }
            entities.SaveChanges();
            return result;
        }
    }
}
