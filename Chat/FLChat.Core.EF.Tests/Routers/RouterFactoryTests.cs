using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Core.Routers.Tests
{
    [TestClass]
    public class RouterFactoryTests
    {
        [TestMethod]
        public void RouterFactory_CreateWithOne() {
            ActionRouter r1 = new ActionRouter(Guid.NewGuid());
            ChainRouter router = RouterFactory.CreateDefaultRouters(r1);
            Assert.AreSame(r1, router.First());
            Assert.AreEqual(r1.Addressee, router.RouteMessage(null, null, null));
        }

        [TestMethod]
        public void RouterFactory_CreateWithMany() {
            ActionRouter r1 = new ActionRouter(Guid.NewGuid());
            ActionRouter r2 = new ActionRouter(Guid.NewGuid());
            ChainRouter router = RouterFactory.CreateDefaultRouters(new IMessageRouter[] { r1, r2 });
            CollectionAssert.AreEquivalent(new IMessageRouter[] { r1, r2 }, router.Take(2).ToArray());
            Assert.AreEqual(r1.Addressee, router.RouteMessage(null, null, null));
        }

    }
}
