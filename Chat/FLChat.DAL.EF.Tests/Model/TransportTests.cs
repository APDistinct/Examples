using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.DAL.Model.Tests
{
    [TestClass]
    public class TransportTests
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
        public void Transport_GetTransportByOuterId() {
            string tid = Guid.NewGuid().ToString();
            User u1 = entities.User.Add(new User() { Enabled = true });
            u1.Transports.Add(new Transport() {
                Kind = TransportKind.Test,
                Enabled = true,
                TransportOuterId = tid
            });
            entities.SaveChanges();
            Transport transport1 = u1.Transports.Get(TransportKind.Test);

            Assert.IsNotNull(entities.Transport.GetTransportByOuterId(TransportKind.Test, tid, onlyEnabled: true));

            transport1.Enabled = false;
            entities.SaveChanges();
            Assert.IsNull(entities.Transport.GetTransportByOuterId(TransportKind.Test, tid, onlyEnabled: true));
            Assert.IsNotNull(entities.Transport.GetTransportByOuterId(TransportKind.Test, tid, onlyEnabled: false));

            /*User u2 = entities.User.Add(new User() { Enabled = true });
            u2.Transports.Add(new Transport() {
                Kind = TransportKind.Test,
                Enabled = true,
                TransportOuterId = tid
            });
            entities.SaveChanges();
            Transport transport2 = u2.Transports.Get(TransportKind.Test);

            Assert.AreEqual(u2.Id, entities.Transport.GetTransportByOuterId(TransportKind.Test, tid, onlyEnabled: true).UserId);
            Assert.AreEqual(u2.Id, entities.Transport.GetTransportByOuterId(TransportKind.Test, tid, onlyEnabled: false).UserId);*/
        }

        [TestMethod]
        public void Transport_ChangeAddresseeTest() {
            User user = entities.GetUser(u => u.OwnerUserId != null, u => u.OwnerUser = entities.GetUser(null, null), TransportKind.Test);
            Transport transport = user.Transports.Get(TransportKind.Test);
            User addr = entities.GetUser(u => u.Id != user.Id && u.Id != user.OwnerUserId, null, TransportKind.FLChat);
            User savedAddr = transport.MsgAddressee;
            try {
                transport.MsgAddressee = null;
                entities.SaveChanges();
                entities.Entry(transport).Reload();

                transport.ChangeAddressee(entities, addr);
                entities.SaveChanges();
                entities.Entry(transport).Reload();
                Assert.AreEqual(addr.Id, transport.MsgAddressee.Id);

                transport.ChangeAddressee(entities, user.OwnerUser);
                entities.SaveChanges();
                entities.Entry(transport).Reload();
                Assert.IsNull(transport.MsgAddressee);

            } finally {
                transport.MsgAddressee = savedAddr;
                entities.SaveChanges();
            }
        }

        [TestMethod]
        public void Transport_IsAddressee_Test() {
            User user = entities.GetUser(u => u.OwnerUserId != null, u => u.OwnerUser = entities.GetUser(null, null), TransportKind.Test);
            Transport transport = user.Transports.Get(TransportKind.Test);
            User addr = entities.GetUser(u => u.Id != user.Id && u.Id != user.OwnerUserId, null, TransportKind.FLChat);
            User savedAddr = transport.MsgAddressee;
            try {
                transport.MsgAddressee = null;
                entities.SaveChanges();
                entities.Entry(transport).Reload();

                Assert.IsTrue(transport.IsAddressee(user.OwnerUser));
                Assert.IsFalse(transport.IsAddressee(addr));

                transport.ChangeAddressee(entities, addr);
                entities.SaveChanges();
                entities.Entry(transport).Reload();

                Assert.IsFalse(transport.IsAddressee(user.OwnerUser));
                Assert.IsTrue(transport.IsAddressee(addr));
            } finally {
                transport.MsgAddressee = savedAddr;
                entities.SaveChanges();
            }
        }
    }
}
