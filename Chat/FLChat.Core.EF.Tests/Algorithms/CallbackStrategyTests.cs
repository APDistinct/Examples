using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL.Model;
using FLChat.DAL;

namespace FLChat.Core.Algorithms.Tests
{
    [TestClass]
    public class CallbackStrategyTests
    {
        private ChatEntities entities;
        private User user;

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
            user = entities.GetUser(null, null, TransportKind.Test);
        }

        [TestCleanup]
        public void Cleanup() {
            entities.Dispose();
        }

        [TestMethod]
        public void CallbackStrategy_Test() {
            FakeCallbackData callbackData = new FakeCallbackData() {
                FromUserId = user.Transports.Get(TransportKind.Test).TransportOuterId
            };
            bool called = false;
            CallbackStrategy callback = new CallbackStrategy(new ActionCallbackDataProcessor((ent, t, d) => {
                Assert.AreSame(entities, ent);
                Assert.AreSame(callbackData, d);
                Assert.AreEqual(user.Transports.Get(TransportKind.Test).Kind, t.Kind);
                Assert.AreEqual(user.Transports.Get(TransportKind.Test).TransportOuterId, t.TransportOuterId);
                Assert.AreEqual(user.Id, t.UserId);
                called = true;
            }));

            callback.Process(entities, callbackData);
            Assert.IsTrue(called);

            Message msg = entities.Message.Where(m => m.FromUserId == user.Id).OrderByDescending(m => m.Idx).First();
            Assert.AreEqual(entities.SystemBot.Id, msg.ToUser.ToUserId);
            Assert.AreEqual(TransportKind.FLChat, msg.ToTransport.Kind);
            Assert.AreEqual(TransportKind.Test, msg.FromTransportKind);
            Assert.AreEqual(user.Id, msg.FromUserId);
            Assert.AreEqual(callbackData.Data, msg.Text);
        }

        [TestMethod]
        public void CallbackStrategy_ClearAddr() {
            CallbackStrategy strategy = new CallbackStrategy(new CallbackSelectAddressee());

            User user = entities.GetUser(u => u.OwnerUserId != null, u => u.OwnerUser = entities.GetUser(null, null), TransportKind.Test);
            Transport transport = user.Transports.Get(TransportKind.Test);
            User addr = entities.GetUser(u => u.Id != user.Id && u.Id != user.OwnerUserId, null, TransportKind.FLChat);
            User savedAddr = transport.MsgAddressee;

            try {
                transport.MsgAddressee = null;
                entities.SaveChanges();
                entities.Entry(transport).Reload();//.ComplexProperty(t => t.MsgAddressee).

                FakeCallbackData data = new FakeCallbackData() {
                    TransportKind = TransportKind.Test,
                    Id = Guid.NewGuid().ToString(),
                    FromUserId = transport.TransportOuterId,                    
                    Data = ICallbackDataExtentions.ADDRESSEE_SWITCH + addr.Id.ToString()
                };

                strategy.Process(entities, data);                
                entities.Entry(transport).Reload();//.ComplexProperty(t => t.MsgAddressee).
                Assert.AreEqual(addr.Id, transport.MsgAddressee.Id);

                data.Data = ICallbackDataExtentions.ADDRESSEE_SWITCH + user.OwnerUserId.ToString();
                strategy.Process(entities, data);                
                entities.Entry(transport).Reload();//.ComplexProperty(t => t.MsgAddressee).
                Assert.IsNull(transport.MsgAddressee);

            } finally {
                transport.MsgAddressee = savedAddr;
                entities.SaveChanges();
            }
        }
    }
}
