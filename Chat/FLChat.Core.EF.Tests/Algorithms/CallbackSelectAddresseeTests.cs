using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FLChat.DAL.Model;
using FLChat.DAL;
using FLChat.Core;

namespace FLChat.Core.Algorithms.Tests
{
    [TestClass]
    public class CallbackSelectAddresseeTests
    {
        ChatEntities entities;
        CallbackSelectAddressee handler = new CallbackSelectAddressee();

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Cleanup() {
            entities.Dispose();
        }

        [TestMethod]
        public void CallbackSelectAddresseeTest() {
            User user = entities.GetUser(u => u.OwnerUserId != null, u => u.OwnerUser = entities.GetUser(null, null), TransportKind.Test);
            Transport transport = user.Transports.Get(TransportKind.Test);
            User addr = entities.GetUser(u => u.Id != user.Id && u.Id != user.OwnerUserId, null, TransportKind.FLChat);
            User savedAddr = transport.MsgAddressee;
            try {
                transport.MsgAddressee = null;
                entities.SaveChanges();
                entities.Entry(transport).Reload();//.ComplexProperty(t => t.MsgAddressee).

                FakeCallbackData data = new FakeCallbackData() {
                    Data = ICallbackDataExtentions.ADDRESSEE_SWITCH + addr.Id.ToString()
                };

                handler.Process(entities, transport, data);
                entities.SaveChanges();
                entities.Entry(transport).Reload();//.ComplexProperty(t => t.MsgAddressee).
                Assert.AreEqual(addr.Id, transport.MsgAddressee.Id);

                data.Data = ICallbackDataExtentions.ADDRESSEE_SWITCH + user.OwnerUserId.ToString();
                handler.Process(entities, transport, data);
                entities.SaveChanges();
                entities.Entry(transport).Reload();//.ComplexProperty(t => t.MsgAddressee).
                Assert.IsNull(transport.MsgAddressee);

            } finally {
                transport.MsgAddressee = savedAddr;
                entities.SaveChanges();
            }
        }

        [TestMethod]
        public void CallbackSelectAddressee_NotifyMessage() {
            User user = entities.GetUser(u => u.OwnerUserId != null, u => u.OwnerUser = entities.GetUser(null, null), TransportKind.Test);
            Transport transport = user.Transports.Get(TransportKind.Test);
            User addr = entities.GetUser(u => u.Id != user.Id && u.Id != user.OwnerUserId, null, TransportKind.FLChat);
            User savedAddr = transport.MsgAddressee;

            Guid botGuid = entities.SystemBot.Id;
            MessageToUser prev = entities.MessageToUser
                .Where(m => m.Message.FromUserId == botGuid && m.ToUserId == user.Id && m.ToTransportTypeId == (int)TransportKind.Test)
                .OrderByDescending(m => m.Idx)
                .FirstOrDefault();

            try {
                FakeCallbackData data = new FakeCallbackData() {
                    Data = ICallbackDataExtentions.ADDRESSEE_SWITCH + addr.Id.ToString()
                };

                handler.Process(entities, transport, data);
                entities.SaveChanges();

                MessageToUser last = entities.MessageToUser
                    .Where(m => m.Message.FromUserId == botGuid && m.ToUserId == user.Id && m.ToTransportTypeId == (int)TransportKind.Test)
                    .OrderByDescending(m => m.Idx)
                    .FirstOrDefault();
                Assert.IsNotNull(last);
                if (prev != null)
                    Assert.AreNotEqual(prev.Message.Id, last.Message.Id);

            } finally {
                transport.MsgAddressee = savedAddr;
                entities.SaveChanges();
            }
        }
    }
}
