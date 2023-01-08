using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FLChat.DAL.Model;

namespace FLChat.Core.Tests
{
    [TestClass]
    public class TransportIdSaverTests
    {
        ChatEntities entities;
        TransportIdSaver saver = new TransportIdSaver();

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Cleanup() {
            entities.Dispose();
        }

        [TestMethod]
        public void TransportIdSaver_SaveTo() {
            User from = entities.GetUser(u => true, null, DAL.TransportKind.FLChat);
            User to = entities.GetUser(u => u.Id != from.Id, null, DAL.TransportKind.Test);
            Message msg = new Message() {
                FromUserId = from.Id,
                FromTransportTypeId = from.Transports.Get(DAL.TransportKind.FLChat).TransportTypeId,
                Kind = DAL.MessageKind.Personal,
                Text = "Test",
                ToUsers = new MessageToUser[] {
                    new MessageToUser() {
                        ToUserId = to.Id,
                        ToTransportTypeId = to.Transports.Get(DAL.TransportKind.Test).TransportTypeId,
                    }
                }
            };
            String id = Guid.NewGuid().ToString();
            saver.SaveTo(entities, id, msg.ToUsers.Single());
            entities.Message.Add(msg);
            entities.SaveChanges();

            MessageTransportId tid = entities.MessageTransportId.Where(tm => tm.MsgId == msg.Id).SingleOrDefault();
            Assert.IsNotNull(tid);
            entities.Entry(tid).Reload();
            Assert.AreEqual(id, tid.TransportId);
            Assert.AreEqual(msg.Id, tid.MsgId);
            Assert.AreEqual(to.Id, tid.ToUserId);
            Assert.AreEqual((int)DAL.TransportKind.Test, tid.TransportTypeId);

            Assert.AreEqual(0, tid.Index);
            Assert.AreEqual(1, tid.Count);
        }

        [TestMethod]
        public void TransportIdSaver_SaveFrom() {
            User from = entities.GetUser(u => true, null, DAL.TransportKind.Test);
            User to = entities.GetUser(u => u.Id != from.Id, null, DAL.TransportKind.FLChat);
            Message msg = new Message() {
                FromUserId = from.Id,
                FromTransportTypeId = from.Transports.Get(DAL.TransportKind.Test).TransportTypeId,
                Kind = DAL.MessageKind.Personal,
                Text = "Test",
                ToUsers = new MessageToUser[] {
                    new MessageToUser() {
                        ToUserId = to.Id,
                        ToTransportTypeId = to.Transports.Get(DAL.TransportKind.FLChat).TransportTypeId,
                        IsSent = true
                    }
                }
            };
            String id = Guid.NewGuid().ToString();
            saver.SaveFrom(entities, id, msg);
            entities.Message.Add(msg);
            entities.SaveChanges();

            MessageTransportId tid = entities.MessageTransportId.Where(tm => tm.MsgId == msg.Id).SingleOrDefault();
            Assert.IsNotNull(tid);
            entities.Entry(tid).Reload();
            Assert.AreEqual(id, tid.TransportId);
            Assert.AreEqual(msg.Id, tid.MsgId);
            Assert.IsNull(tid.ToUserId);
            Assert.AreEqual((int)DAL.TransportKind.Test, tid.TransportTypeId);

            Assert.AreEqual(0, tid.Index);
            Assert.AreEqual(1, tid.Count);
        }

        [TestMethod]
        public void TransportIdSaver_SaveToMany() {
            User from = entities.GetUser(u => true, null, DAL.TransportKind.FLChat);
            User to = entities.GetUser(u => u.Id != from.Id, null, DAL.TransportKind.Test);
            Message msg = entities.SendMessage(from.Id, to.Id, tot: DAL.TransportKind.Test);

            string[] ids = new string[] { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };
            saver.SaveTo(entities, ids, msg.ToUsers.Single());
            entities.SaveChanges();

            MessageTransportId[] tid = entities
                .MessageTransportId
                .Where(tm => tm.MsgId == msg.Id)
                .OrderBy(tm => tm.Index)
                .ToArray();

            Assert.AreEqual(ids.Length, tid.Length);
            CollectionAssert.AreEqual(ids, tid.Select(t => t.TransportId).ToArray());
            Assert.AreEqual(msg.Id, tid.Select(t => t.MsgId).Distinct().Single());
            Assert.AreEqual(to.Id, tid.Select(t => t.ToUserId).Distinct().Single());
            Assert.AreEqual((int)DAL.TransportKind.Test, tid.Select(t => t.TransportTypeId).Distinct().Single());

            CollectionAssert.AreEqual(new byte[] { 0, 1 }, tid.Select(t => t.Index).ToArray());
            Assert.AreEqual(2, tid.Select(t => t.Count).Distinct().Single());
        }

        [TestMethod]
        public void TransportIdSaver_SaveToMany_Failed() {
            User from = entities.GetUser(u => true, null, DAL.TransportKind.FLChat);
            User to = entities.GetUser(u => u.Id != from.Id, null, DAL.TransportKind.Test);
            Message msg = entities.SendMessage(from.Id, to.Id, tot: DAL.TransportKind.Test);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => 
                saver.SaveTo(entities, new string[] { }, msg.ToUsers.Single()));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => 
                saver.SaveTo(entities, Enumerable.Repeat("1", 256).ToArray(), msg.ToUsers.Single()));
        }
    }
}
