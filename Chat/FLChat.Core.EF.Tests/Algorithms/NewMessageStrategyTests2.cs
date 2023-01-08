using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL.Model;
using System.Linq;
using FLChat.DAL;

namespace FLChat.Core.Algorithms.Tests
{
    [TestClass]
    public class NewMessageStrategyTests2
    {
        private ChatEntities entities;        

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Cleanup() {
            entities.Dispose();
        }

        [TestMethod]
        public void NewMessageStrategy_DeepLink_Accepted() {
            //user who will send message from outer transport
            User user = entities.GetUserQ(
                q => q.Where(u => u.Transports.Where(t => t.TransportTypeId == (int)TransportKind.Test).Any() == false),
                null);
            //check user from has't transport of that type
            Assert.IsNull(entities.Transport.Where(t => t.UserId == user.Id && t.TransportTypeId == (int)TransportKind.Test).FirstOrDefault());

            //user who make deep-link invite
            User inv = entities.GetUser(u => u.Enabled, null, TransportKind.FLChat);

            ActionDeepLinkStrategy dlStrategy = new ActionDeepLinkStrategy() {
                Result = true,
                User = user,
            };

            FakeNewMessageListener listener = new FakeNewMessageListener();

            NewMessageStrategy strategy = new NewMessageStrategy(
                new ActionRouter(Guid.Empty),
                dlStrategy,
                listener: listener);

            //make message
            FakeOuterMessage msg = new FakeOuterMessage() {
                FromId = Guid.NewGuid().ToString(),                
                FromName = user.FullName,
                Text = "Message with deep link",
                DeepLink = "deeplink",
                IsTransportEnabled = true,                
            };

            strategy.Process(entities, msg, out DeepLinkResult dlResult);

            Assert.IsNotNull(listener.DeepLinkResult);
            Assert.AreSame(dlStrategy.Context, listener.DeepLinkResult.Context);
            Assert.AreEqual(DeepLinkResultStatus.Accepted, listener.DeepLinkResult.Status);
            Assert.AreSame(listener.DeepLinkResult, dlResult);

            entities.Entry(user).Collection(u => u.Transports).Load();
            Transport transport = entities.Transport.Where(t => t.UserId == user.Id && t.TransportTypeId == (int)TransportKind.Test).Single();
            Assert.AreEqual(msg.FromId, transport.TransportOuterId);
            Assert.IsTrue(transport.Enabled);

            Assert.IsNotNull(listener.DeepLinkTransport);
            Assert.AreEqual(user.Id, listener.DeepLinkTransport.UserId);
            Assert.AreEqual(TransportKind.Test, listener.DeepLinkTransport.Kind);
        }

        [TestMethod]
        public void NewMessageStrategy_DeepLink_AcceptedEarly() {
            //user who will send message from outer transport
            User user = entities.GetUserQ(transport: TransportKind.Test);

            //user who make deep-link invite
            User inv = entities.GetUser(u => u.Enabled, null, TransportKind.FLChat);

            ActionDeepLinkStrategy dlStrategy = new ActionDeepLinkStrategy() {
                Result = true,
                User = user,
            };
            Assert.AreEqual(0, dlStrategy.CountOfCalls);

            FakeNewMessageListener listener = new FakeNewMessageListener();            

            NewMessageStrategy strategy = new NewMessageStrategy(
                new ActionRouter(Guid.Empty),
                dlStrategy,
                listener: listener) { ProcessDeepLinkForActiveUser = true };

            //make message
            FakeOuterMessage msg = new FakeOuterMessage() {
                FromId = user.Transports.Get(TransportKind.Test).TransportOuterId,
                FromName = user.FullName,
                Text = "Message with deep link",
                DeepLink = "deeplink",
                IsTransportEnabled = true,
            };

            strategy.Process(entities, msg, out DeepLinkResult dlResult);

            Assert.AreEqual(1, dlStrategy.CountOfCalls);

            Assert.IsNotNull(listener.DeepLinkResult);
            Assert.AreSame(dlStrategy.Context, listener.DeepLinkResult.Context);
            Assert.AreEqual(DeepLinkResultStatus.AcceptedEarly, listener.DeepLinkResult.Status);
            Assert.AreSame(listener.DeepLinkResult, dlResult);
            
            Assert.IsNotNull(listener.DeepLinkTransport);
            Assert.AreEqual(user.Id, listener.DeepLinkTransport.UserId);
            Assert.AreEqual(TransportKind.Test, listener.DeepLinkTransport.Kind);
        }

    }
}
