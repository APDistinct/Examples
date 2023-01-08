using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL.Model;
using FLChat.DAL;
using System.Linq;
using FLChat.WebService.DataTypes;

namespace FLChat.WebService.Handlers.WebChat.Tests
{
    [TestClass]
    public class AnswerWebChatTests
    {
        ChatEntities entities;
        AnswerWebChat handler;
        DAL.Model.User from;
        DAL.Model.User to;
        WebChatDeepLink wc;

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
            handler = new AnswerWebChat();

            from = entities.GetUser(u => u.Enabled, null, TransportKind.FLChat);
            to = entities.GetUser(u => u.Enabled && u.Id != from.Id, null);

            var msg = entities.SendMessage(from.Id, to.Id, tot: TransportKind.WebChat);

            wc = entities.WebChatDeepLink.Where(i => i.MsgId == msg.Id).Single();
        }
        
        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void AnswerWebChat_Test() {
            MessageToUser msgToUser;
            msgToUser = entities.MessageToUser.Where(mtu => mtu.ToUserId == to.Id && mtu.MsgId == wc.MsgId).Single();
            Assert.IsFalse(msgToUser.IsRead);

            WebChatAnswerRequest request = new WebChatAnswerRequest() {
                Code = wc.Link,
                Text = "Answer to " + wc.Id.ToString()
            };
            object resp = handler.ProcessRequest(entities, entities.SystemBot, request);

            Assert.IsNull(resp);

            DAL.Model.Message msg = entities.Message.Where(m => m.AnswerToId == msgToUser.MsgId).SingleOrDefault();
            Assert.IsNotNull(msg);
            Assert.AreEqual(to.Id, msg.FromUserId);
            Assert.AreEqual(TransportKind.WebChat, msg.FromTransportKind);
            Assert.AreEqual(request.Text, msg.Text);

            MessageToUser msgto = msg.ToUsers.SingleOrDefault();
            Assert.IsNotNull(msgto);
            Assert.AreEqual(from.Id, msgto.ToUserId);
            Assert.AreEqual(TransportKind.FLChat, msgto.ToTransportKind);
        }
    }
}
