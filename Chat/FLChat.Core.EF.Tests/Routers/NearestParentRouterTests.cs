using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FLChat.DAL;
using FLChat.DAL.Model;

namespace FLChat.Core.Routers.Tests
{
    [TestClass]
    public class NearestParentRouterTests
    {
        ChatEntities entities;
        NearestParentRouter router = new NearestParentRouter();

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void NearestParentRouter_Owner() {
            //owner and owner's owner has FLChat
            User user = entities.GetUserQ(
                q => q.Where(u => u.Enabled && u.OwnerUserId != null
                    && u.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.Test).Any()
                    && u.OwnerUser.Enabled && u.OwnerUser.OwnerUserId != null
                    && u.OwnerUser.Transports.Where(t1 => t1.Enabled && t1.TransportTypeId == (int)TransportKind.FLChat).Any()
                    && u.OwnerUser.OwnerUser.Enabled
                    && u.OwnerUser.OwnerUser.Transports.Where(t2 => t2.Enabled && t2.TransportTypeId == (int)TransportKind.FLChat).Any()),
                u => {
                    u.Transports.Add(new Transport() { Enabled = true, Kind = TransportKind.Test, TransportOuterId = Guid.NewGuid().ToString() });
                    u.OwnerUser = new User();
                    u.OwnerUser.Transports.Add(new Transport() { Enabled = true, Kind = TransportKind.FLChat });
                    u.OwnerUser.OwnerUser = new User();
                    u.OwnerUser.OwnerUser.Transports.Add(new Transport() { Enabled = true, Kind = TransportKind.FLChat });
                }
                );
            Guid? value = router.RouteMessage(entities, new FakeOuterMessage() { }, new Message() { FromUserId = user.Id });
            Assert.AreEqual(user.OwnerUserId, value.Value);
        }

        [TestMethod]
        public void NearestParentRouter_OwnerOwner() {
            //owner has not flchat
            //owner's owner has flchat
            User user = entities.GetUserQ(
                q => q.Where(u => u.Enabled && u.OwnerUserId != null
                    && u.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.Test).Any()
                    && u.OwnerUser.Enabled && u.OwnerUser.OwnerUserId != null
                    && u.OwnerUser.Transports.Where(t1 => t1.Enabled && t1.TransportTypeId == (int)TransportKind.FLChat).Any() == false
                    && u.OwnerUser.OwnerUser.Enabled
                    && u.OwnerUser.OwnerUser.Transports.Where(t2 => t2.Enabled && t2.TransportTypeId == (int)TransportKind.FLChat).Any()),
                u => {
                    u.Transports.Add(new Transport() { Enabled = true, Kind = TransportKind.Test, TransportOuterId = Guid.NewGuid().ToString() });
                    u.OwnerUser = new User();
                    //u.OwnerUser.Transports.Add(new Transport() { Enabled = true, Kind = TransportKind.FLChat });
                    u.OwnerUser.OwnerUser = new User();
                    u.OwnerUser.OwnerUser.Transports.Add(new Transport() { Enabled = true, Kind = TransportKind.FLChat });
                }
                );
            Guid? value = router.RouteMessage(entities, new FakeOuterMessage() { }, new Message() { FromUserId = user.Id });
            Assert.AreEqual(user.OwnerUser.OwnerUserId, value.Value);
        }

        [TestMethod]
        public void NearestParentRouter_OwnerOwner2() {
            //owner has flchat but disabled
            //owner's owner has flchat
            User user = entities.GetUserQ(
                q => q.Where(u => u.Enabled && u.OwnerUserId != null
                    && u.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.Test).Any()
                    && u.OwnerUser.Enabled && u.OwnerUser.OwnerUserId != null
                    && u.OwnerUser.Transports.Where(t1 => t1.Enabled == false && t1.TransportTypeId == (int)TransportKind.FLChat).Any()
                    && u.OwnerUser.OwnerUser.Enabled
                    && u.OwnerUser.OwnerUser.Transports.Where(t2 => t2.Enabled && t2.TransportTypeId == (int)TransportKind.FLChat).Any()),
                u => {
                    u.Transports.Add(new Transport() { Enabled = true, Kind = TransportKind.Test, TransportOuterId = Guid.NewGuid().ToString() });
                    u.OwnerUser = new User() { Enabled = true };
                    u.OwnerUser.Transports.Add(new Transport() { Enabled = false, Kind = TransportKind.FLChat });
                    u.OwnerUser.OwnerUser = new User() { Enabled = true };
                    u.OwnerUser.OwnerUser.Transports.Add(new Transport() { Enabled = true, Kind = TransportKind.FLChat });
                }
                );
            Guid? value = router.RouteMessage(entities, new FakeOuterMessage() { }, new Message() { FromUserId = user.Id });
            Assert.AreEqual(user.OwnerUser.OwnerUserId, value.Value);
        }

        [TestMethod]
        public void NearestParentRouter_Nothing() {
            //owner has not flchat
            //owner's owner has not flchat
            User user = entities.GetUserQ(
                q => q.Where(u => u.Enabled && u.OwnerUserId != null
                    && u.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.Test).Any()
                    && u.OwnerUser.Enabled && u.OwnerUser.OwnerUserId != null
                    && u.OwnerUser.Transports.Where(t1 => t1.TransportTypeId == (int)TransportKind.FLChat).Any() == false
                    && u.OwnerUser.OwnerUser.Enabled
                    && u.OwnerUser.OwnerUser.Transports.Where(t2 => t2.TransportTypeId == (int)TransportKind.FLChat).Any() == false),
                u => {
                    u.Transports.Add(new Transport() { Enabled = true, Kind = TransportKind.Test, TransportOuterId = Guid.NewGuid().ToString() });
                    u.OwnerUser = new User();
                    //u.OwnerUser.Transports.Add(new Transport() { Enabled = false, Kind = TransportKind.FLChat });
                    u.OwnerUser.OwnerUser = new User();
                    //u.OwnerUser.OwnerUser.Transports.Add(new Transport() { Enabled = true, Kind = TransportKind.FLChat });
                }
                );
            Guid? value = router.RouteMessage(entities, new FakeOuterMessage() { }, new Message() { FromUserId = user.Id });
            Assert.IsFalse(value.HasValue);
        }
    }
}
