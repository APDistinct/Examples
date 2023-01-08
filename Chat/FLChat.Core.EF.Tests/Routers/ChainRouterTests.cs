using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Core.Routers.Tests
{
    [TestClass]
    public class ChainRouterTests
    {
        [TestMethod]
        public void ChainRouter_Route() {
            ChainRouter r = new ChainRouter();

            //empty router
            Assert.IsNull(r.RouteMessage(null, null, null));

            //with one router
            Guid? reply1 = Guid.NewGuid();
            r.Add(new ActionRouter((e, om, m) => reply1));
            Assert.AreEqual(reply1, r.RouteMessage(null, null, null));

            //with two routers
            Guid? reply2 = Guid.NewGuid();
            Assert.AreNotEqual(reply1, reply2);
            r.Add(new ActionRouter((e, om, m) => reply2));

            //reply2 returns true
            Assert.AreEqual(reply2, r.RouteMessage(null, null, null));

            //reply1 returns true
            reply2 = null;
            Assert.AreEqual(reply1, r.RouteMessage(null, null, null));

            //reply1 and reply2 returns false
            reply1 = null;
            Assert.IsNull(r.RouteMessage(null, null, null));
        }

        [TestMethod]
        public void ChainRouter_ConstructorTest() {
            Guid? reply1 = Guid.NewGuid();
            Guid? reply2 = Guid.NewGuid();
            Assert.AreNotEqual(reply1, reply2);

            IMessageRouter[] routers = new IMessageRouter[] {
                new ActionRouter((e, om, m) => reply2),
                new ActionRouter((e, om, m) => reply1)
            };

            Guid? reply3 = Guid.NewGuid();
            IMessageRouter[] routers2 = new IMessageRouter[] {
                new ActionRouter((e, om, m) => reply3)
            };

            ChainRouter r = new ChainRouter(routers.Concat(routers2));

            //reply2 returns true
            Assert.AreEqual(reply2, r.RouteMessage(null, null, null));

            //reply1 returns true
            reply2 = null;
            Assert.AreEqual(reply1, r.RouteMessage(null, null, null));

            
            reply1 = null;
            Assert.AreEqual(reply3, r.RouteMessage(null, null, null));

            //reply1 and reply2 returns false
            reply3 = null;
            Assert.IsNull(r.RouteMessage(null, null, null));
        }

        [TestMethod]
        public void ChainRouter_EnumerateTest() {
            IMessageRouter[] routers = new IMessageRouter[] {
                new ActionRouter(Guid.Empty),
                new ActionRouter(Guid.Empty)
            };

            ChainRouter r = new ChainRouter(routers);
            CollectionAssert.AreEqual(routers, r.ToArray());
        }
    }
}
