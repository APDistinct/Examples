using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using System.Net;

namespace FLChat.WebService.Handlers.Message.Tests
{
    [TestClass]
    public class MessageSentInfoTests
    {
        ChatEntities entities;

        DAL.Model.User sender;
        DAL.Model.User to;
        DAL.Model.Message[] msgs;

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
            sender = entities.GetUserQ(transport: DAL.TransportKind.FLChat);
            to = entities.GetUserQ(where: q => q.Where(u => u.Id != sender.Id), transport: DAL.TransportKind.Test);

            msgs = new DAL.Model.Message[2];
            //broadcast with file
            msgs[0] = entities.SendMessage(sender.Id, to.Id, tot: DAL.TransportKind.Test, kind: DAL.MessageKind.Broadcast, fileMediaType: "image/jpeg");
            //broadcast without file
            msgs[1] = entities.SendMessage(sender.Id, to.Id, tot: DAL.TransportKind.Test, kind: DAL.MessageKind.Broadcast);
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        /// <summary>
        /// Check main process, all success
        /// </summary>
        [TestMethod]
        public void MessageSentInfo_Perform() {
            MessageSentInfo handler = new MessageSentInfo();
            MessageSentInfoResponse resp = handler.ProcessRequest(entities, sender, msgs[0].Id.ToString());
            Assert.IsNotNull(resp);
            Assert.AreEqual(msgs[0].Id, resp.Id);
            Assert.AreEqual(msgs[0].Kind, resp.Kind);
            Assert.AreEqual(msgs[0].PostTm, resp.PostTm);
            Assert.AreEqual(msgs[0].Text, resp.Text);
            Assert.AreEqual(msgs[0].FileId, resp.File.FileId);
            Assert.IsNotNull(resp.Stats);
            Assert.IsNotNull(resp.Recipients);
            Assert.IsTrue(resp.Recipients.Any());
            Assert.IsTrue(resp.Recipients.Where(i => i.User == to.Id).Any());
        }

        /// <summary>
        /// test messages with file and without file
        /// </summary>
        [TestMethod]
        public void MessageSentInfo_Perform_File() {
            MessageSentInfo handler = new MessageSentInfo();

            //with file
            MessageSentInfoResponse resp = handler.ProcessRequest(entities, sender, msgs[0].Id.ToString());
            Assert.IsNotNull(resp.File);

            //without file
            resp = handler.ProcessRequest(entities, sender, msgs[1].Id.ToString());
            Assert.IsNull(resp.File);
        }

        /// <summary>
        /// Check access to message by another user and test property OnlySelfMessages
        /// </summary>
        [TestMethod]
        public void MessageSentInfo_AccessToMsg() {
            ErrorResponseException e;
            MessageSentInfo handler = new MessageSentInfo();
            //request by sender
            MessageSentInfoResponse resp = handler.ProcessRequest(entities, sender, msgs[0].Id.ToString());
            //request by another user
            e = Assert.ThrowsException<ErrorResponseException>(() 
                => handler.ProcessRequest(entities, to, msgs[0].Id.ToString()));
            Assert.AreEqual((int)HttpStatusCode.NotFound, e.GetHttpCode());

            handler.OnlySelfMessages = false;
            resp = handler.ProcessRequest(entities, to, msgs[0].Id.ToString());
            Assert.IsNotNull(resp);
        }

        /// <summary>
        /// Get stats for unknown message
        /// </summary>
        [TestMethod]
        public void MessageSentInfo_MessageNotFound() {
            MessageSentInfo handler = new MessageSentInfo();
            var e = Assert.ThrowsException<ErrorResponseException>(() 
                => handler.ProcessRequest(entities, sender, Guid.NewGuid().ToString()));
            Assert.AreEqual((int)HttpStatusCode.NotFound, e.GetHttpCode());
        }

        /// <summary>
        /// incorrect message id
        /// </summary>
        [TestMethod]
        public void MessageSentInfo_IncorrectMessageId() {
            MessageSentInfo handler = new MessageSentInfo();
            var e = Assert.ThrowsException<ErrorResponseException>(()
                => handler.ProcessRequest(entities, sender, "123"));
            Assert.AreEqual((int)HttpStatusCode.BadRequest, e.GetHttpCode());
        }
    }
}
