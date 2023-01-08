using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FLChat.DAL;
using FLChat.DAL.Model;

namespace FLChat.Core.Algorithms.Tests
{
    [TestClass]
    public class UnsubscribeStrategyTests
    {
        ChatEntities entities;
        UnsubscribeStrategy strategy = new UnsubscribeStrategy();

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Cleanup() {
            entities.Dispose();
        }

        [TestMethod]
        public void UnsubscribeStrategy_DisableTransport() {
            User user = entities.GetUser(
                u => u.Transports.Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.Test && t.TransportOuterId != String.Empty).Any(),
                u => u.Transports.Add(new Transport() { Enabled = true, Kind = TransportKind.Test, TransportOuterId = Guid.NewGuid().ToString() })
                );
            Transport transport = user.Transports.Get(TransportKind.Test);

            FakeUnsubscribeData data = new FakeUnsubscribeData() {
                UserId = transport.TransportOuterId
            };

            try {
                strategy.Process(entities, data);

                entities.Entry(transport).Reload();
                Assert.IsFalse(transport.Enabled);
            } finally {
                transport.Enabled = true;
                entities.SaveChanges();
            }
        }

        [TestMethod]
        public void UnsubscribeStrategy_NewUser() {
            FakeUnsubscribeData data = new FakeUnsubscribeData() {
                UserId = Guid.NewGuid().ToString()
            };

            strategy.Process(entities, data);

            Transport transport = entities
                .Transport
                .Where(t => t.TransportTypeId == (int)TransportKind.Test && t.TransportOuterId == data.UserId)
                .FirstOrDefault();
            Assert.IsNull(transport);
        }
    }
}
