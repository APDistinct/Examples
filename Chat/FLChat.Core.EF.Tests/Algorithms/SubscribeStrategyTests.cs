using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FLChat.DAL;
using FLChat.DAL.Model;

namespace FLChat.Core.Algorithms.Tests
{
    [TestClass]
    public class SubscribeStrategyTests
    {
        ChatEntities entities;
        SubscribeStrategy strategy = new SubscribeStrategy();

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Cleanup() {
            entities.Dispose();
        }

        [TestMethod]
        public void SubscribeStrategy_EnableTransport() {
            User user = entities.GetUser(
                u => u.Transports.Where(t => t.Enabled == false && t.TransportTypeId == (int)TransportKind.Test && t.TransportOuterId != String.Empty).Any(),
                u => u.Transports.Add(new Transport() { Enabled = false, Kind = TransportKind.Test, TransportOuterId = Guid.NewGuid().ToString() })
                );
            Transport transport = user.Transports.Get(TransportKind.Test);

            //change transport outer Id, because now database easyly can has same transport id in enabled and disabled records
            transport.TransportOuterId = Guid.NewGuid().ToString();
            entities.SaveChanges();

            FakeSubscribeData data = new FakeSubscribeData() {
                UserId = transport.TransportOuterId
            };

            try {
                strategy.Process(entities, data);

                entities.Entry(transport).Reload();
                Assert.IsTrue(transport.Enabled);
            } finally {
                transport.Enabled = false;
                entities.SaveChanges();
            }
        }

        [TestMethod]
        public void SubscribeStrategy_NewUser() {
            FakeSubscribeData data = new FakeSubscribeData() {
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
