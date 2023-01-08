using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.Core;
using FLChat.Viber.Client.Types;
using FLChat.Viber.Bot.Adapters;

namespace FLChat.Viber.Bot.Algorithms.Tests
{
    [TestClass]
    public class DeepLinkStrategyTests
    {
        ChatEntities entities;
        DeepLinkStrategy strategy = new DeepLinkStrategy();

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void DeepLinkStrategy_BeforeAdd_ClearDisabled() {
            using (var trans = entities.Database.BeginTransaction()) {
                DAL.Model.User userDisabled = entities.GetUser(
                    u => u.Enabled && u.Transports.Where(t =>
                        t.Enabled == false
                        && t.TransportTypeId == (int)TransportKind.Viber
                        && t.TransportOuterId != String.Empty).Any(),
                    u => u.Transports.Add(new Transport() {
                        Enabled = false,
                        TransportTypeId = (int)TransportKind.Viber,
                        TransportOuterId = Guid.NewGuid().ToString()
                    }));
                Transport transport = userDisabled.Transports.Where(t => t.TransportTypeId == (int)TransportKind.Viber).Single();
                FakeOuterMessage msg = new FakeOuterMessage() {
                    FromId = transport.TransportOuterId
                };
                strategy.BeforeAddTransport(entities, msg, null);

                entities.Entry(transport).Reload();
                Assert.AreEqual(String.Empty, transport.TransportOuterId);
                //Assert.AreEqual(System.Data.Entity.EntityState.Modified, entities.Entry(transport).State);

                trans.Rollback();
            }

            //entities.Entry(transport).Reload();
        }

        [TestMethod]
        public void DeepLinkStrategy_BeforeAdd_AvoidEnabled() {
            DAL.Model.User userDisabled = entities.GetUser(
                u => u.Enabled && u.Transports.Where(t =>
                    t.Enabled == true
                    && t.TransportTypeId == (int)TransportKind.Viber
                    && t.TransportOuterId != String.Empty).Any(),
                u => u.Transports.Add(new Transport() {
                    Enabled = true,
                    TransportTypeId = (int)TransportKind.Viber,
                    TransportOuterId = Guid.NewGuid().ToString()
                }));
            Transport transport = userDisabled.Transports.Where(t => t.TransportTypeId == (int)TransportKind.Viber).Single();
            FakeOuterMessage msg = new FakeOuterMessage() {
                FromId = transport.TransportOuterId
            };
            strategy.BeforeAddTransport(entities, msg, null);

            Assert.AreNotEqual(String.Empty, transport.TransportOuterId);
            Assert.AreEqual(System.Data.Entity.EntityState.Unchanged, entities.Entry(transport).State);
        }

        //[TestMethod]
        //public void DeepLinkStrategy_IsTransportEnabled() {
        //    CallbackData callback = new CallbackData() {
        //        Subscribed = true,
        //        Context = "123",
        //        Event = CallbackEvent.ConversationStarted
        //    };
        //    ViberConversationStartedAdapter message = new ViberConversationStartedAdapter(callback);
        //    Assert.IsTrue(strategy.IsTransportEnabled(entities, message));

        //    callback.Subscribed = false;
        //    Assert.IsFalse(strategy.IsTransportEnabled(entities, message));

        //    callback.Subscribed = null;
        //    Assert.ThrowsException<NullReferenceException>(() => strategy.IsTransportEnabled(entities, message));
        //}
    }
}
