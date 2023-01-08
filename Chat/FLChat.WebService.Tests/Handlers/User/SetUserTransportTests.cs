using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.WebService.Handlers.User.Tests
{
    [TestClass]
    public class SetUserTransportTests
    {
        [TestMethod]
        public void SetUserTransport_Key() {
            Assert.AreEqual("id", SetUserTransport.Key);
        }
    }
}
