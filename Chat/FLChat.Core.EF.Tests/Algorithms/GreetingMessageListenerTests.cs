using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL.Model;
using System.Linq;
using FLChat.DAL;
using FLChat.Core.Texts;

namespace FLChat.Core.Algorithms.Tests
{
    [TestClass]
    public class GreetingMessageListenerTests
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

        

        private WebChatDeepLink CreateDeepLink() {
            //user who will send message from outer transport
            User from = entities.GetUser(
                u => u.Enabled && u.Transports.Where(t => t.TransportTypeId == (int)TransportKind.Test).Any() == false,
                null);
            //check user from has't transport of that type
            Assert.IsNull(entities.Transport.Where(t => t.UserId == from.Id && t.TransportTypeId == (int)TransportKind.Test).FirstOrDefault());

            //user who make deep-link invite
            User inv = entities.GetUser(u => u.Id != from.Id && u.Enabled, null, TransportKind.FLChat);
            //invited message
            Message wcmsg = entities.SendMessage(inv.Id, from.Id, TransportKind.FLChat, TransportKind.WebChat);

            //seek deep-link
            return entities.WebChatDeepLink.Where(wc => wc.MsgId == wcmsg.Id).Single();
        }

        [TestMethod]
        public void GreetingMessageListener_AcceptedDL() {
            WebChatDeepLink deepLink = CreateDeepLink();
            Transport transport = new Transport() {
                Kind = TransportKind.Test,
                TransportOuterId = Guid.NewGuid().ToString()
            };
            deepLink.MessageToUser.ToTransport.User.Transports.Add(transport);
            entities.SaveChanges();

            //messages before call
            entities.Entry(deepLink.MessageToUser.Message).Collection(c => c.ToUsers).Load();
            Assert.AreEqual(1, deepLink.MessageToUser.Message.ToUsers.Count());

            GreetingMessageListener listener = new GreetingMessageListener();
            listener.DeepLinkAccepted(entities, new FakeOuterMessage() { TransportKind = TransportKind.Test },
                new DeepLinkResult(DeepLinkResultStatus.Accepted, 
                    new WebChatDeepLinkStrategy.Context(deepLink)),
                transport);

            //added new MessageToUser
            entities.Entry(deepLink.MessageToUser.Message).Collection(c => c.ToUsers).Load();
            Assert.AreEqual(2, deepLink.MessageToUser.Message.ToUsers.Count());
            Assert.AreEqual(deepLink.ToUserId, deepLink.MessageToUser.Message.ToUsers.Select(m => m.ToUserId).Distinct().Single());
            CollectionAssert.AreEquivalent(
                new TransportKind[] { TransportKind.WebChat, TransportKind.Test },
                deepLink
                    .MessageToUser
                    .Message
                    .ToUsers
                    .Select(m => m.ToTransportTypeId)
                    .ToArray()
                    .Select(tk => (TransportKind)tk)
                    .ToArray()
                );

            Assert.IsTrue(deepLink
                .MessageToUser
                .Message
                .ToUsers
                .Where(mtu => mtu.ToTransportTypeId == (int)TransportKind.Test)
                .Single().
                IsWebChatGreeting);
            Assert.IsFalse(deepLink
                .MessageToUser
                .Message
                .ToUsers
                .Where(mtu => mtu.ToTransportTypeId == (int)TransportKind.WebChat)
                .Single().
                IsWebChatGreeting);
        }

