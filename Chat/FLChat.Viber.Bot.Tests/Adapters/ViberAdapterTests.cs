using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL;
using FLChat.Viber.Client.Types;

namespace FLChat.Viber.Bot.Adapters.Tests
{
    [TestClass]
    public class ViberAdapterTests
    {
        [TestMethod]
        public void ViberAdapter_Test() {
            ViberAdapter msg = new ViberAdapter(new CallbackData());
            Assert.AreEqual(TransportKind.Viber, msg.TransportKind);
        }
    }
}
