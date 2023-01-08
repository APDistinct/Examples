using System;
using System.Linq;
using FLChat.DAL;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Core.Tests
{
    [TestClass]
    public class MessageLoaderTests
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
        public void MessageLoader_TestOK()
        {
            MessageLoader loader = new MessageLoader(TransportKind.Test);
            Message[] msgs = CreateMessagesToDifferentUsers(1);
            var msg = msgs[0].ToUsers.Single();

            entities.Entry(msg).Reload();
            var ret = loader.Load(entities);
            Assert.IsTrue(ret.Contains(msg));
        }

        [TestMethod]
        public void MessageLoader_TestNO_IsSent()
        {
            MessageLoader loader = new MessageLoader(TransportKind.Test);
            Message[] msgs = CreateMessagesToDifferentUsers(1);
            var msg = msgs[0].ToUsers.Single();

            entities.Entry(msg).Reload();
            msg.IsSent = true;
            entities.SaveChanges();
            var ret = loader.Load(entities);
            Assert.IsFalse(ret.Contains(msg));
        }

        [TestMethod]
        public void MessageLoader_TestNO_IsFailed()
        {
            MessageLoader loader = new MessageLoader(TransportKind.Test);
            Message[] msgs = CreateMessagesToDifferentUsers(1);
            var msg = msgs[0].ToUsers.Single();

            entities.Entry(msg).Reload();
            msg.IsFailed = true;
            entities.SaveChanges();
            var ret = loader.Load(entities);
            Assert.IsFalse(ret.Contains(msg));
        }

        [TestMethod]
        public void MessageLoader_Test_Time()
        {
            MessageLoader loader = new MessageLoader(TransportKind.Test);
            Message[] msgs = CreateMessagesToDifferentUsers(1,DateTime.UtcNow.AddDays(1));
            var msg = msgs[0].ToUsers.Single();

            entities.Entry(msg).Reload();            
            var ret = loader.Load(entities);
            Assert.IsFalse(ret.Contains(msg));
            msgs[0].DelayedStart = DateTime.UtcNow.AddDays(-1);
            entities.SaveChanges();
            ret = loader.Load(entities);
            Assert.IsTrue(ret.Contains(msg));
        }

        private Message[] CreateMessagesToDifferentUsers(int cnt, DateTime? dt = null)
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
                    } },
                    DelayedStart = dt,
                };
                result[i] = msg;
                entities.Message.Add(msg);
            }
            entities.SaveChanges();
            return result;
        }
    }
}
