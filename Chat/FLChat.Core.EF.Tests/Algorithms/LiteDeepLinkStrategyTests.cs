using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FLChat.DAL.Model;
using FLChat.DAL;

namespace FLChat.Core.Algorithms.Tests
{
    [TestClass]
    public class LiteDeepLinkStrategyTests
    {
        ChatEntities entities;
        LiteDeepLinkStrategy strategy = new LiteDeepLinkStrategy();

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void LiteDeepLinkStrategy_FLChatUserNumber() {
            User user = entities.GetUserQ(
                q => q.Where(u => u.FLUserNumber != null),
                u => u.FLUserNumber = 999
            );
            string code = strategy.Generate(user);
            FakeDeepLinkData data = new FakeDeepLinkData() { DeepLink = code };
            bool result = strategy.AcceptDeepLink(entities, data, 
                out User retUser, out Message answer, out object context, out IDeepLinkStrategy sender);

            Assert.IsTrue(result);
            Assert.AreEqual(user.Id, retUser.Id);
            Assert.IsNull(answer);
            Assert.IsNotNull(context);
            Assert.IsInstanceOfType(context, typeof(LiteDeepLinkStrategy.Context));
            LiteDeepLinkStrategy.Context llc = (LiteDeepLinkStrategy.Context)context;
            Assert.AreEqual(LiteDeepLinkStrategy.Context.LinkType.LinkByNumber, llc.Link);
            Assert.AreEqual(user.Id, llc.User.Id);
            Assert.AreEqual(user.Id, llc.UserId.Value);
            Assert.AreEqual(user.FLUserNumber, llc.UserNumber.Value);
            Assert.AreSame(strategy, sender);
        }

        [TestMethod]
        public void LiteDeepLinkStrategy_UserId() {
            User user = entities.GetUserQ(
                q => q.Where(u => u.FLUserNumber == null),
                u => u.FLUserNumber = null
            );
            string code = strategy.Generate(user);
            FakeDeepLinkData data = new FakeDeepLinkData() { DeepLink = code };
            bool result = strategy.AcceptDeepLink(entities, data,
                out User retUser, out Message answer, out object context, out IDeepLinkStrategy sender);

            Assert.IsTrue(result);
            Assert.AreEqual(user.Id, retUser.Id);
            Assert.IsNull(answer);
            Assert.IsNotNull(context);
            Assert.IsInstanceOfType(context, typeof(LiteDeepLinkStrategy.Context));
            LiteDeepLinkStrategy.Context llc = (LiteDeepLinkStrategy.Context)context;
            Assert.AreEqual(LiteDeepLinkStrategy.Context.LinkType.LinkById, llc.Link);
            Assert.AreEqual(user.Id, llc.User.Id);
            Assert.AreEqual(user.Id, llc.UserId.Value);
            Assert.IsNull(llc.UserNumber);
            Assert.AreSame(strategy, sender);
        }

        [TestMethod]
        public void LiteDeepLinkStrategy_UnknownFLUserNumber() {
            int number = entities
                .User
                .Where(u => u.FLUserNumber.HasValue)
                .Select(u => u.FLUserNumber.Value)
                .Max() + 1;
            
            string code = strategy.Generate(new User() { FLUserNumber = number });
            FakeDeepLinkData data = new FakeDeepLinkData() { DeepLink = code };
            bool result = strategy.AcceptDeepLink(entities, data,
                out User retUser, out Message answer, out object context, out IDeepLinkStrategy sender);

            Assert.IsTrue(result);
            Assert.IsNull(retUser);
            Assert.IsNull(answer);
            Assert.IsInstanceOfType(context, typeof(LiteDeepLinkStrategy.Context));
            LiteDeepLinkStrategy.Context llc = (LiteDeepLinkStrategy.Context)context;
            Assert.AreEqual(number, llc.UserNumber.Value);
            Assert.IsNull(llc.UserId);
            Assert.IsNull(llc.User);
            Assert.AreSame(strategy, sender);
        }

