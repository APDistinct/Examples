using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.TelegramBot.Adapters;
using FLChat.DAL.Model;
using FLChat.DAL;

namespace FLChat.TelegramBot.Algorithms.Tests
{
    [TestClass]
    public class StartRouterTests
    {
        StartRouter router = new StartRouter();

        [TestMethod]
        public void StartRouter_DeepLinkMsg() {
            TelegramMessageAdapter msg = ResourceHelper.Read("message_deep_link");
            Guid? guid = router.RouteMessage(null, msg, null);
            Assert.IsNull(guid);
        }

        [TestMethod]
        public void StartRouter_TextMsg() {
            TelegramMessageAdapter msg = ResourceHelper.Read("message");
            Guid? guid = router.RouteMessage(null, msg, null);
            Assert.IsNull(guid);
        }

        [TestMethod]
        public void StartRouter_StartMsg_Temporary() {
            TelegramMessageAdapter msg = ResourceHelper.Read("message_start");
            var dbmsg = new Message() {
                FromTransport = new Transport() {
                    User = new User() {
                        IsTemporary = true
                    }
                }
            };
            Guid? guid = router.RouteMessage(null, msg, dbmsg);
            Assert.IsNull(guid);
        }

        [TestMethod]
        public void StartRouter_StartMsg() {
            TelegramMessageAdapter msg = ResourceHelper.Read("message_start");
            var dbmsg = new Message() {
                FromTransport = new Transport() {
                    User = new User() {
                        IsTemporary = false
                    }
                }
            };
            using (ChatEntities entities = new ChatEntities()) {
                Guid? guid = router.RouteMessage(entities, msg, dbmsg);
                Assert.IsNotNull(guid);
                Assert.AreEqual(Global.SystemBotId, guid.Value);

                var entity = entities.ChangeTracker.Entries<Message>().FirstOrDefault();
                var newMsg = entity.Entity;
                //entity.State = System.Data.Entity.EntityState.Detached;
                Assert.IsNotNull(newMsg);
                Assert.AreSame(entities.SystemBotTransport, newMsg.FromTransport);
                Assert.AreSame(dbmsg.FromTransport, newMsg.ToTransport);
            }
        }
    }
}
