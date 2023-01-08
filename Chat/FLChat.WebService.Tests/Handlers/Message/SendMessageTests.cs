using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Net;

using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using FLChat.FDAL.Model;
using FLChat.WebService.Handlers.File;
using FLChat.Core.MsgCompilers;
using FLChat.Core;

namespace FLChat.WebService.Handlers.Message.Tests
{
    [TestClass]
    public class SendMessageTests
    {
        FileEntities fileEntities;
        ChatEntities entities;
        SendMessage handler;
        DAL.Model.User from;

        [TestInitialize]
        public void Init()
        {
            fileEntities = new FileEntities();
            entities = new ChatEntities();

            handler = new SendMessage();

            from = entities.GetUser(
                u => u.Enabled && u.Transports.Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.FLChat).Any(),
                u => {
                    u.Enabled = true;
                    u.Transports.Add(new DAL.Model.Transport() {
                        TransportTypeId = (int)TransportKind.FLChat,
                        Enabled = true
                    });
                });
        }

        [TestCleanup]
        public void Clean()
        {
            entities.Dispose();
            fileEntities.Dispose();
        }

        /// <summary>
        /// Successfull personal FLChat -> FLChat message 
        /// 31.01.2020 add DelayedStart - NULL
        /// </summary>
        [TestMethod]
        public void SendMessage_PersonalInner() {
            DAL.Model.User to = entities.GetUser(
                u => u.Enabled 
                    && u.Transports
                            .Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.FLChat).Any()
                    && u.Id != from.Id,
                u => {
                    u.Enabled = true;
                    u.Transports.Add(new DAL.Model.Transport() {
                        Enabled = true,
                        Kind = TransportKind.FLChat
                    });
                });