        [TestMethod]
        public void LiteDeepLinkStrategy_UnknownId() {
            Guid guid = Guid.NewGuid();
            string code = strategy.Generate(new User() { Id = guid });
            FakeDeepLinkData data = new FakeDeepLinkData() { DeepLink = code };
            bool result = strategy.AcceptDeepLink(entities, data,
                out User retUser, out Message answer, out object context, out IDeepLinkStrategy sender);

            Assert.IsTrue(result);
            Assert.IsNull(retUser);
            Assert.IsNull(answer);
            Assert.IsInstanceOfType(context, typeof(LiteDeepLinkStrategy.Context));
            LiteDeepLinkStrategy.Context llc = (LiteDeepLinkStrategy.Context)context;
            Assert.AreEqual(guid, llc.UserId.Value);
            Assert.IsNull(llc.UserNumber);
            Assert.IsNull(llc.User);
            Assert.AreSame(strategy, sender);
        }

        [TestMethod]
        public void LiteDeepLinkStrategy_InvalidCode() {
            FakeDeepLinkData data = new FakeDeepLinkData() { DeepLink = "123456789" };
            bool result = strategy.AcceptDeepLink(entities, data,
                out User retUser, out Message answer, out object context, out IDeepLinkStrategy sender);

            Assert.IsFalse(result);
            Assert.IsNull(retUser);
            Assert.IsNull(answer);
            Assert.IsNull(context);
            Assert.IsNull(sender);
        }

        [TestMethod]
        public void LiteDeepLinkStrategy_AfterAddTransport() {
            DAL.Model.User user = entities.GetUserQ(
                where: q => q.Where(u => u.OwnerUserId != null && u.OwnerUser.OwnerUserId != null
                    && u.OwnerUser.OwnerUser.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any()
                    && u.OwnerUser.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any() == false),
                create: u => {
                    u.OwnerUser = entities.GetUserQ(q => q.Where(uo => uo.OwnerUserId == null
                        && uo.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any() == false));
                    u.OwnerUser.OwnerUser = entities.GetUserQ(q => q.Where(uo => uo.OwnerUserId == null), transport: TransportKind.FLChat);
                },
                transport: TransportKind.Test
                );
            user.Transports.Get(TransportKind.Test).ChangeAddressee(entities, null);
            Assert.IsNull(user.Transports.Get(TransportKind.Test).MsgAddressee);

            LiteDeepLinkStrategy.Context cntx = new LiteDeepLinkStrategy.Context(
                LiteDeepLinkStrategy.Context.LinkType.LinkByNumber, 
                user);
            Assert.IsNull(cntx.RouteTo);

            strategy.AfterAddTransport(entities, null, user.Transports.Get(TransportKind.Test), cntx);
            Assert.IsNotNull(cntx.RouteTo);
            Assert.AreEqual(user.OwnerUser.OwnerUserId, cntx.RouteTo.Id);

            User addr = user.Transports.Get(TransportKind.Test).MsgAddressee;
            Assert.IsNotNull(addr);
            Assert.AreEqual(user.OwnerUser.OwnerUserId, addr.Id);
        }

        [TestMethod]
        public void LiteDeepLinkStrategy_AfterAddTransport_HasnotParentFLChat() {
            DAL.Model.User user = entities.GetUserQ(
                where: q => q.Where(u => u.OwnerUserId != null && u.OwnerUser.OwnerUserId != null && u.OwnerUser.OwnerUser.OwnerUserId == null
                    && u.OwnerUser.OwnerUser.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any() == false
                    && u.OwnerUser.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any() == false),
                create: u => {
                    u.OwnerUser = entities.GetUserQ(q => q.Where(uo => uo.OwnerUserId == null
                        && uo.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any() == false));
                    u.OwnerUser.OwnerUser = entities.GetUserQ(q => q.Where(uo => uo.OwnerUserId == null 
                        && uo.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any() == false));
                },
                transport: TransportKind.Test
                );
            user.Transports.Get(TransportKind.Test).ChangeAddressee(entities, null);
            Assert.IsNull(user.Transports.Get(TransportKind.Test).MsgAddressee);

            LiteDeepLinkStrategy.Context cntx = new LiteDeepLinkStrategy.Context(
                LiteDeepLinkStrategy.Context.LinkType.LinkByNumber,
                user);
            Assert.IsNull(cntx.RouteTo);

            strategy.AfterAddTransport(entities, null, user.Transports.Get(TransportKind.Test), cntx);
            Assert.IsNull(cntx.RouteTo);
            Assert.IsNull(user.Transports.Get(TransportKind.Test).MsgAddressee);
        }
    }
}
