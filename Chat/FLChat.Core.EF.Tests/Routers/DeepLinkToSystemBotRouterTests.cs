using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FLChat.DAL;

namespace FLChat.Core.Routers.Tests
{
    [TestClass]
    public class DeepLinkToSystemBotRouterTests
    {
        DeepLinkToSystemBotRouter router = new DeepLinkToSystemBotRouter();

        [TestMethod]
        public void DeepLinkToSystemBotRouter_DeepLink() {
            FakeOuterMessage msg = new FakeOuterMessage() {
                DeepLink = "link"
            };
            Assert.AreEqual(Global.SystemBotId, router.RouteMessage(null, msg, null));
        }

        [TestMethod]
        public void DeepLinkToSystemBotRouter_Common() {
            FakeOuterMessage msg = new FakeOuterMessage() {
                DeepLink = null
            };
            Assert.IsNull(router.RouteMessage(null, msg, null));
        }

    }
}