            SendMessageRequest request = new SendMessageRequest() {
                Text = "Text message",
                ToUser = to.Id,
                Type = MessageKind.Personal,
//                DelayedStart = DateTime.UtcNow,
                ToTransport = TransportKind.FLChat
            };
            SendMessagePersonalResponse response = handler.ProcessRequestTransaction(entities, from, request) as SendMessagePersonalResponse;

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.MessageId);
            Assert.AreEqual(MessageStatus.Sent, response.Status);
            Assert.AreEqual(to.Id, response.User.ToUser);
            Assert.AreEqual(TransportKind.FLChat, response.User.ToTransport);
            Assert.AreEqual(MessageStatus.Sent, response.User.Status);

            DAL.Model.Message msg = entities
                .Message
                .Where(m => m.Id == response.MessageId)
                .Include(m => m.ToUsers)
                .FirstOrDefault();

            //check message
            Assert.IsNotNull(msg);
            //Assert.IsTrue(DateTime.Now - msg.PostTm)
            Assert.AreEqual(MessageKind.Personal, msg.Kind);
            Assert.AreEqual(from.Id, msg.FromUserId);
            Assert.IsNull(msg.DelayedStart);
            Assert.AreEqual(TransportKind.FLChat, msg.FromTransportKind);
            Assert.AreEqual(1, msg.ToUsers.Count);
            MessageToUser msgTo = msg.ToUsers.Single();
            Assert.AreEqual(to.Id, msgTo.ToUserId);
            Assert.AreEqual(TransportKind.FLChat, msgTo.ToTransportKind);
            Assert.IsFalse(msg.IsDeleted);
            Assert.IsTrue(msgTo.IsSent);
            Assert.IsFalse(msgTo.IsFailed);
            Assert.IsFalse(msgTo.IsDelivered);
            Assert.IsFalse(msgTo.IsRead);
            Assert.AreEqual(request.Text, msg.Text);
        }

        /// <summary>
        /// Can't send message to himself
        /// </summary>
        [TestMethod]
        public void SendMessage_Personal_ToHimself() {
            SendMessageRequest request = new SendMessageRequest() {
                Text = "Text message",
                ToUser = from.Id,
                Type = DAL.MessageKind.Personal
            };

            try {
                SendMessageResponse responce = handler.ProcessRequestTransaction(entities, from, request);
                Assert.Fail("Exception has not thrown");
            } catch (ErrorResponseException e) {
                e.Check(HttpStatusCode.BadRequest, ErrorResponse.Kind.not_support);
            }
        }

        /// <summary>
        /// Failed personal message, addressee can't be empty
        /// </summary>
        [TestMethod]
        public void SendMessage_Personal_ToNull() {
            SendMessageRequest request = new SendMessageRequest() {
                Text = "Text message",
                ToUser = null,
                Type = DAL.MessageKind.Personal
            };

            try {
                SendMessageResponse responce = handler.ProcessRequestTransaction(entities, from, request);
                Assert.Fail("Exception has not thrown");
            } catch (ErrorResponseException e) {
                e.Check(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error);
            }
        }

        /// <summary>
        /// Send message to user without transport (send to default transport)
        /// </summary>
        //[TestMethod]
        //public void SendMessage_Personal_ToUserWithoutTransport() {
        //    User to = entities.GetUser(
        //        u => u.Enabled 
        //            && u.Transports.Where(t => t.Enabled).Any() == false
        //            && u.Id != from.Id,
        //        u => { u.Enabled = true; });

        //    SendMessageRequest request = new SendMessageRequest() {
        //        Text = "Text message",
        //        ToUser = to.Id,
        //        Type = DAL.MessageKind.Personal
        //    };

        //    SendMessagePersonalResponse response = handler.ProcessRequest(entities, from, request) as SendMessagePersonalResponse;
        //    Assert.IsNotNull(response);
        //    Assert.AreEqual(MessageStatus.TransportNotFound, response.Status);
        //    //Assert.AreEqual(MessageStatus.Sent, response.Status);
        //    Assert.IsNull(response.MessageId);
        //}

        /// <summary>
        /// Send message to user without transport (send to specified transport)
        /// </summary>
        [TestMethod]
        public void SendMessage_Personal_ToUserWithoutFLChat() {
            DAL.Model.User to = entities.GetUser(
                u => u.Enabled
                    && u.Transports.Where(t => t.Enabled && t.TransportTypeId != 0).Any() == true
                    && u.Transports.Where(t => t.Enabled && t.TransportTypeId == 0).Any() == false
                    && u.Id != from.Id,
                u => {
                    u.Enabled = true;
                    u.Transports.Add(new DAL.Model.Transport() {
                        Kind = TransportKind.Telegram,
                        Enabled = true,
                        TransportOuterId = "test_ts_" + DateTime.Now.ToString()
                    });
                });

            SendMessageRequest request = new SendMessageRequest() {
                Text = "Text message",
                ToUser = to.Id,
                Type = DAL.MessageKind.Personal,
                ToTransport = TransportKind.FLChat
            };

            SendMessagePersonalResponse response = handler.ProcessRequestTransaction(entities, from, request) as SendMessagePersonalResponse;
            Assert.IsNotNull(response);
            Assert.AreEqual(MessageStatus.TransportNotFound, response.Status);
            Assert.IsNull(response.MessageId);
            Assert.AreEqual(to.Id, response.User.ToUser);
            Assert.AreEqual(MessageStatus.TransportNotFound, response.User.Status);
        }

        /// <summary>
        /// Send message to user with disabled transport
        /// </summary>
        [TestMethod]
        public void SendMessage_Personal_ToDisabledTransport()
        {
            DAL.Model.User to = entities.GetUser(
                u => u.Enabled
                    && u.Transports.Where(t => t.Enabled == false && t.TransportTypeId == 0).Any() == true
                    && u.Id != from.Id,
                u =>
                {
                    u.Enabled = true;
                    u.Transports.Add(new DAL.Model.Transport()
                    {
                        Kind = TransportKind.FLChat,
                        Enabled = false
                    });
                });

            SendMessageRequest request = new SendMessageRequest()
            {
                Text = "Text message",
                ToUser = to.Id,
                Type = DAL.MessageKind.Personal,
                ToTransport = TransportKind.FLChat
            };

            SendMessagePersonalResponse response = handler.ProcessRequestTransaction(entities, from, request) as SendMessagePersonalResponse;
            Assert.IsNotNull(response);
            Assert.AreEqual(MessageStatus.TransportNotFound, response.Status);
            Assert.IsNull(response.MessageId);
            Assert.AreEqual(to.Id, response.User.ToUser);
            Assert.AreEqual(MessageStatus.TransportNotFound, response.User.Status);
        }

        ///// <summary>
        ///// Successfull segment message 
        ///// </summary>
        //[TestMethod]
        //public void SendMessage_Segment()
        //{
        //    Segment to = entities.GetSegment(s => !s.IsDeleted, s => s.IsDeleted = false );

        //    SendMessageRequest request = new SendMessageRequest()
        //    {
        //        Text = "Text segment message",
        //        ToSegment = MainDef.PrefixSeg + to.Id.ToString(),
        //        Type = MessageKind.Segment,
        //        ToTransport = TransportKind.FLChat
        //    };
        //    SendMessageResponse response = handler.ProcessRequest(entities, from, request) ;

        //    Assert.IsNotNull(response);
        //    Assert.IsNotNull(response.MessageId);
        //    //Assert.AreEqual(MessageStatus.Sent, response.Status);

        //    DAL.Model.Message msg = entities
        //        .Message
        //        .Where(m => m.Id == response.MessageId)
        //        .Include(m => m.ToUsers)
        //        .FirstOrDefault();

        //    //check message
        //    Assert.IsNotNull(msg);
        //    //Assert.IsTrue(DateTime.Now - msg.PostTm)
        //    Assert.AreEqual(MessageKind.Segment, msg.Kind);
        //    Assert.AreEqual(from.Id, msg.FromUserId);
        //    //Assert.AreEqual(TransportKind.FLChat, msg.FromTransportKind);  //??
        //    //Assert.AreEqual(1, msg.ToUsers.Count);

        //    //MessageToUser 
        //    MessageToSegment msgTo = msg.MessageToSegment.FirstOrDefault();
        //    Assert.AreEqual(to.Id, msgTo.SegmentId);
        //    //Assert.AreEqual(TransportKind.FLChat, msgTo.ToTransportKind);
        //    //Assert.IsFalse(msg.IsDeleted);
        //    //Assert.IsTrue(msgTo.IsSent);
        //    //Assert.IsFalse(msgTo.IsFailed);
        //    //Assert.IsFalse(msgTo.IsDelivered);
        //    //Assert.IsFalse(msgTo.IsRead);
        //    Assert.AreEqual(request.Text, msg.Text);
        //}

        /// <summary>
        /// Successfull message to segment and user
        /// </summary>
        [TestMethod]
        public void SendMessage_Broadcast_User_Segm()
        {
            //  Users. users 0, 1 and 2 are message addressee, users 1 and 4 include to segment
            int num = 4;
            TransportKind?[] transports = new TransportKind?[num];
            transports[0] = null;
            for (int j = 1; j < num; ++j)
            {
                transports[j] = TransportKind.FLChat;
            }
            DAL.Model.User[] uto = entities.GetUsers
                (num,
                u => u.Enabled
                    && u.Transports
                            .Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.FLChat).Any()
                    && u.Id != from.Id && u.OwnerUserId == from.Id,
                u =>
                {
                    u.Enabled = true;
                    u.OwnerUserId = from.Id;
                    u.Transports.Add(new DAL.Model.Transport()
                    {
                        Enabled = true,
                        Kind = TransportKind.FLChat
                    });
                });

            // Segment
            DAL.Model.Segment sto = entities.GetSegment(s => !s.IsDeleted && s.Members.Contains(uto[1]) && s.Members.Contains(uto[num - 1]),
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
                Text = "Text Broadcast message1",
                ToUsers = ToUsers,
                ToSegments = ToSegments,
                Type = MessageKind.Broadcast,
                //ToTransport = TransportKind.FLChat
            };
            SendMessageBroadcastResponse response = handler.ProcessRequestTransaction(entities, from, request) as SendMessageBroadcastResponse;

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.MessageId);
            //Assert.AreEqual(MessageStatus.Sent, response.Status);
            //all users is included to response
            CollectionAssert.IsSubsetOf(
                uto.Select(u => u.Id).ToArray(),
                response.Users.Select(u => u.ToUser).ToArray());

            DAL.Model.Message msg = entities
                .Message
                .Where(m => m.Id == response.MessageId)
                .Include(m => m.ToUsers)
                .FirstOrDefault();

            //check message
            Assert.IsNotNull(msg);
            //Assert.IsTrue(DateTime.Now - msg.PostTm)
            Assert.AreEqual(MessageKind.Broadcast, msg.Kind);
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
        }

        /// <summary>
        /// Successfull message to segment only
        /// </summary>
        [TestMethod]
        public void SendMessage_Broadcast_Segm()
        {
            //  Users. 
            int num = 2;
            TransportKind?[] transports = new TransportKind?[num];
            transports[0] = null;
            for (int j = 1; j < num; ++j)
            {
                transports[j] = TransportKind.FLChat;
            }
            DAL.Model.User[] uto = entities.GetUsers
                (num,
                u => u.Enabled
                    && u.Transports
                            .Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.FLChat).Any()
                    && u.Id != from.Id && u.OwnerUserId == from.Id,
                u =>
                {
                    u.Enabled = true;
                    u.OwnerUserId = from.Id;
                    u.Transports.Add(new DAL.Model.Transport()
                    {
                        Enabled = true,
                        Kind = TransportKind.FLChat
                    });
                });

            // Segment
            DAL.Model.Segment sto = entities.GetSegment(s => !s.IsDeleted && s.Members.Contains(uto[0]) && s.Members.Contains(uto[num - 1]),
                s => { s.IsDeleted = false; s.Members.Add(uto[0]); s.Members.Add(uto[num - 1]); });
            //  Пользователь и в сегменте - рассылка в два направления            

            List<string> ToSegments = new List<string>();
            ToSegments.Add(MainDef.PrefixSeg + sto.Id.ToString());
            List<UserSendInfo> ToUsers = new List<UserSendInfo>();

            SendMessageRequest request = //MakeRequest();
            new SendMessageRequest()
            {
                Text = "Text Broadcast message2",
                //ToUsers = ToUsers,
                ToSegments = ToSegments,
                Type = MessageKind.Broadcast,
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
            Assert.AreEqual(MessageKind.Broadcast, msg.Kind);
            //Assert.AreEqual(TransportKind.FLChat, msg.FromTransportKind);  //??
            Assert.AreEqual(num, msg.ToUsers.Count);

            //MessageToUser 
            //MessageToSegment msgTo = msg.MessageToSegment;
            //Assert.AreEqual(to.Id, msgTo.SegmentId);
            //Assert.AreEqual(TransportKind.FLChat, msgTo.ToTransportKind);            
            Assert.AreEqual(request.Text, msg.Text);

            Guid[] msgto = msg.ToUsers.Select(u => u.ToUserId).ToArray();
            CollectionAssert.AreEquivalent(uto.Select(u => u.Id).ToArray(), msgto);
        }

        /// <summary>
        /// Successfull message to users only
        /// 31.01.2020 add DelayedStart - null
        /// </summary>
        [TestMethod]
        public void SendMessage_Broadcast_User()
        {
            //  Users.
            int num = 2;
            TransportKind?[] transports = new TransportKind?[num];
            transports[0] = null;
            for (int j = 1; j < num; ++j)
            {
                transports[j] = TransportKind.FLChat;
            }
            DAL.Model.User[] uto = entities.GetUsers
                (num,
                u => u.Enabled
                    && u.Transports
                            .Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.FLChat).Any()
                    && u.Id != from.Id && u.OwnerUserId == from.Id,
                u =>
                {
                    u.Enabled = true;
                    u.OwnerUserId = from.Id;
                    u.Transports.Add(new DAL.Model.Transport()
                    {
                        Enabled = true,
                        Kind = TransportKind.FLChat
                    });
                });

            List<string> ToSegments = new List<string>();
            int i = 0;
            List<UserSendInfo> ToUsers = uto.Select(x => new UserSendInfo()
            { ToUser = x.Id, ToTransport = /*(TransportKind?)*/transports[i++] }).ToList();

            SendMessageRequest request = //MakeRequest();
            new SendMessageRequest()
            {
                Text = "Text Broadcast message3",
                ToUsers = ToUsers,
                //ToSegments = ToSegments,
                Type = MessageKind.Broadcast,
                //DelayedStart = DateTime.UtcNow,
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
            Assert.AreEqual(MessageKind.Broadcast, msg.Kind);
            Assert.AreEqual(from.Id, msg.FromUserId);
            Assert.IsNull(msg.DelayedStart);
            //Assert.AreEqual(TransportKind.FLChat, msg.FromTransportKind);  //??
            //Assert.IsTrue(num >= msg.ToUsers.Count);
            Assert.AreEqual(num, msg.ToUsers.Count);

            //MessageToUser 
            //MessageToSegment msgTo = msg.MessageToSegment;
            //Assert.AreEqual(to.Id, msgTo.SegmentId);
            //Assert.AreEqual(TransportKind.FLChat, msgTo.ToTransportKind);            
            Assert.AreEqual(request.Text, msg.Text);

            Guid[] msgto = msg.ToUsers.Select(u => u.ToUserId).ToArray();
            CollectionAssert.AreEquivalent(uto.Select(u => u.Id).ToArray(), msgto);
        }

        /// <summary>
        /// Successfull message to users only with FileData
        /// </summary>
        [TestMethod]
        public void SendMessage_Broadcast_User_with_FileData()
        {
            //  Users.
            int num = 2;
            TransportKind?[] transports = new TransportKind?[num];
            transports[0] = null;
            for (int j = 1; j < num; ++j)
            {
                transports[j] = TransportKind.FLChat;
            }
            DAL.Model.User[] uto = entities.GetUsers
                (num,
                u => u.Enabled
                    && u.Transports
                            .Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.FLChat).Any()
                    && u.Id != from.Id && u.OwnerUserId == from.Id,
                u =>
                {
                    u.Enabled = true;
                    u.OwnerUserId = from.Id;
                    u.Transports.Add(new DAL.Model.Transport()
                    {
                        Enabled = true,
                        Kind = TransportKind.FLChat
                    });
                });

            //List<string> ToSegments = new List<string>();
            int i = 0;
            List<UserSendInfo> ToUsers = uto.Select(x => new UserSendInfo()
            { ToUser = x.Id, ToTransport = /*(TransportKind?)*/transports[i++] }).ToList();

            // File
            byte[] fileData = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
            string fileName = "test.png";
            FileInfoData file = new FileInfoData()
            {
                Data = Convert.ToBase64String(fileData),
                FileName = fileName,
                FileMediaType = MediaGroupKind.Image,               
            };
            SendMessageRequest request = //MakeRequest();
            new SendMessageRequest()
            {
                Text = "Text Broadcast message4",
                ToUsers = ToUsers,
                //ToSegments = ToSegments,
                Type = MessageKind.Broadcast,
                File = file,                
                //ToTransport = TransportKind.FLChat
            };
            SendMessageResponse response = handler.ProcessRequestTransaction(entities, from, request);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.MessageId);
            //Assert.AreEqual(MessageStatus.Sent, response.Status);

            //DAL.Model.Message msg = entities
            //    .Message
            //    .Where(m => m.Id == response.MessageId)
            //    .Include(m => m.ToUsers)
            //    .FirstOrDefault();

            ////check message
            //Assert.IsNotNull(msg);

            // File
            var fileId = response.FileId;
            var fileSaved = entities.FileInfo.Where(x => x.Id == fileId && x.FileOwnerId == from.Id)
                .FirstOrDefault();
            Assert.IsNotNull(fileSaved);

            //  file id
            Assert.AreEqual(fileId, fileSaved.Id);
            //  file data
            var fileSavedF = fileEntities.FileData.Where(x => x.Id == fileId)
                .FirstOrDefault();
            Assert.IsNotNull(fileSavedF);
            CollectionAssert.AreEquivalent(fileData, fileSavedF.Data);

            //  file Addressee list
            //CollectionAssert.AreEquivalent(ToUsers.Select(x => x.ToUser).ToList(),
            //    fileSaved.FileAddressee.Select(x => x.AddresseeId).ToList());
        }

        /// <summary>
        /// Successfull personal FLChat -> FLChat message with FileData
        /// </summary>
        [TestMethod]
        public void SendMessage_PersonalInner_with_FileData()
        {
            DAL.Model.User to = entities.GetUser(
                u => u.Enabled
                    && u.Transports
                            .Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.FLChat).Any()
                    && u.Id != from.Id,
                u => {
                    u.Enabled = true;
                    u.Transports.Add(new DAL.Model.Transport()
                    {
                        Enabled = true,
                        Kind = TransportKind.FLChat
                    });
                });

            // File
            byte[] fileData = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
            string fileName = "test.png";
            FileInfoData file = new FileInfoData()
            {
                Data = Convert.ToBase64String(fileData),
                FileName = fileName,
                FileMediaType = MediaGroupKind.Image,
            };

            SendMessageRequest request = //MakeRequest();
            new SendMessageRequest()
            {
                Text = "Text Broadcast message5",
                ToUser = to.Id,
                Type = MessageKind.Personal,
                ToTransport = TransportKind.FLChat,
                File = file,                
            };

            SendMessagePersonalResponse response = handler.ProcessRequestTransaction(entities, from, request) as SendMessagePersonalResponse;

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.MessageId);

            // File
            var fileId = response.FileId;
            var fileSaved = fileEntities.FileData.Where(x => x.Id == fileId)
                .FirstOrDefault();
            Assert.IsNotNull(fileSaved);

            //  file id
            Assert.AreEqual(fileId, fileSaved.Id);
            //  file data
            CollectionAssert.AreEquivalent(fileData, fileSaved.Data);            
        }

        /// <summary>
        /// Successfull message to users only with FileId
        /// </summary>
        [TestMethod]
        public void SendMessage_Broadcast_User_with_FileId()
        {
            //  Users.
            int num = 2;
            TransportKind?[] transports = new TransportKind?[num];
            transports[0] = null;
            for (int j = 1; j < num; ++j)
            {
                transports[j] = TransportKind.FLChat;
            }
            DAL.Model.User[] uto = entities.GetUsers
                (num,
                u => u.Enabled
                    && u.Transports
                            .Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.FLChat).Any()
                    && u.Id != from.Id && u.OwnerUserId == from.Id,
                u =>
                {
                    u.Enabled = true;
                    u.OwnerUserId = from.Id;
                    u.Transports.Add(new DAL.Model.Transport()
                    {
                        Enabled = true,
                        Kind = TransportKind.FLChat
                    });
                });

            //List<string> ToSegments = new List<string>();
            int i = 0;
            List<UserSendInfo> ToUsers = uto.Select(x => new UserSendInfo()
            { ToUser = x.Id, ToTransport = /*(TransportKind?)*/transports[i++] }).ToList();

            // File - Id

            Guid newfileId = GetFileId();

            SendMessageRequest request = //MakeRequest();
            new SendMessageRequest()
            {
                Text = "Text Broadcast message6",
                ToUsers = ToUsers,
                Type = MessageKind.Broadcast,
                FileId = newfileId,
            };
            SendMessageResponse response = handler.ProcessRequestTransaction(entities, from, request);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.MessageId);

            //// File
            //var fileId = response.FileId;
            ////var fileSaved = entities.FileInfo.Where(x => x.Id == fileId && x.FileOwnerId == from.Id)
            ////    .FirstOrDefault();

            ////Assert.IsNotNull(fileSaved);

            ////  file id
            //Assert.AreEqual(fileId, newfileId);
            //Assert.AreEqual(fileId, fileSaved.Id);
        }

        /// <summary>
        /// Successfull personal FLChat -> FLChat message with FileId
        /// </summary>
        [TestMethod]
        public void SendMessage_PersonalInner_with_FileId()
        {
            DAL.Model.User to = entities.GetUser(
                u => u.Enabled
                    && u.Transports
                            .Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.FLChat).Any()
                    && u.Id != from.Id,
                u => {
                    u.Enabled = true;
                    u.Transports.Add(new DAL.Model.Transport()
                    {
                        Enabled = true,
                        Kind = TransportKind.FLChat
                    });
                });

            // File
            //byte[] fileData = new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 };
            //string fileName = "test.docx";

            // File - Id

            Guid newfileId = GetFileId();

            SendMessageRequest request = //MakeRequest();
            new SendMessageRequest()
            {
                Text = "Text Broadcast message7",
                ToUser = to.Id,
                Type = MessageKind.Personal,
                ToTransport = TransportKind.FLChat,
                FileId = newfileId,
            };

            SendMessagePersonalResponse response = handler.ProcessRequestTransaction(entities, from, request) as SendMessagePersonalResponse;

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.MessageId);

            // File
            //var fileId = response.FileId;
            //var fileSaved = entities.FileInfo.Where(x => x.Id == fileId && x.FileOwnerId == from.Id)
            //    .FirstOrDefault();

            //Assert.IsNotNull(fileSaved);

            //  file id
            Assert.AreEqual(response.FileId, request.FileId);
        }
        /// <summary>
        /// Not Successfull personal FLChat -> FLChat message with FileId and FileData
        /// </summary>
        [TestMethod]
        public void SendMessage_PersonalInner_with_FileId_FileData()
        {
            DAL.Model.User to = entities.GetUser(
                u => u.Enabled
                    && u.Transports
                            .Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.FLChat).Any()
                    && u.Id != from.Id,
                u => {
                    u.Enabled = true;
                    u.Transports.Add(new DAL.Model.Transport()
                    {
                        Enabled = true,
                        Kind = TransportKind.FLChat
                    });
                });

            // File
            byte[] fileData = new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 };
            string fileName = "test.doc";
            Guid fileGuid = GetFileId();
            FileInfoData file = new FileInfoData()
            {
                Data = Convert.ToBase64String(fileData),
                FileName = fileName,
                FileMediaType = MediaGroupKind.Document,
            };

            SendMessageRequest request = //MakeRequest();
            new SendMessageRequest()
            {
                Text = "Text personal message8",
                ToUser = to.Id,
                Type = MessageKind.Personal,
                ToTransport = TransportKind.FLChat,
                File = file,
                FileId = fileGuid,            
            };
            try
            {
                SendMessageResponse responce = handler.ProcessRequestTransaction(entities, from, request);
                Assert.Fail("Exception has not thrown for file and file_id");
            }
            catch (ErrorResponseException e)
            {
                e.Check(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error);
            }
        }
        /// <summary>
        /// Successfull personal FLChat -> FLChat message with FileData
        /// </summary>
        [TestMethod]
        public void SendMessage_PersonalInner_with_FileData_Correct()
        {
            DAL.Model.User to = entities.GetUser(
                u => u.Enabled
                    && u.Transports
                            .Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.FLChat).Any()
                    && u.Id != from.Id,
                u => {
                    u.Enabled = true;
                    u.Transports.Add(new DAL.Model.Transport()
                    {
                        Enabled = true,
                        Kind = TransportKind.FLChat
                    });
                });

            // File
            byte[] fileData = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
            string fileName = "test.png";
            string strBase = Convert.ToBase64String(fileData); //"w7/DmA==";
            //Guid fileGuid = GetFileId();
            FileInfoData file = new FileInfoData()
            {
                //Data = strBase,
                Data = Convert.ToBase64String(fileData),
                FileName = fileName,
                FileMediaType = MediaGroupKind.Image,
            };

            SendMessageRequest request = //MakeRequest();
            new SendMessageRequest()
            {
                Text = "Text personal message9",
                ToUser = to.Id,
                Type = MessageKind.Personal,
                ToTransport = TransportKind.FLChat,
                File = file,
                //FileId = fileGuid,
            };
            SendMessageResponse response = handler.ProcessRequestTransaction(entities, from, request);
            
            // File
            var fileId = response.FileId;
            var fileSaved = fileEntities.FileData.Where(x => x.Id == fileId)
                .FirstOrDefault();
            Assert.IsNotNull(fileSaved);
        }
        /// <summary>
        /// Not Successfull personal FLChat -> FLChat message with FileData
        /// </summary>
        [TestMethod]
        public void SendMessage_PersonalInner_with_FileData_Not_Correct()
        {
            DAL.Model.User to = entities.GetUser(
                u => u.Enabled
                    && u.Transports
                            .Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.FLChat).Any()
                    && u.Id != from.Id,
                u => {
                    u.Enabled = true;
                    u.Transports.Add(new DAL.Model.Transport()
                    {
                        Enabled = true,
                        Kind = TransportKind.FLChat
                    });
                });

            // File
            byte[] fileData = new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 };
            string fileName = "test.doc";
            //Guid fileGuid = GetFileId();
            FileInfoData file = new FileInfoData()
            {
                Data = Convert.ToBase64String(fileData),
                FileName = fileName,
                FileMediaType = MediaGroupKind.Image,
            };

            SendMessageRequest request = //MakeRequest();
            new SendMessageRequest()
            {
                Text = "Text personal message10",
                ToUser = to.Id,
                Type = MessageKind.Personal,
                ToTransport = TransportKind.FLChat,
                File = file,
                //FileId = fileGuid,
            };
            try
            {
                SendMessageResponse responce = handler.ProcessRequestTransaction(entities, from, request);
                Assert.Fail("Exception has not thrown for not_support file's kind ");
            }
            catch (ErrorResponseException e)
            {
                e.Check(HttpStatusCode.UnsupportedMediaType, ErrorResponse.Kind.not_support);
            }
        }

        /// <summary>
        /// Successfull personal FLChat -> FLChat message with FileData
        /// </summary>
        [TestMethod]
        public void AddNew_Unknown_MimeType()
        {
            FilePerformer filePerformer = new FilePerformer();
            int mediaTypeGroupId = 1;
            var fileMimeType = Guid.NewGuid().ToString();
            filePerformer.GetMediaTypeId(fileMimeType, entities, out int? mediaTypeId, ref mediaTypeGroupId);
            DAL.Model.MediaType mediaType = entities.MediaType.Where(x => x.Name == fileMimeType).FirstOrDefault();
            Assert.IsNotNull(mediaType);
            Assert.AreEqual(mediaType.MediaTypeGroupId, mediaTypeGroupId);
            entities.MediaType.Remove(mediaType);
            entities.SaveChanges();
        }

        ///// <summary>
        ///// Successfull personal FLChat -> FLChat message with FileData
        ///// </summary>
        //[TestMethod]
        //public void SendMessage_with_FileData_Unknown_MimeType()
        //{
        //    DAL.Model.User to = entities.GetUser(
        //        u => u.Enabled
        //            && u.Transports
        //                    .Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.FLChat).Any()
        //            && u.Id != from.Id,
        //        u => {
        //            u.Enabled = true;
        //            u.Transports.Add(new DAL.Model.Transport()
        //            {
        //                Enabled = true,
        //                Kind = TransportKind.FLChat
        //            });
        //        });

        //    // File
        //    byte[] fileData = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
        //    string fileName = "test.docxxx";
        //    string strBase = Convert.ToBase64String(fileData); //"w7/DmA==";
        //    //Guid fileGuid = GetFileId();
            
        //    FileInfoData file = new FileInfoData()
        //    {
        //        //Data = strBase,
        //        Data = Convert.ToBase64String(fileData),
        //        FileName = fileName,
        //        FileMediaType = MediaType.MediaTypeGroup.FileMediaType.Document,
        //        FileMimeType = Guid.NewGuid().ToString(),
        //    };

        //    SendMessageRequest request = //MakeRequest();
        //    new SendMessageRequest()
        //    {
        //        Text = "Text fileMimeType message",
        //        ToUser = to.Id,
        //        Type = MessageKind.Personal,
        //        ToTransport = TransportKind.FLChat,
        //        File = file,                
        //    };
        //    SendMessageResponse response = handler.ProcessRequest(entities, from, request);

        //    var newMT = entities.MediaType.Where(x => x.Name == file.FileMimeType).FirstOrDefault();
        //    Assert.IsNotNull(newMT);

        //    // File
        //    var fileId = response.FileId;
        //    var fileSaved = fileEntities.FileData.Where(x => x.Id == fileId)
        //        .FirstOrDefault();
        //    Assert.IsNotNull(fileSaved);
        //}

        Guid GetFileId()
        {
            FileInfo fileSaved = entities.FileInfo.FirstOrDefault();
            if(fileSaved == null)
            {
                byte[] fileData = new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 };
                fileSaved = new FileInfo()
                {
                    FileOwnerId = from.Id,
                    FileName = "test.png",
                    FileLength = fileData.Length,
                    MediaTypeId = 1,
                };
                entities.FileInfo.Add(fileSaved);
                entities.SaveChanges();
                
                fileEntities.FileData.Add(new FileData()
                {
                    Id = fileSaved.Id,
                    Data = fileData,
                    MediaTypeId = 1,
                });
                fileEntities.SaveChanges();
            }
            return fileSaved.Id;
        }

        private SendMessageRequest MakeRequest()
        {
            string json =
                "{  'type': 'Broadcast', " +
                "'to_users': [ " +
                "{'user_id': 'A4D6231A-AB97-44D4-A6C7-042070F006E2', 'transport': 'FLChat'}, " +
                "{'user_id': '08C67188-B2E2-41C3-9B9E-058359648E05', 'transport': 'FLChat'}, " +
                "{'user_id': '06974870-3E72-E911-B5CD-74D4352752DA'}   ], " +
                "'to_segments' :   [ 'seg-06974870-3E72-E911-B5CD-74D4352752DA',   ], " +
                "'text': 'Text Broadcast message'}";
            
            var v = JsonConvert.DeserializeObject<SendMessageRequest>(json);
            return v;
        }

        [TestMethod]
        public void SendMessage_Broadcast_ToSelection() {
            DAL.Model.User user = entities.GetUser(
                u => u.OwnerUserId != null && u.OwnerUser.OwnerUserId != null
                    && u.OwnerUser.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any()
                    && u.OwnerUser.OwnerUser.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any(),
                u => u.OwnerUser = entities.GetUser(
                    u2 => u2.OwnerUserId != null
                        && u2.OwnerUser.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any(),
                    u2 => u2.OwnerUser = entities.GetUser(null, null, TransportKind.FLChat),
                    TransportKind.FLChat),
                TransportKind.FLChat
                );

            Dictionary<Guid, TransportKind?> transports = entities.GetUserTransport(new Guid[] { user.Id, user.OwnerUserId.Value });
            DAL.Model.User owner = user.OwnerUser.OwnerUser;

            SendMessageRequest request = new SendMessageRequest() {
                Selection = new UserSelection() {
                    IncludeWithStructure = new List<UserSelection.UserStructureSelection>() {
                        new UserSelection.UserStructureSelection() {
                            Type = UserSelection.SelectionType.Deep,
                            UserId = user.OwnerUserId.Value
                        }
                    }
                },
                Text = "test message",
                Type = MessageKind.Broadcast,
            };

            SendMessageBroadcastResponse resp = handler.ProcessRequestTransaction(entities, owner, request) as SendMessageBroadcastResponse;
            Assert.IsNotNull(resp);
            Assert.IsNotNull(resp.Users);
            CollectionAssert.IsSubsetOf(
                new Guid[] { user.Id, user.OwnerUserId.Value },
                resp.Users.Select(u => u.ToUser).ToArray());

            //check transports
            Assert.AreEqual(transports[user.Id], resp.Users.Where(u => u.ToUser == user.Id).Single().ToTransport);
            Assert.AreEqual(transports[user.OwnerUserId.Value], resp.Users.Where(u => u.ToUser == user.OwnerUserId.Value).Single().ToTransport);

        }

        [TestMethod]
        public void SendMessage_Mailing_ToSelection() {
            DAL.Model.User user = entities.GetUser(
                u => u.OwnerUserId != null && u.OwnerUser.OwnerUserId != null
                    && u.OwnerUser.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.Email).Any()
                    && u.OwnerUser.OwnerUser.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any(),
                u => u.OwnerUser = entities.GetUser(
                    u2 => u2.OwnerUserId != null 
                        && u2.OwnerUser.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any(),
                    u2 => u2.OwnerUser = entities.GetUser(null, null, TransportKind.FLChat),
                    TransportKind.Email),
                TransportKind.Email
                );
            Dictionary<Guid, TransportKind?> transports = entities.GetUserMailingTransport(new Guid[] { user.Id, user.OwnerUserId.Value });
            DAL.Model.User owner = user.OwnerUser.OwnerUser;

            SendMessageRequest request = new SendMessageRequest() {
                Selection = new UserSelection() {
                    IncludeWithStructure = new List<UserSelection.UserStructureSelection>() {
                        new UserSelection.UserStructureSelection() {
                            Type = UserSelection.SelectionType.Deep,
                            UserId = user.OwnerUserId.Value
                        }
                    }
                },
                Text = "test message",
                Type = MessageKind.Mailing,
            };

            SendMessageBroadcastResponse resp = handler.ProcessRequestTransaction(entities, owner, request) as SendMessageBroadcastResponse;
            Assert.IsNotNull(resp);
            Assert.IsNotNull(resp.Users);
            CollectionAssert.IsSubsetOf(
                new Guid[] { user.Id, user.OwnerUserId.Value },
                resp.Users.Select(u => u.ToUser).ToArray());

            //check transports
            Assert.AreEqual(transports[user.Id], resp.Users.Where(u => u.ToUser == user.Id).Single().ToTransport);
            Assert.AreEqual(transports[user.OwnerUserId.Value], resp.Users.Where(u => u.ToUser == user.OwnerUserId.Value).Single().ToTransport);
        }

        [TestMethod]
        public void SendMessage_Broadcast_ExceedOnceLimit() {
            MessageType type = entities.MessageType.Where(mt => mt.Id == (int)MessageKind.Broadcast).Single();
            int? onceLimit = type.LimitForOnce;
            int? dayLimit = type.LimitForDay;

            try {
                int msgCount = entities.Message.Count();

                type.LimitForDay = null;
                type.LimitForOnce = 10;
                entities.SaveChanges();

                //DAL.Model.User[] users = entities.GetUsers(t.LimitForOnce.Value + 2, u => u.Enabled, null,
                //    TransportKind.FLChat);
                DAL.Model.User owner = entities.GetUserQ(
                    where: q => q.Where(u => u.ChildUsers.Where(
                            ch => ch.Enabled
                            && ch.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any()).Count() >= 11),
                    create: u => {
                        for (int i = 0; i < 11; ++i) {
                            var nu = new DAL.Model.User() { Enabled = true };
                            nu.Transports.Add(new DAL.Model.Transport() {
                                TransportTypeId = (int)TransportKind.FLChat,
                                Enabled = true
                            });
                            u.ChildUsers.Add(nu);
                        }
                    },
                    transport: TransportKind.FLChat);
                DAL.Model.User[] users = owner.ChildUsers
                    .Where(u => u.Enabled
                        && u.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any())
                    .ToArray();

                SendMessageRequest request = new SendMessageRequest() {
                    Selection = new UserSelection() {
                        Include = users.Select(u => u.Id).ToList()
                    },
                    Text = "test message",
                    Type = type.Kind,
                };

                var e = Assert.ThrowsException<ErrorResponseException>(() => handler.ProcessRequestTransaction(entities, owner, request));
                Assert.AreEqual((int)HttpStatusCode.Forbidden, e.GetHttpCode());
                Assert.AreEqual(ErrorResponse.Kind.exceed_limit, e.Error.Error);
                Assert.IsInstanceOfType(e.Error, typeof(MessageLimitErrorResponse));
                MessageLimitErrorResponse me = e.Error as MessageLimitErrorResponse;
                Assert.IsNotNull(me.Limit);
                Assert.IsNotNull(me.Limit.ExceedOnceLimit);
                Assert.IsNotNull(me.Limit.LimitForOnce);
                Assert.AreEqual(users.Length, me.Limit.SelectionCount);

                Assert.AreEqual(msgCount, entities.Message.Count());
            } finally {
                type.LimitForDay = dayLimit;
                type.LimitForOnce = onceLimit;
                entities.SaveChanges();
            }
        }

        [TestMethod]
        public void SendMessage_Broadcast_ExceedDayLimit() {
            MessageType type = entities.MessageType.Where(mt => mt.Id == (int)MessageKind.Broadcast).Single();
            int? onceLimit = type.LimitForOnce;
            int? dayLimit = type.LimitForDay;

            try {
                int msgCount = entities.Message.Count();

                type.LimitForDay = 10;
                type.LimitForOnce = null;
                entities.SaveChanges();

                DAL.Model.User owner = entities.GetUserQ(
                    where: q => q.Where(u => u.ChildUsers.Where(
                            ch => ch.Enabled
                            && ch.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any()).Count() >= 11),
                    create: u => {
                        for (int i = 0; i < 11; ++i) {
                            var nu = new DAL.Model.User() { Enabled = true };
                            nu.Transports.Add(new DAL.Model.Transport() {
                                TransportTypeId = (int)TransportKind.FLChat,
                                Enabled = true
                            });
                            u.ChildUsers.Add(nu);
                        }
                    },
                    transport: TransportKind.FLChat);
                DAL.Model.User[] users = owner.ChildUsers
                    .Where(u => u.Enabled 
                        && u.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any())
                    .ToArray();
                //entities.GetUsers(t.LimitForDay.Value + 2, u => u.Enabled, null,
                //    TransportKind.FLChat);

                SendMessageRequest request = new SendMessageRequest() {
                    Selection = new UserSelection() {
                        Include = users.Select(u => u.Id).ToList()
                    },
                    Text = "test message",
                    Type = type.Kind,
                };

                var e = Assert.ThrowsException<ErrorResponseException>(() => handler.ProcessRequestTransaction(entities, owner, request));
                Assert.AreEqual((int)HttpStatusCode.Forbidden, e.GetHttpCode());
                Assert.AreEqual(ErrorResponse.Kind.exceed_limit, e.Error.Error);
                Assert.IsInstanceOfType(e.Error, typeof(MessageLimitErrorResponse));
                MessageLimitErrorResponse me = e.Error as MessageLimitErrorResponse;
                Assert.IsNotNull(me.Limit);
                Assert.IsNotNull(me.Limit.ExceedDayLimit);
                Assert.IsNotNull(me.Limit.LimitForDay);
                Assert.AreEqual(users.Length, me.Limit.SelectionCount);

                Assert.AreEqual(msgCount, entities.Message.Count());
            } finally {
                type.LimitForDay = dayLimit;
                type.LimitForOnce = onceLimit;
                entities.SaveChanges();
            }
        }

        /// <summary>
        /// Successfull personal FLChat -> FLChat message 
        /// </summary>
        [TestMethod]
        public void SendMessage_NeedToChangeText()
        {
            DAL.Model.User to = entities.GetUser(
                u => u.Enabled
                    && u.Transports
                            .Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.FLChat).Any()
                    && u.Id != from.Id,
                u => {
                    u.Enabled = true;
                    u.Transports.Add(new DAL.Model.Transport()
                    {
                        Enabled = true,
                        Kind = TransportKind.FLChat
                    });
                });
            var hrtc = IMessageTextCompilerExtentions.CreateTagTextCompiler();


            SendMessageRequest request = new SendMessageRequest()
            {
                Text = "Text message #ФИО",
                ToUser = to.Id,
                Type = MessageKind.Personal,
                ToTransport = TransportKind.FLChat
            };

            {
                SendMessage handler1 = new SendMessage(null, new FakeMessageTextCompiler(
                    (s) => true, (mtu, s) => throw new NotImplementedException()));

                SendMessagePersonalResponse response = handler1.ProcessRequestTransaction(entities, from, request) as SendMessagePersonalResponse;

                DAL.Model.Message msg = entities
                    .Message
                    .Where(m => m.Id == response.MessageId)
                    .Include(m => m.ToUsers)
                    .FirstOrDefault();

                //check message
                Assert.IsNotNull(msg);
                Assert.IsTrue(msg.NeedToChangeText);
            }

            {
                SendMessage handler2 = new SendMessage(null, new FakeMessageTextCompiler(
                    (s) => false, (mtu, s) => throw new NotImplementedException()));

                SendMessagePersonalResponse response = handler2.ProcessRequestTransaction(entities, from, request) as SendMessagePersonalResponse;

                DAL.Model.Message msg = entities
                    .Message
                    .Where(m => m.Id == response.MessageId)
                    .Include(m => m.ToUsers)
                    .FirstOrDefault();

                //check message
                Assert.IsNotNull(msg);
                Assert.IsFalse(msg.NeedToChangeText);
            }
        }

        /// <summary>
        /// Successfull personal FLChat -> FLChat message 
        /// 31.01.2020 add DelayedStart
        /// </summary>
        [TestMethod]
        public void SendMessage_Personal_WithDelayedStart()
        {
            DAL.Model.User to = entities.GetUser(
                u => u.Enabled
                    && u.Transports
                            .Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.FLChat).Any()
                    && u.Id != from.Id,
                u => {
                    u.Enabled = true;
                    u.Transports.Add(new DAL.Model.Transport()
                    {
                        Enabled = true,
                        Kind = TransportKind.FLChat
                    });
                });

            SendMessageRequest request = new SendMessageRequest()
            {
                Text = "Text message",
                ToUser = to.Id,
                Type = MessageKind.Personal,
                DelayedStart = DateTime.UtcNow,
                ToTransport = TransportKind.FLChat
            };
            SendMessagePersonalResponse response = handler.ProcessRequestTransaction(entities, from, request) as SendMessagePersonalResponse;

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.MessageId);
            
            DAL.Model.Message msg = entities
                .Message
                .Where(m => m.Id == response.MessageId)
                .Include(m => m.ToUsers)
                .FirstOrDefault();

            //check message
            Assert.IsNotNull(msg);
            Assert.AreEqual(request.DelayedStart, msg.DelayedStart);
        }

        /// <summary>
        /// Successfull message to users only
        /// 31.01.2020 add DelayedStart
        /// </summary>
        [TestMethod]
        public void SendMessage_Broadcast_WithDelayedStart() {
            //  Users.
            int num = 2;
            TransportKind?[] transports = new TransportKind?[num];
            transports[0] = null;
            for (int j = 1; j < num; ++j) {
                transports[j] = TransportKind.FLChat;
            }
            DAL.Model.User[] uto = entities.GetUsers
                (num,
                u => u.Enabled
                    && u.Transports
                            .Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.FLChat).Any()
                    && u.Id != from.Id && u.OwnerUserId == from.Id,
                u => {
                    u.Enabled = true;
                    u.OwnerUserId = from.Id;
                    u.Transports.Add(new DAL.Model.Transport() {
                        Enabled = true,
                        Kind = TransportKind.FLChat
                    });
                });

            List<string> ToSegments = new List<string>();
            int i = 0;
            List<UserSendInfo> ToUsers = uto.Select(x => new UserSendInfo() { ToUser = x.Id, ToTransport = /*(TransportKind?)*/transports[i++] }).ToList();

            SendMessageRequest request = //MakeRequest();
            new SendMessageRequest() {
                Text = "Text Broadcast message3",
                ToUsers = ToUsers,
                //ToSegments = ToSegments,
                Type = MessageKind.Broadcast,
                //DelayedStart = DateTime.UtcNow,
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
            Assert.AreEqual(MessageKind.Broadcast, msg.Kind);
            Assert.AreEqual(request.DelayedStart, msg.DelayedStart);

        }

        [TestMethod]
        public void SendMessage_Mailing_WithDelayedStart() {
            DAL.Model.User user = entities.GetUser(
                u => u.OwnerUserId != null && u.OwnerUser.OwnerUserId != null
                    && u.OwnerUser.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.Email).Any()
                    && u.OwnerUser.OwnerUser.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any(),
                u => u.OwnerUser = entities.GetUser(
                    u2 => u2.OwnerUserId != null
                        && u2.OwnerUser.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any(),
                    u2 => u2.OwnerUser = entities.GetUser(null, null, TransportKind.FLChat),
                    TransportKind.Email),
                TransportKind.Email
                );
            Dictionary<Guid, TransportKind?> transports = entities.GetUserMailingTransport(new Guid[] { user.Id, user.OwnerUserId.Value });
            DAL.Model.User owner = user.OwnerUser.OwnerUser;

            SendMessageRequest request = new SendMessageRequest() {
                Selection = new UserSelection() {
                    IncludeWithStructure = new List<UserSelection.UserStructureSelection>() {
                        new UserSelection.UserStructureSelection() {
                            Type = UserSelection.SelectionType.Deep,
                            UserId = user.OwnerUserId.Value
                        }
                    }
                },
                Text = "test message",
                DelayedStart = DateTime.UtcNow,
                Type = MessageKind.Mailing,
            };

            SendMessageBroadcastResponse response = handler.ProcessRequestTransaction(entities, owner, request) as SendMessageBroadcastResponse;
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Users);

            DAL.Model.Message msg = entities
               .Message
               .Where(m => m.Id == response.MessageId)
               .Include(m => m.ToUsers)
               .FirstOrDefault();

            //check message
            Assert.IsNotNull(msg);
            Assert.AreEqual(request.DelayedStart, msg.DelayedStart);
        }
    }
}
