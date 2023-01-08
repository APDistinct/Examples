using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL.Model;
using System.Collections.Generic;

using FLChat.WebService.DataTypes;

namespace FLChat.WebService.Handlers.Message.Tests
{
    [TestClass]
    public class MessageSentHistoryTests
    {
        ChatEntities entities;        

        DAL.Model.User sender;
        DAL.Model.User tot;
        DAL.Model.User tom;
        DAL.Model.Message []msgs;

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
            sender = entities.GetUserQ(transport: DAL.TransportKind.FLChat);
            tot = entities.GetUserQ(where: q => q.Where(u => u.Id != sender.Id), transport: DAL.TransportKind.Test);
            tom = entities.GetUserQ(where: q => q.Where(u => u.Id != sender.Id && u.Id != tot.Id), transport: DAL.TransportKind.Email);

            msgs = new DAL.Model.Message[3];
            //broadcast with file
            msgs[0] = entities.SendMessage(sender.Id, tot.Id, tot: DAL.TransportKind.Test, kind: DAL.MessageKind.Broadcast, fileMediaType: "image/jpeg");
            //broadcast without file
            msgs[1] = entities.SendMessage(sender.Id, tot.Id, tot: DAL.TransportKind.Test, kind: DAL.MessageKind.Broadcast);
            //mailing
            msgs[2] = entities.SendMessage(sender.Id, tom.Id, tot: DAL.TransportKind.Email, kind: DAL.MessageKind.Mailing);
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void MessageSentHistory_Get() {
            MessageSentHistory handler = new MessageSentHistory();
            var response = handler.ProcessRequest(entities, sender, null);
            Guid[] expected = msgs.Reverse().Select(m => m.Id).ToArray();
            CollectionAssert.AreEqual(
                expected,
                response.Messages.Take(3).Select(m => m.Id).ToArray());
            Assert.IsNotNull(response.LastMessageId);
            Assert.AreEqual(response.Messages.Last().Id, response.LastMessageId);
        }

        [TestMethod]
        public void MessageSentHistory_GetBroadcast() {
            MessageSentHistory handler = new MessageSentHistory();
            var response = handler.ProcessRequest(entities, sender, new MessageSentHistoryRequest() {
                Types = new DAL.MessageKind[] { DAL.MessageKind.Broadcast }
            });
            Guid[] expected = msgs.Reverse().Where(m => m.Kind == DAL.MessageKind.Broadcast).Select(m => m.Id).ToArray();
            CollectionAssert.AreEqual(
                expected,
                response.Messages.Take(expected.Length).Select(m => m.Id).ToArray());
        }

        [TestMethod]
        public void MessageSentHistory_GetMailing() {
            MessageSentHistory handler = new MessageSentHistory();
            var response = handler.ProcessRequest(entities, sender, new MessageSentHistoryRequest() {
                Types = new DAL.MessageKind[] { DAL.MessageKind.Mailing }
            });
            Guid[] expected = msgs.Reverse().Where(m => m.Kind == DAL.MessageKind.Mailing).Select(m => m.Id).ToArray();
            CollectionAssert.AreEqual(
                expected,
                response.Messages.Take(expected.Length).Select(m => m.Id).ToArray());
        }

        [TestMethod]
        public void MessageSentHistory_Partial() {
            MessageSentHistory handler = new MessageSentHistory();
            var response = handler.ProcessRequest(entities, sender, null);
            Assert.IsTrue(response.Count >= msgs.Length);
            Assert.AreEqual(handler.MaxCount, response.MaxCount);
            Assert.AreEqual(0, response.Offset);
            Assert.AreEqual(handler.MaxCount, response.RequestedCount);
            Assert.IsNotNull(response.TotalCount);
            Assert.IsTrue(response.TotalCount.Value >= response.Count);
            Assert.IsNull(response.StartedFrom);

            response = handler.ProcessRequest(entities, sender, new MessageSentHistoryRequest() {
                Offset = 2
            });
            Assert.IsTrue(response.Count >= msgs.Length - 2);
            Assert.AreEqual(handler.MaxCount, response.MaxCount);
            Assert.AreEqual(2, response.Offset);
            Assert.AreEqual(handler.MaxCount, response.RequestedCount);
            Assert.IsNull(response.TotalCount);
            Guid[] expected = msgs.Reverse().Select(m => m.Id).Skip(2).ToArray();
            CollectionAssert.AreEqual(
                expected,
                response.Messages.Take(expected.Length).Select(m => m.Id).ToArray());
            Assert.IsNull(response.StartedFrom);

            response = handler.ProcessRequest(entities, sender, new MessageSentHistoryRequest() {
                Count = 2
            });
            Assert.AreEqual(2, response.Count);
            Assert.AreEqual(handler.MaxCount, response.MaxCount);
            Assert.AreEqual(0, response.Offset);
            Assert.AreEqual(2, response.RequestedCount);
            Assert.IsNotNull(response.TotalCount);
            expected = msgs.Reverse().Select(m => m.Id).Take(2).ToArray();
            CollectionAssert.AreEqual(
                expected,
                response.Messages.Take(expected.Length).Select(m => m.Id).ToArray());
            Assert.IsNull(response.StartedFrom);
        }

