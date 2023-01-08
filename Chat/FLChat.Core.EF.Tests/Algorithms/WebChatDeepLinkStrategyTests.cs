using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.Core.Algorithms.WebChat;

namespace FLChat.Core.Algorithms.Tests
{
    [TestClass]
    public class WebChatDeepLinkStrategyTests
    {
        private ChatEntities entities;
        private WebChatDeepLinkStrategy strategy = new WebChatDeepLinkStrategy();
        private WebChatCodeGenerator webChatGen = new WebChatCodeGenerator();

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Cleanup() {
            entities.Dispose();
        }


        [TestMethod]
        public void WebChatDeepLinkStrategy_NewUser() {
            //user who will send message from outer transport
            User user = entities.GetUserQ(
                q => q.Where(u => u.Transports.Where(t => t.TransportTypeId == (int)TransportKind.Test).Any() == false),
                null);
            //check user from has't transport of that type
            Assert.IsNull(entities.Transport.Where(t => t.UserId == user.Id && t.TransportTypeId == (int)TransportKind.Test).FirstOrDefault());

            //user who make deep-link invite
            User inv = entities.GetUserQ(null, null, transport: TransportKind.FLChat);
            //invited message
            Message wcmsg = new Message() {
                Kind = MessageKind.Personal,
                FromTransport = inv.Transports.Get(TransportKind.FLChat),
                Text = "invite",
                ToUsers = new MessageToUser[] {
                    new MessageToUser() {
                        //ToTransport = user.Transports.Get(TransportKind.WebChat)
                        ToUserId = user.Id,
                        ToTransportKind = TransportKind.WebChat
                    }
                }
            };
            entities.Message.Add(wcmsg);
            entities.SaveChanges();
            webChatGen.Gen(wcmsg.ToUser);

            //seek deep-link
            WebChatDeepLink wcdl = entities.WebChatDeepLink.Where(wc => wc.MsgId == wcmsg.Id).Single();

            //make message
            FakeDeepLinkData msg = new FakeDeepLinkData() {
                FromId = user.Id.ToString(),
                DeepLink = wcdl.Link,
                IsTransportEnabled = true
            };

            bool result = strategy.AcceptDeepLink(entities, msg, 
                out User acceptedUser, out Message answerTo, out object context, out IDeepLinkStrategy sender);

            Assert.IsTrue(result);

            Assert.IsNotNull(acceptedUser);
            Assert.AreEqual(user.Id, acceptedUser.Id);

            Assert.IsNotNull(answerTo);
            Assert.AreEqual(wcmsg.Id, answerTo.Id);

            Assert.IsNotNull(context);
            Assert.IsInstanceOfType(context, typeof(WebChatDeepLinkStrategy.Context));
            WebChatDeepLinkStrategy.Context wccontext = context as WebChatDeepLinkStrategy.Context;
            Assert.AreEqual(wcmsg.Id, wccontext.MessageId);
            Assert.AreEqual(wcdl.Id, wccontext.WebChat.Id);
            Assert.AreEqual(wcdl.Id, wccontext.WebChatId);

            Assert.AreSame(strategy, sender);            
        }

        [TestMethod]
        public void WebChatDeepLinkStrategy_AcceptedEarly() {
            //user who will send message from outer transport
            User user = entities.GetUserQ(transport: TransportKind.Test);

            //user who make deep-link invite
            User inv = entities.GetUserQ(transport: TransportKind.FLChat);

            //invited message
            Message wcmsg = new Message() {
                Kind = MessageKind.Personal,
                FromTransport = inv.Transports.Get(TransportKind.FLChat),
                Text = "invite",
                ToUsers = new MessageToUser[] {
                    new MessageToUser() {
                        ToUserId = user.Id,
                        ToTransportKind = TransportKind.WebChat                        
                    }
                }
            };
            entities.Message.Add(wcmsg);
            entities.SaveChanges();
            webChatGen.Gen(wcmsg.ToUser);

            //seek deep-link
            WebChatDeepLink wcdl = entities.WebChatDeepLink.Where(wc => wc.MsgId == wcmsg.Id).Single();

            wcdl.AcceptedTransportType.Add(entities.TransportType.Where(tt => tt.Id == (int)TransportKind.Test).Single());
            entities.SaveChanges();

            //make message
            FakeDeepLinkData msg = new FakeDeepLinkData() {
                FromId = user.Transports.Get(TransportKind.Test).TransportOuterId,
                DeepLink = wcdl.Link,
                IsTransportEnabled = true
            };

            bool result = strategy.AcceptDeepLink(entities, msg,
                out User acceptedUser, out Message answerTo, out object context, out IDeepLinkStrategy sender);

            Assert.IsTrue(result);

            Assert.IsNotNull(acceptedUser);
            Assert.AreEqual(user.Id, acceptedUser.Id);

            Assert.IsNotNull(answerTo);
            Assert.AreEqual(wcmsg.Id, answerTo.Id);

            Assert.IsNotNull(context);
            Assert.IsInstanceOfType(context, typeof(WebChatDeepLinkStrategy.Context));
            WebChatDeepLinkStrategy.Context wccontext = context as WebChatDeepLinkStrategy.Context;
            Assert.AreEqual(wcmsg.Id, wccontext.MessageId);
            Assert.AreEqual(wcdl.Id, wccontext.WebChat.Id);
            Assert.AreEqual(wcdl.Id, wccontext.WebChatId);

            Assert.AreSame(strategy, sender);
        }

        [TestMethod]
        public void WebChatDeepLinkStrategy_AcceptedEarlyByOtherUser() {
            //user who will send message from outer transport
            User user = entities.GetUserQ(transport: TransportKind.Test);

            //user who make deep-link invite
            User inv = entities.GetUserQ(transport: TransportKind.FLChat);

            //invited message
            Message wcmsg = new Message() {
                Kind = MessageKind.Personal,
                FromTransport = inv.Transports.Get(TransportKind.FLChat),
                Text = "invite",
                ToUsers = new MessageToUser[] {
                    new MessageToUser() {
                        ToUserId = user.Id,
                        ToTransportKind = TransportKind.WebChat
                    }
                }
            };
            entities.Message.Add(wcmsg);
            entities.SaveChanges();
            webChatGen.Gen(wcmsg.ToUser);

            //seek deep-link
            WebChatDeepLink wcdl = entities.WebChatDeepLink.Where(wc => wc.MsgId == wcmsg.Id).Single();

            wcdl.AcceptedTransportType.Add(entities.TransportType.Where(tt => tt.Id == (int)TransportKind.Test).Single());
            entities.SaveChanges();

            //make message
            FakeDeepLinkData msg = new FakeDeepLinkData() {
                FromId = Guid.NewGuid().ToString(), //another user id
                DeepLink = wcdl.Link,
                IsTransportEnabled = true
            };

            bool result = strategy.AcceptDeepLink(entities, msg,
                out User acceptedUser, out Message answerTo, out object context, out IDeepLinkStrategy sender);

            Assert.IsTrue(result);

            Assert.IsNull(acceptedUser);            

            Assert.IsNotNull(context);
            Assert.IsInstanceOfType(context, typeof(WebChatDeepLinkStrategy.Context));
            WebChatDeepLinkStrategy.Context wccontext = context as WebChatDeepLinkStrategy.Context;
            Assert.AreEqual(wcmsg.Id, wccontext.MessageId);
            Assert.AreEqual(wcdl.Id, wccontext.WebChat.Id);
            Assert.AreEqual(wcdl.Id, wccontext.WebChatId);

            Assert.AreSame(strategy, sender);
        }
    }
}