        [TestMethod]
        public void GreetingMessageListener_RejectedDL() {
            WebChatDeepLink deepLink = CreateDeepLink();
            User user = entities.GetUser(null, null, tk: TransportKind.Test);

            //deep link messages before call
            entities.Entry(deepLink.MessageToUser.Message).Collection(c => c.ToUsers).Load();
            Assert.AreEqual(1, deepLink.MessageToUser.Message.ToUsers.Count());

            int cnt = entities.MessageToUser.Where(mtu => mtu.ToUserId == user.Id).Count();

            GreetingMessageListener listener = new GreetingMessageListener(new FakeGreetingMessages());
            listener.DeepLinkAccepted(entities, new FakeOuterMessage() { TransportKind = TransportKind.Test },
                new DeepLinkResult(DeepLinkResultStatus.Rejected, 
                    new WebChatDeepLinkStrategy.Context(deepLink)),
                user.Transports.Get(TransportKind.Test));

            //deep link not created message (still has single message)
            entities.Entry(deepLink.MessageToUser.Message).Collection(c => c.ToUsers).Load();
            Assert.AreEqual(1, deepLink.MessageToUser.Message.ToUsers.Count());

            //messages to user after call: 1 new messages
            int cnt2 = entities.MessageToUser.Where(mtu => mtu.ToUserId == user.Id).Count();
            Assert.AreEqual(cnt + 1, cnt2);

            //get new messages
            MessageToUser []msgs = entities
                .MessageToUser
                .Where(mtu => mtu.ToUserId == user.Id)
                .OrderByDescending(mtu => mtu.Idx)
                .Take(1)
                .ToArray();

            Assert.AreEqual(TransportKind.Test, msgs.Select(m => m.ToTransportKind).Distinct().Single());
            Assert.AreEqual(Global.SystemBotId, msgs.Select(m => m.Message.FromUserId).Distinct().Single());
            Assert.AreEqual(TransportKind.FLChat, msgs.Select(m => m.Message.FromTransportKind).Distinct().Single());
        }

        [TestMethod]
        public void GreetingMessageListener_UnknownDL() {
            User user = entities.GetUser(null, null, tk: TransportKind.Test);

            long cnt = entities.MessageToUser.Where(mtu => mtu.ToUserId == user.Id).LongCount();

            GreetingMessageListener listener = new GreetingMessageListener(new FakeGreetingMessages());
            listener.DeepLinkAccepted(entities, new FakeOuterMessage() { TransportKind = TransportKind.Test },
                new DeepLinkResult(DeepLinkResultStatus.Unknown),
                user.Transports.Get(TransportKind.Test));

            //messages to user after call: 1 new messages
            long cnt2 = entities.MessageToUser.Where(mtu => mtu.ToUserId == user.Id).LongCount();
            Assert.AreEqual(cnt + 1, cnt2);

            //get new messages
            MessageToUser[] msgs = entities
                .MessageToUser
                .Where(mtu => mtu.ToUserId == user.Id)
                .OrderByDescending(mtu => mtu.Idx)
                .Take(1)
                .ToArray();

            Assert.AreEqual(TransportKind.Test, msgs.Select(m => m.ToTransportKind).Distinct().Single());
            Assert.AreEqual(Global.SystemBotId, msgs.Select(m => m.Message.FromUserId).Distinct().Single());
            Assert.AreEqual(TransportKind.FLChat, msgs.Select(m => m.Message.FromTransportKind).Distinct().Single());
        }

        [TestMethod]
        public void GreetingMessageListener_NewUser() {
            User user = entities.GetUser(null, null, tk: TransportKind.Test);

            long cnt = entities.MessageToUser.Where(mtu => mtu.ToUserId == user.Id).LongCount();

            GreetingMessageListener listener = new GreetingMessageListener();
            listener.NewUserCreated(entities, new FakeOuterMessage() { TransportKind = TransportKind.Test },                
                user.Transports.Get(TransportKind.Test));

            //messages to user after call: 1 new messages
            long cnt2 = entities.MessageToUser.Where(mtu => mtu.ToUserId == user.Id).LongCount();
            Assert.AreEqual(cnt + 1, cnt2);

            //get new messages
            MessageToUser[] msgs = entities
                .MessageToUser
                .Where(mtu => mtu.ToUserId == user.Id)
                .OrderByDescending(mtu => mtu.Idx)
                .Take(1)
                .ToArray();

            Assert.AreEqual(TransportKind.Test, msgs.Select(m => m.ToTransportKind).Distinct().Single());
            Assert.AreEqual(Global.SystemBotId, msgs.Select(m => m.Message.FromUserId).Distinct().Single());
            Assert.AreEqual(TransportKind.FLChat, msgs.Select(m => m.Message.FromTransportKind).Distinct().Single());
        }

