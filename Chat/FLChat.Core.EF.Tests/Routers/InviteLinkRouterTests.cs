using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.Core.Routers;

namespace FLChat.Core.Routers.Tests
{
    [TestClass]
    public class InviteLinkRouterTests
    {
        [TestMethod]
        public void MergeUsersTest()
        {
            InviteLinkRouter router = new InviteLinkRouter();
            TransportKind kind = TransportKind.Viber;
            string TransportOuterId = Guid.NewGuid().ToString();
            Transport fromTransp = new Transport() { TransportOuterId = TransportOuterId, Enabled = true, Kind = kind };

            User from = new User();
            from.Transports.Add(fromTransp);
            User to = new User();
            router.MergeUsers(from, to, (int)kind);
            Assert.IsTrue(from.Transports.Contains(fromTransp));
            Assert.IsFalse(fromTransp.Enabled);
            Assert.IsTrue(string.IsNullOrEmpty( fromTransp.TransportOuterId));
            DAL.Model.Transport toTransp = to.Transports.Get(kind);
            Assert.IsNotNull(toTransp);
            Assert.AreEqual(toTransp.Kind, fromTransp.Kind);
            Assert.AreEqual(toTransp.TransportOuterId, TransportOuterId);
            Assert.IsTrue(toTransp.Enabled);
            //, User to, int FromTransportTypeId
        }
    }
}
