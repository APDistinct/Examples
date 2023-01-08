using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using System.Net;

namespace FLChat.WebService.Handlers.WebChat.Tests
{
    [TestClass]
    public class ReadWebChatTests
    {
        ChatEntities entities;
        ReadWebChat handler;
        DAL.Model.User from;
        DAL.Model.User to;
        WebChatDeepLink wc;

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
            handler = new ReadWebChat();

            from = entities.GetUser(u => u.Enabled, null, TransportKind.FLChat);
            to = entities.GetUser(u => u.Enabled && u.Id != from.Id, null);

            var msg = entities.SendMessage(from.Id, to.Id, tot: TransportKind.WebChat);

            wc = entities.WebChatDeepLink.Where(i => i.MsgId == msg.Id).Single();
            //wc = MakeMsg();
        }

        //public WebChatDeepLink MakeMsg() {
        //    DAL.Model.Message msg = new DAL.Model.Message() {
        //        FromTransport = from.Transports.Get(TransportKind.FLChat),
        //        Text = "Test",
        //        ToUsers = new MessageToUser[] {
        //            new MessageToUser() {
        //                ToTransport = to.Transports.Get(TransportKind.WebChat)
        //            }
        //        }
        //    };
        //    entities.Message.Add(msg);
        //    entities.SaveChanges();
        //    entities.Entry(msg).Collection(m => m.ToUsers).Load();
        //    entities.Entry(msg.ToUsers.Single()).Collection(m => m.WebChatDeepLink).Load();            

        //    return msg.ToUsers.Single().WebChatDeepLink.Single();
        //}

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void ReadWebChat_Test() {
            MessageToUser msgToUser;
            msgToUser = entities.MessageToUser.Where(mtu => mtu.ToUserId == to.Id && mtu.MsgId == wc.MsgId).Single();
            Assert.IsFalse(msgToUser.IsRead);

            WebChatReadResponse resp = handler.ProcessRequest(entities, entities.SystemBot, wc.Link);

            Assert.IsNotNull(resp);
            Assert.IsNotNull(resp.Message);
            Assert.IsNotNull(resp.User);
            Assert.IsNotNull(resp.Sender);
            Assert.IsNotNull(resp.InviteButtons);

            Assert.AreEqual(wc.MsgId, resp.Message.Id);
            Assert.AreEqual(true, resp.Message.Incoming);            
            Assert.AreEqual(wc.ToUserId, resp.User.Id);
            Assert.AreEqual(wc.MessageToUser.Message.FromUserId, resp.Sender.Id);
            CollectionAssert.AreEqual(
                entities
                    .TransportType
                    .Where(tt => tt.Enabled && tt.DeepLink != null)
                    .ToArray()
                    .Select(tt => tt.Kind)
                    .OrderBy(tt => tt)
                    .ToArray(),
                resp.InviteButtons.Select(b => b.Transport).OrderBy(b => b).ToArray());
            foreach(var ib in resp.InviteButtons) {
                Assert.IsTrue(ib.Url.Contains(wc.Link));
            }

            msgToUser = entities.MessageToUser.Where(mtu => mtu.ToUserId == to.Id && mtu.MsgId == wc.MsgId).Single();
            Assert.IsTrue(msgToUser.IsRead);
        }

        /// <summary>
        /// Make request with User's token instead of System.Bot's token
        /// </summary>
        [TestMethod]
        public void ReadWebChat_UsersToken_Test() {
            try {
                WebChatReadResponse resp = handler.ProcessRequest(entities, to, wc.Link);
                Assert.Fail("Exception has't thrown");
            } catch(ErrorResponseException e) {
                Assert.AreEqual((int)HttpStatusCode.Unauthorized, e.GetHttpCode());
            }
        }

        /// <summary>
        /// Make request with unknown message code
        /// </summary>
        [TestMethod]
        public void ReadWebChat_InvalidCode_Test() {
            try {
                string code = "___";
                WebChatReadResponse resp = handler.ProcessRequest(entities, entities.SystemBot, code);
                Assert.Fail("Exception has't thrown");
            } catch (ErrorResponseException e) {
                Assert.AreEqual((int)HttpStatusCode.NotFound, e.GetHttpCode());
            }
        }

    }
}
