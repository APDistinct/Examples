using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;

namespace FLChat.WebService.Handlers.Message.Tests
{
    [TestClass]
    public class ReadMessageNotifyTests
    {
        ChatEntities entities;
        DAL.Model.User from;
        DAL.Model.User to;

        ReadMessageNotify handler = new ReadMessageNotify();

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();

            from = entities.GetUser(
                u => u.Enabled && u.Transports.Where(t => t.TransportTypeId == 0 && t.Enabled == true).Any(),
                u => {
                    u.Enabled = true;
                    u.Transports.Add(new DAL.Model.Transport() {
                        Enabled = true,
                        TransportTypeId = 0
                    });
                });

            to = entities.GetUser(
                u => u.Enabled && u.Id != from.Id && u.Transports.Where(t => t.TransportTypeId == 0 && t.Enabled == true).Any(),
                u => {
                    u.Enabled = true;
                    u.Transports.Add(new DAL.Model.Transport() {
                        Enabled = true,
                        TransportTypeId = 0
                    });
                });
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void ReadMessageNotify_ManyOne() {
            DAL.Model.Message msg= entities.Message.Add(new DAL.Model.Message() {
                    Kind = MessageKind.Personal,
                    Text = "Test message ReadMessageNotify_ManyMsg one",
                    FromUserId = from.Id,
                    FromTransportKind = TransportKind.FLChat,
                    ToUsers = new MessageToUser[] {
                        new MessageToUser() { ToUserId = to.Id, ToTransportTypeId = 0, IsSent = true }
                    }
                });
            entities.SaveChanges();

            entities.Entry(msg.ToUsers.Single()).Reload();
            Assert.IsFalse(msg.ToUsers.Single().IsRead);

            handler.ProcessRequest(entities, to, new ReadMessageNotifyRequest() { Messages = new Guid[] { msg.Id } });

            entities.Entry(msg.ToUsers.Single()).Reload();
            entities.Entry(msg).Reload();
            Assert.IsTrue(msg.ToUsers.Single().IsRead);
            //check the other fields too
            Assert.IsFalse(msg.ToUsers.Single().IsDelivered);
            Assert.IsFalse(msg.ToUsers.Single().IsFailed);
            Assert.IsFalse(msg.IsDeleted);
        }

        [TestMethod]
        public void ReadMessageNotify_ManyMsg() {
            List<DAL.Model.Message> msgs = new List<DAL.Model.Message>();
            for (int i = 0; i < 3; ++i)
                msgs.Add(entities.Message.Add(
                new DAL.Model.Message() {
                    Kind = MessageKind.Personal,
                    Text = "Test message ReadMessageNotify_ManyMsg #" + i.ToString(),
                    FromUserId = from.Id,
                    FromTransportKind = TransportKind.FLChat,
                    ToUsers = new MessageToUser[] {
                        new MessageToUser() { ToUserId = to.Id, ToTransportTypeId = 0, IsSent = true }
                    }
                }));
            entities.SaveChanges();

            foreach (var m in msgs) {
                entities.Entry(m.ToUsers.Single()).Reload();
                Assert.IsFalse(m.ToUsers.Single().IsRead);
            }

            handler.ProcessRequest(entities, to, new ReadMessageNotifyRequest() { Messages = msgs.Select(m => m.Id).ToArray() });

            foreach (var m in msgs) {
                entities.Entry(m.ToUsers.Single()).Reload();
                Assert.IsTrue(m.ToUsers.Single().IsRead);
            }
        }
    }
}