        [TestMethod]
        public void MessageSentHistory_StartFrom() {
            MessageSentHistory handler = new MessageSentHistory();
            var response = handler.ProcessRequest(entities, sender, new MessageSentHistoryRequest() {
                StartFrom = msgs[2].Id
            });

            Assert.IsTrue(response.Count >= msgs.Length - 1);
            Assert.AreEqual(handler.MaxCount, response.MaxCount);
            Assert.AreEqual(0, response.Offset);
            Assert.AreEqual(handler.MaxCount, response.RequestedCount);
            Assert.IsNull(response.TotalCount);
            Guid[] expected = msgs.Reverse().Select(m => m.Id).Skip(1).ToArray();
            CollectionAssert.AreEqual(
                expected,
                response.Messages.Take(expected.Length).Select(m => m.Id).ToArray());
            Assert.AreEqual(msgs[2].Id, response.StartedFrom);
        }

        [TestMethod]
        public void MessageSentHistoryAll_Get() {
            MessageSentHistory handler = new MessageSentHistory();
            var response = handler.ProcessRequest(entities, tom, null);
            Guid[] expected = msgs.Reverse().Select(m => m.Id).ToArray();

            Assert.IsFalse(response.Messages.Take(3).Select(m => m.Id).ToArray().Intersect(expected).Any());

            handler.Mode = MessageSentHistory.ModeEnum.All;
            response = handler.ProcessRequest(entities, tom, null);
            CollectionAssert.AreEqual(
               expected,
               response.Messages.Take(3).Select(m => m.Id).ToArray());            
        }

        [TestMethod]
        public void MessageSentHistory_SelectedUser_Get() {
            MessageSentHistory handler = new MessageSentHistory();
            var response = handler.ProcessRequest(entities, tom, new MessageSentHistoryRequest() { Ids = sender.Id.ToString() });
            Guid[] expected = msgs.Reverse().Select(m => m.Id).ToArray();

            Assert.IsFalse(response.Messages.Take(3).Select(m => m.Id).ToArray().Intersect(expected).Any());

            handler.Mode = MessageSentHistory.ModeEnum.SelectedUser;
            response = handler.ProcessRequest(entities, tom, new MessageSentHistoryRequest() { Ids = sender.Id.ToString() });
            CollectionAssert.AreEqual(
               expected,
               response.Messages.Take(3).Select(m => m.Id).ToArray());

            response = handler.ProcessRequest(entities, tom, new MessageSentHistoryRequest() { Ids = tot.Id.ToString() });
            Assert.IsFalse(response.Messages.Take(3).Select(m => m.Id).ToArray().Intersect(expected).Any());
        }

        /// <summary>
        /// Test property IncludeUserInfo
        /// Is property is true, then field User will filled with sender information
        /// </summary>
        [TestMethod]
        public void IncludeUserInfo() {
            MessageSentHistory handler = new MessageSentHistory() { Mode = MessageSentHistory.ModeEnum.All };
            Assert.IsFalse(handler.IncludeUserInfo);

            var response = handler.ProcessRequest(entities, tom, null);
            Assert.IsTrue(response.Messages.Any());
            Assert.IsFalse(response.Messages.Where(i => i.User != null).Any());

            handler.IncludeUserInfo = true;
            response = handler.ProcessRequest(entities, tom, null);

            Assert.IsFalse(response.Messages.Where(i => i.User == null).Any());
            Assert.AreEqual(sender.Id, response.Messages.Take(3).Select(m => m.User.Id).ToArray().Distinct().Single());
        }
    }
}
