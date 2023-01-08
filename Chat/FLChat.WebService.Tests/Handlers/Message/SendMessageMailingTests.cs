using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using FLChat.WebService.Handlers.Message;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.WebService.Handlers.Message.Tests
{
    [TestClass]
    public class SendMessageMailingTests
    {
        ChatEntities entities;

        SendMessage handler;
        DAL.Model.User from;

        [TestInitialize]
        public void Init()
        {
            entities = new ChatEntities();

            handler = new SendMessage();

            from = entities.GetUser(
                u => u.Enabled && u.Transports.Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.FLChat).Any(),
                u => {
                    u.Enabled = true;
                    u.Transports.Add(new DAL.Model.Transport()
                    {
                        TransportTypeId = (int)TransportKind.FLChat,
                        Enabled = true
                    });
                });
        }

        [TestCleanup]
        public void Clean()
        {
            entities.Dispose();
        }

        /// <summary>
        /// Successfull message to segment and user
        /// </summary>
        [TestMethod]
        public void SendMessage_Mailing_User_Segm()
        {
            //using (var trans = entities.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted)) 
            {
                //  Users. users 0, 1 and 2 are message addressee, users 1 and 4 include to segment
                int num = 4;
                TransportKind?[] transports = new TransportKind?[num];
                transports[0] = null;
                for (int j = 1; j < num; ++j) {
                    transports[j] = TransportKind.Email;
                }
                DAL.Model.User[] uto = entities.GetUsers
                    (num,
                    u => u.Enabled
                        //&& u.Transports.Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.Email).Any()
                        && u.Id != from.Id && u.OwnerUserId == from.Id && u.Email != null,
                    u => {
                        u.Enabled = true;
                        u.OwnerUserId = from.Id;
                        u.Email = Guid.NewGuid() + "@ya.com";
                    //u.Transports.Add(new DAL.Model.Transport()
                    //{
                    //    Enabled = true,
                    //    Kind = TransportKind.Email
                    //});
                });

                // Segment
                Segment sto = entities.GetSegment(s => !s.IsDeleted && s.Members.Contains(uto[1]) && s.Members.Contains(uto[num - 1]),
                    s => { s.IsDeleted = false; s.Members.Add(uto[1]); s.Members.Add(uto[num - 1]); });
                //  Пользователь и в сегменте - рассылка в два направления            

                List<string> ToSegments = new List<string>();
                ToSegments.Add(MainDef.PrefixSeg + sto.Id.ToString());

                int i = 0;
                List<UserSendInfo> ToUsers = uto.Take(num - 1).Select(x => new UserSendInfo() { ToUser = x.Id, ToTransport = /*(TransportKind?)*/transports[i++] }).ToList();

                SendMessageRequest request = //MakeRequest();
                new SendMessageRequest() {
                    Text = "Text Mailing message1",
                    //ToUsers = ToUsers,
                    //ToSegments = ToSegments,
                    Type = MessageKind.Mailing,
                    Selection = new UserSelection() {
                        Include = ToUsers.Select(u => u.ToUser.Value).ToList(),
                        Segments = ToSegments
                    }
                //ToTransport = TransportKind.FLChat
            };
                SendMessageResponse response = handler.ProcessRequestTransaction(entities, from, request);

                Assert.IsNotNull(response);
                Assert.IsNotNull(response.MessageId);
                //Assert.AreEqual(MessageStatus.Sent, response.Status);

                DAL.Model.Message msg = entities
                    .Message
                    .Where(m => m.Id == response.MessageId)
                    .Include(m => m.ToUsers)
                    .FirstOrDefault();

                //check message
                Assert.IsNotNull(msg);
                //Assert.IsTrue(DateTime.Now - msg.PostTm)
                Assert.AreEqual(MessageKind.Mailing, msg.Kind);
                Assert.AreEqual(from.Id, msg.FromUserId);
                //Assert.AreEqual(TransportKind.FLChat, msg.FromTransportKind);  //??
                Assert.AreEqual(num, msg.ToUsers.Count);

                //MessageToUser 
                //MessageToSegment msgTo = msg.MessageToSegment;
                //Assert.AreEqual(to.Id, msgTo.SegmentId);
                //Assert.AreEqual(TransportKind.FLChat, msgTo.ToTransportKind);            
                Assert.AreEqual(request.Text, msg.Text);

                Guid[] msgto = msg.ToUsers.Select(u => u.ToUserId).ToArray();
                CollectionAssert.AreEquivalent(uto.Select(u => u.Id).ToArray(), msgto);

                //trans.Commit();
            }
        }

        /// <summary>
        /// Successfull message to segment and user
        /// </summary>
        [TestMethod]
        public void SendMessage_Mailing_with_different_TransportType()
        {
            //  Users. users 0, 1 and 2 are message addressee, users 1 and 4 include to segment
            int num = 4;
            TransportKind?[] transports = new TransportKind?[num];
            transports[0] = null;
            transports[1] = TransportKind.FLChat;
            transports[2] = TransportKind.Telegram;
            for (int j = 3; j < num; ++j)
            {
                transports[j] = TransportKind.Email;
            }
            DAL.Model.User[] uto = entities.GetUsers
                (num,
                u => u.Enabled
                    //&& u.Transports.Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.Email).Any()
                    && u.Id != from.Id && u.OwnerUserId == from.Id && u.Email != null,
                u =>
                {
                    u.Enabled = true;
                    u.OwnerUserId = from.Id;
                    u.Email = Guid.NewGuid() + "@ya.com";
                    //u.Transports.Add(new DAL.Model.Transport()
                    //{
                    //    Enabled = true,
                    //    Kind = TransportKind.Email
                    //});
                });

            // Segment
            Segment sto = entities.GetSegment(s => !s.IsDeleted && s.Members.Contains(uto[1]) && s.Members.Contains(uto[num - 1]),
                s => { s.IsDeleted = false; s.Members.Add(uto[1]); s.Members.Add(uto[num - 1]); });
            //  Пользователь и в сегменте - рассылка в два направления            

            List<string> ToSegments = new List<string>();
            ToSegments.Add(MainDef.PrefixSeg + sto.Id.ToString());

            int i = 0;
            List<UserSendInfo> ToUsers = uto.Take(num - 1).Select(x => new UserSendInfo()
            { ToUser = x.Id, ToTransport = /*(TransportKind?)*/transports[i++] }).ToList();

            SendMessageRequest request = //MakeRequest();
            new SendMessageRequest()
            {
                Text = "Text Mailing message2",
                ToUsers = ToUsers,
                ToSegments = ToSegments,
                Type = MessageKind.Mailing,
                //ToTransport = TransportKind.FLChat
            };
            SendMessageResponse response = handler.ProcessRequestTransaction(entities, from, request);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.MessageId);            

            DAL.Model.Message msg = entities
                .Message
                .Where(m => m.Id == response.MessageId)
                .Include(m => m.ToUsers)
                .FirstOrDefault();

            //check message
            Assert.IsNotNull(msg);            
            Assert.AreEqual(MessageKind.Mailing, msg.Kind);
            
            foreach(var mtu in  msg.ToUsers)
            {
                Assert.AreEqual(mtu.ToTransportTypeId, (int)TransportKind.Email);
            }
        }

        /// <summary>
        /// Successfull message to users with Email addressee and not send for some without
        /// </summary>
        [TestMethod]
        public void SendMessage_Mailing_some_without_Email_TransportType()
        {
            //  Users. users 0, 1 and 2 are message addressee, users 1 and 4 include to segment
            int num = 4;
            TransportKind?[] transports = new TransportKind?[num];
            transports[0] = null;
            transports[1] = TransportKind.FLChat;
            transports[2] = TransportKind.Telegram;
            for (int j = 3; j < num; ++j)
            {
                transports[j] = TransportKind.Email;
            }
            DAL.Model.User[] uto = entities.GetUsers
                (num,
                u => u.Enabled
                    //&& u.Transports.Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.Email).Any()
                    && u.Id != from.Id && u.OwnerUserId == from.Id && u.Email != null,
                u =>
                {
                    u.Enabled = true;
                    u.OwnerUserId = from.Id;
                    u.Email = Guid.NewGuid() + "@ya.com";
                    //u.Transports.Add(new DAL.Model.Transport()
                    //{
                    //    Enabled = true,
                    //    Kind = TransportKind.Email
                    //});
                });
            uto[0].Email = null;
            entities.SaveChanges();  //  Весьма надо для сохранения изменений, отработки триггеров и т.п.

            // Segment
            Segment sto = entities.GetSegment(s => !s.IsDeleted && s.Members.Contains(uto[1]) && s.Members.Contains(uto[num - 1]),
                s => { s.IsDeleted = false; s.Members.Add(uto[1]); s.Members.Add(uto[num - 1]); });
            //  Пользователь и в сегменте - рассылка в два направления            

            List<string> ToSegments = new List<string>();
            ToSegments.Add(MainDef.PrefixSeg + sto.Id.ToString());

            int i = 0;
            List<UserSendInfo> ToUsers = uto.Take(num - 1).Select(x => new UserSendInfo()
            { ToUser = x.Id, ToTransport = /*(TransportKind?)*/transports[i++] }).ToList();

            SendMessageRequest request = //MakeRequest();
            new SendMessageRequest()
            {
                Text = "Text Mailing message3",
                ToUsers = ToUsers,
                ToSegments = ToSegments,
                Type = MessageKind.Mailing,
                //ToTransport = TransportKind.FLChat
            };
            SendMessageResponse response = handler.ProcessRequestTransaction(entities, from, request);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.MessageId);            

            DAL.Model.Message msg = entities
                .Message
                .Where(m => m.Id == response.MessageId)
                .Include(m => m.ToUsers)
                .FirstOrDefault();

            //check message
            Assert.IsNotNull(msg);
            Assert.AreEqual(MessageKind.Mailing, msg.Kind);            

            Assert.AreEqual(num-1, msg.ToUsers.Count);            
            Guid[] msgto = msg.ToUsers.Select(u => u.ToUserId).ToArray();           
            Assert.IsFalse(msgto.Contains(uto[0].UserId));
           
        }
        /// <summary>
        /// Message with MessageKind.Mailing don't change LastUsedTransport [TransportTypeId]        
        /// </summary>
        [TestMethod]
        public void SendMessage_Mailing_Not_Change_LastUsedTransport()
        {
            //get users
            DAL.Model.User[] users = entities.GetUsers(2,
                u => u.Enabled && u.Id != from.Id && u.Email != null
                    && u.Transports.Where(t => t.TransportTypeId == (int)TransportKind.FLChat && t.Enabled).Any(),
                //&& u.Transports.Where(t => t.TransportTypeId == (int)TransportKind.Test && t.Enabled).Any(),
                u => {
                    u.Enabled = true;
                    u.Email = Guid.NewGuid() + "@ya.com";
                    u.Transports.Add(new DAL.Model.Transport() { Enabled = true, Kind = TransportKind.FLChat });
                    //u.Transports.Add(new DAL.Model.Transport() { Enabled = true, Kind = TransportKind.Test, TransportOuterId = Guid.NewGuid().ToString() });
                });
            DAL.Model.User u1 = users[0];
            DAL.Model.User u2 = users[1];
            entities.SaveChanges();

            //make messages
            {
                entities.SendMessage(u1.Id, u2.Id, TransportKind.FLChat, TransportKind.FLChat, "Test message", MessageKind.Personal);
                
                //get default transport
                TransportKind? deftransportKind = (TransportKind?)entities
                    .UserDefaultTransportView
                    .Where(t => t.UserId == u1.Id)
                    .Select(t => t.DefaultTransportTypeId)
                    .SingleOrDefault();
                Assert.IsNotNull(deftransportKind);
                Assert.AreEqual(deftransportKind.Value, TransportKind.FLChat);
            }

            {
                entities.SendMessage(u1.Id, u2.Id, TransportKind.Email, TransportKind.Email, "Test message - mailing", MessageKind.Mailing);
                                
                //get default transport
                TransportKind? deftransportKind = (TransportKind?)entities
                    .UserDefaultTransportView
                    .Where(t => t.UserId == u1.Id)
                    .Select(t => t.DefaultTransportTypeId)
                    .SingleOrDefault();
                Assert.IsNotNull(deftransportKind);
                Assert.AreNotEqual(deftransportKind.Value, TransportKind.Email);
            }
        }

    }
}
