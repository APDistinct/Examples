using System;
using System.Linq;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Core.Algorithms.WebChat.Tests
{
    [TestClass]
    public class WebChatCodeGeneratorTests
    {
        ChatEntities entities;

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Cleanup() {
            entities.Dispose();
        }

        [TestMethod]
        public void WebChatCodeGenerator() {
            WebChatCodeGenerator gen = new WebChatCodeGenerator() {
                ExpireInDays = TimeSpan.FromDays(5)
            };

            User from = entities.GetUserQ(transport: DAL.TransportKind.FLChat);
            User to = entities.GetUserQ(transport: DAL.TransportKind.WebChat, notSameToUser: from.Id);
            Message msg = entities.SendMessage(from.Id, to.Id, 
                fromt: DAL.TransportKind.FLChat, tot: DAL.TransportKind.WebChat, autoGenWebChatCode: false);
            Assert.IsNull(entities.WebChatDeepLink.Where(wc => wc.MsgId == msg.Id).FirstOrDefault());

            gen.Gen(msg.ToUser);

            WebChatDeepLink dl = entities.WebChatDeepLink.Where(wc => wc.MsgId == msg.Id).Single();
            Assert.AreEqual(to.Id, dl.ToUserId);
            Assert.AreEqual((int)DAL.TransportKind.WebChat, dl.ToTransportTypeId);
            Assert.IsTrue(Math.Abs((DateTime.UtcNow + TimeSpan.FromDays(5) - dl.ExpireDate).TotalSeconds) < 5);
            Assert.IsNotNull(dl.Link);
            Assert.AreEqual(gen.CodeLength, dl.Link.Length);
        }
    }
}
