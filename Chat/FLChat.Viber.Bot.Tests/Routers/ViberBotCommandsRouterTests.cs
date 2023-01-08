using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.Core;
using FLChat.Core.Routers;
using FLChat.DAL.Model;

namespace FLChat.Viber.Bot.Routers.Tests
{
    [TestClass]
    public class ViberBotCommandsRouterTests
    {
        ViberBotCommandsRouter router = new ViberBotCommandsRouter();

        private Message CreateDbMsg() {
            return new Message() {
                FromTransport = new Transport() {
                    User = new User() { FullName = "name" }
                }
            };
        }

        [TestMethod]
        public void ViberBotCommandsRouter_CmdScore() {
            FakeOuterMessage msg = new FakeOuterMessage() {
                Text = BotCommandsRouter.CMD_SCORE
            };
            Message dbmsg = CreateDbMsg();
            using (ChatEntities entities = new ChatEntities()) {
                Assert.AreEqual(Guid.Empty, router.RouteMessage(entities, msg, dbmsg));     //routed to bot
                Assert.IsTrue(entities.ChangeTracker.Entries<Message>().Any());     //has new message
            }
        }

        [TestMethod]
        public void ViberBotCommandsRouter_CmdSelectAddressee() {
            FakeOuterMessage msg = new FakeOuterMessage() {
                Text = BotCommandsRouter.CMD_SELECT_ADDRESSEE
            };
            Message dbmsg = CreateDbMsg();
            using (ChatEntities entities = new ChatEntities()) {
                Assert.AreEqual(Guid.Empty, router.RouteMessage(entities, msg, dbmsg)); //routed to bot
                Assert.IsTrue(entities.ChangeTracker.Entries<Message>().Any());     //has new message
            }
        }

        [TestMethod]
        public void ViberBotCommandsRouter_CmdUrl() {
            FakeOuterMessage msg = new FakeOuterMessage() {
                Text = BotCommandsRouter.URL_PREFIX + "www.ya.ru"
            };
            Message dbmsg = CreateDbMsg();
            using (ChatEntities entities = new ChatEntities()) {
                Assert.AreEqual(Guid.Empty, router.RouteMessage(entities, msg, dbmsg)); //routed to bot
                Assert.IsFalse(entities.ChangeTracker.Entries<Message>().Any());    //does't create new messgae
            }
        }

        [TestMethod]
        public void ViberBotCommandsRouter_Text() {
            FakeOuterMessage msg = new FakeOuterMessage() {
                Text = "some text"
            };
            Message dbmsg = CreateDbMsg();
            using (ChatEntities entities = new ChatEntities()) {
                Assert.IsNull(router.RouteMessage(entities, msg, dbmsg));       //does't route
                Assert.IsFalse(entities.ChangeTracker.Entries<Message>().Any()); //does't create new messgae
            }
        }

    }
}
