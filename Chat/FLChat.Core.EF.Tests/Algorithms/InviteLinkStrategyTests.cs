using System;
using System.Linq;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Core.Algorithms.Tests
{
    [TestClass]
    public class InviteLinkStrategyTests
    {
        ChatEntities entities;
        InviteLinkStrategy strategy;

        [TestInitialize]
        public void Init()
        {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Clean()
        {
            entities.Dispose();
        }

        [TestMethod]
        public void InviteLinkStrategy_OK_notCreate()
        {
            strategy = new InviteLinkStrategy(false);
            User user = new User() { Id = Guid.NewGuid() };
            string code = strategy.Generate(user);

            FakeDeepLinkData data = new FakeDeepLinkData() { DeepLink = code };
            bool result = strategy.AcceptDeepLink(entities, data,
                out User retUser, out Message answer, out object context, out IDeepLinkStrategy sender);

            Assert.IsTrue(result);
            Assert.IsNull(retUser);
            Assert.IsNull(answer);
            Assert.IsNotNull(context);
            Assert.IsNotNull(sender);
        }

        [TestMethod]
        public void InviteLinkStrategy_OK_Create()
        {
            strategy = new InviteLinkStrategy();

            User user = entities.GetUserQ(
                q => q.Where(u => u.Enabled == true),
                u => u.Enabled = true
            );
            string code = strategy.Generate(user);
            FakeDeepLinkData data = new FakeDeepLinkData() { DeepLink = code };
            bool result = strategy.AcceptDeepLink(entities, data,
                out User retUser, out Message answer, out object context, out IDeepLinkStrategy sender);

            Assert.IsTrue(result);
            Assert.AreEqual(user.Id, retUser.OwnerUser.Id);
            Assert.IsNull(answer);
            Assert.IsNotNull(context);
            Assert.IsInstanceOfType(context, typeof(InviteLinkStrategy.Context));
            InviteLinkStrategy.Context ilsc = (InviteLinkStrategy.Context)context;
            Assert.AreEqual(InviteLinkStrategy.Context.LinkType.LinkById, ilsc.Link);
            Assert.AreEqual(user.Id, ilsc.User.OwnerUser.Id);
            Assert.AreSame(strategy, sender);
        }

        [TestMethod]
        public void InviteLinkStrategy_InvalidCode()
        {
            strategy = new InviteLinkStrategy(false);
            FakeDeepLinkData data = new FakeDeepLinkData() { DeepLink = "123456789" };
            bool result = strategy.AcceptDeepLink(entities, data,
                out User retUser, out Message answer, out object context, out IDeepLinkStrategy sender);

            Assert.IsFalse(result);
            Assert.IsNull(retUser);
            Assert.IsNull(answer);
            Assert.IsNull(context);
            Assert.IsNull(sender);
        }

    }
}