        [TestMethod]
        public void GreetingMessageListener_LiteLink_AcceptedRouted() {
            User user = entities.GetUserQ(where: q => q.Where(u => u.FullName != null),
                transport: TransportKind.Test);
            User userRoute = entities.GetUserQ(where: q=> q.Where(u => u.FullName != null), 
                transport: TransportKind.FLChat, notSameToUser: user.Id);
            long cnt = entities.MessageToUser.Where(mtu => mtu.ToUserId == user.Id).LongCount();

            DeepLinkResult dlResult = new DeepLinkResult(DeepLinkResultStatus.Accepted,
                new LiteDeepLinkStrategy.Context(LiteDeepLinkStrategy.Context.LinkType.LinkByNumber, user) {
                    RouteTo = userRoute
                });

            GreetingMessageListener listener = new GreetingMessageListener(new FakeGreetingMessages() {
                LiteLinkRouted = "To %addressee%",
                LiteLinkKnownUser = "Hello #ФИО"
            });
            listener.DeepLinkAccepted(entities, null, dlResult, user.Transports.Get(TransportKind.Test));

            //messages to user after call: 2 new messages
            long cnt2 = entities.MessageToUser.Where(mtu => mtu.ToUserId == user.Id).LongCount();
            Assert.AreEqual(cnt + 2, cnt2);

            //get new messages
            MessageToUser[] msgs = entities
                .MessageToUser
                .Where(mtu => mtu.ToUserId == user.Id)
                .OrderByDescending(mtu => mtu.Idx)
                .Take(2)
                .ToArray();

            Assert.AreEqual(TransportKind.Test, msgs.Select(m => m.ToTransportKind).Distinct().Single());
            Assert.AreEqual(Global.SystemBotId, msgs.Select(m => m.Message.FromUserId).Distinct().Single());
            Assert.AreEqual(TransportKind.FLChat, msgs.Select(m => m.Message.FromTransportKind).Distinct().Single());
            Assert.IsTrue(msgs.Select(m => m.Message.NeedToChangeText).Distinct().Single());

            Assert.AreEqual("To " + userRoute.FullName, msgs[0].Message.Text);
            Assert.AreEqual("Hello #ФИО", msgs[1].Message.Text);
        }

        [TestMethod]
        public void GreetingMessageListener_LiteLink_AcceptedUnrouted() {
            User user = entities.GetUserQ(where: q => q.Where(u => u.FullName != null),
                transport: TransportKind.Test);
            long cnt = entities.MessageToUser.Where(mtu => mtu.ToUserId == user.Id).LongCount();

            DeepLinkResult dlResult = new DeepLinkResult(DeepLinkResultStatus.Accepted,
                new LiteDeepLinkStrategy.Context(LiteDeepLinkStrategy.Context.LinkType.LinkByNumber, user));

            FakeGreetingMessages fakeGreetingMessages = new FakeGreetingMessages();
            GreetingMessageListener listener = new GreetingMessageListener(fakeGreetingMessages);
            listener.DeepLinkAccepted(entities, null, dlResult, user.Transports.Get(TransportKind.Test));

            //messages to user after call: 2 new messages
            long cnt2 = entities.MessageToUser.Where(mtu => mtu.ToUserId == user.Id).LongCount();
            Assert.AreEqual(cnt + 2, cnt2);

            //get new messages
            MessageToUser[] msgs = entities
                .MessageToUser
                .Where(mtu => mtu.ToUserId == user.Id)
                .OrderByDescending(mtu => mtu.Idx)
                .Take(2)
                .ToArray();

            Assert.AreEqual(TransportKind.Test, msgs.Select(m => m.ToTransportKind).Distinct().Single());
            Assert.AreEqual(Global.SystemBotId, msgs.Select(m => m.Message.FromUserId).Distinct().Single());
            Assert.AreEqual(TransportKind.FLChat, msgs.Select(m => m.Message.FromTransportKind).Distinct().Single());
            Assert.IsTrue(msgs.Select(m => m.Message.NeedToChangeText).Distinct().Single());

            Assert.AreEqual(fakeGreetingMessages.LiteLinkUnrouted, msgs[0].Message.Text);
            Assert.AreEqual(fakeGreetingMessages.LiteLinkKnownUser, msgs[1].Message.Text);
        }
    }
}
