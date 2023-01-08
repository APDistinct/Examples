using System;
using System.Collections.Generic;
using System.Linq;
using FLChat.DAL.Model;
using FLChat.WebService.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FLChat.WebService.DataTypes;
using FLChat.DAL;

namespace FLChat.WebService.Handlers.Segments.Tests
{
    [TestClass]
    public class SegmentTests
    {
        ChatEntities entities;

        [TestInitialize]
        public void Init()
        {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Clean()
        {
            entities.Dispose();
        }

        [TestMethod]
        public void Segment_GetMembersForUserGetAll() {
            DAL.Model.User userToFind = entities.GetUser(
                u => u.OwnerUserId != null && u.Enabled && u.Segments.Where(s => s.IsDeleted == false).Any(),
                u => {
                    u.OwnerUserId = entities.GetUser(up => true, null).Id;
                    u.Segments.Add(entities.GetSegment(s => s.IsDeleted == false, s => s.IsDeleted = false));
                });
            DAL.Model.User userOwner = entities.User.Where(u => u.Id == userToFind.OwnerUserId).FirstOrDefault();
            DAL.Model.Segment segment = userToFind.Segments.Where(s => s.IsDeleted == false).FirstOrDefault();
            List<DAL.Model.User> users = segment.GetMembersForUser(entities, userOwner.Id);
            //bool ret = users.Select(x => x.Id).Contains(userToFind.Id);
            //var u0 = users[0];
            Assert.IsTrue(users.Select(x => x.Id).Contains(userToFind.Id));
        }

        [TestMethod]
        public void Segment_GetMembersForUserNotGetByUser()
        {
            DAL.Model.User userToFind = entities.GetUser(
                u => u.OwnerUserId != null && u.Enabled && u.Segments.Where(s => s.IsDeleted == false).Any(),
                u => {
                    u.OwnerUserId = entities.GetUser(up => true, null).Id;
                    u.Segments.Add(entities.GetSegment(s => s.IsDeleted == false, s => s.IsDeleted = false));
                });
            DAL.Model.User userOwner = entities.User.Where(u => u.Id == userToFind.OwnerUserId).FirstOrDefault();
            DAL.Model.Segment segment = userToFind.Segments.Where(s => s.IsDeleted == false).FirstOrDefault();
            userToFind.OwnerUserId = null;
            entities.SaveChanges();
            List<DAL.Model.User> users = segment.GetMembersForUser(entities, userOwner.Id);            
            Assert.IsFalse(users.Select(x => x.Id).Contains(userToFind.Id));
        }

        [TestMethod]
        public void Segment_GetMembersForUserNotGetBySegment()
        {
            DAL.Model.User userToFind = entities.GetUser(
                u => u.OwnerUserId != null && u.Enabled && u.Segments.Where(s => s.IsDeleted == false).Any(),
                u => {
                    u.OwnerUserId = entities.GetUser(up => true, null).Id;
                    u.Segments.Add(entities.GetSegment(s => s.IsDeleted == false, s => s.IsDeleted = false));
                });
            DAL.Model.User userOwner = entities.User.Where(u => u.Id == userToFind.OwnerUserId).FirstOrDefault();
            DAL.Model.Segment segment = userToFind.Segments.Where(s => s.IsDeleted == false).FirstOrDefault();
            userToFind.Segments.Remove(segment);
            entities.SaveChanges();
            List<DAL.Model.User> users = segment.GetMembersForUser(entities, userOwner.Id);            
            Assert.IsFalse(users.Select(x => x.Id).Contains(userToFind.Id));
        }

        [TestMethod]
        public void GetSegmentsAllWith0Test() {
            string json = @"{ include_empty : 'true' }";

            JObject input = JObject.Parse(json);
            DAL.Model.User userToFind = entities.GetUser(
                u => u.OwnerUserId != null && u.Enabled && u.Segments.Where(s => s.IsDeleted == false).Any(),
                u => {
                    u.OwnerUserId = entities.GetUser(up => true, null).Id;
                    u.Segments.Add(entities.GetSegment(s => s.IsDeleted == false, s => s.IsDeleted = false));
                });
            DAL.Model.User userOwner = entities.User.Where(u => u.Id == userToFind.OwnerUserId).FirstOrDefault();
            DAL.Model.Segment segmentYes = userToFind.Segments.Where(s => s.IsDeleted == false).FirstOrDefault();
            DAL.Model.Segment segmentNo = entities.GetSegment(s => s.IsDeleted == false && !s.Members.Any(), s => s.IsDeleted = false);
            var gsa = new GetSegmentsAll();
            var segcr = gsa.ProcessRequest(entities, userOwner, input);

            var sYes = segcr.Segments.Where(x => x.Name == segmentYes.Name).FirstOrDefault();
            var sNo = segcr.Segments.Where(x => x.Name == segmentNo.Name).FirstOrDefault();
            Assert.IsNotNull(sYes);
            Assert.IsNotNull(sNo);
            Assert.IsTrue(sYes.Count > 0);
            Assert.IsTrue(sNo.Count == 0);
        }

        [TestMethod]
        public void GetSegmentsAllWithout0Test() {
            string json = @"{ include_empty : 'false' }";

            JObject input = JObject.Parse(json);
            DAL.Model.User userToFind = entities.GetUser(
                u => u.OwnerUserId != null && u.Enabled && u.Segments.Where(s => s.IsDeleted == false).Any(),
                u => {
                    u.OwnerUserId = entities.GetUser(up => true, null).Id;
                    u.Segments.Add(entities.GetSegment(s => s.IsDeleted == false, s => s.IsDeleted = false));
                });
            DAL.Model.User userOwner = entities.User.Where(u => u.Id == userToFind.OwnerUserId).FirstOrDefault();
            DAL.Model.Segment segmentYes = userToFind.Segments.Where(s => s.IsDeleted == false).FirstOrDefault();
            DAL.Model.Segment segmentNo = entities.GetSegment(s => s.IsDeleted == false && !s.Members.Any(), s => s.IsDeleted = false);
            var gsa = new GetSegmentsAll();
            var segcr = gsa.ProcessRequest(entities, userOwner, input);

            var sYes = segcr.Segments.Where(x => x.Name == segmentYes.Name).FirstOrDefault();
            var sNo = segcr.Segments.Where(x => x.Name == segmentNo.Name).FirstOrDefault();
            Assert.IsNotNull(sYes);
            Assert.IsNull(sNo);
            Assert.IsTrue(sYes.Count > 0);
            //Assert.IsTrue(sNo.Count == 0);
        }

        [TestMethod]
        public void GetSegment_LastMessage() {
            DAL.Model.User child = entities.GetUser(
                u => u.Enabled && u.OwnerUserId != null 
                    && u.OwnerUser.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any()
                    && u.Segments.Where(s => s.IsDeleted == false).Any(),
                u => {
                    u.Enabled = true;
                    u.OwnerUserId = entities.GetUser(uo => uo.Enabled && uo.Id != u.Id, null, DAL.TransportKind.FLChat).Id;
                    u.Segments = new DAL.Model.Segment[] { entities.GetSegment(s => s.IsDeleted == false, s => { }) };
                    },
                DAL.TransportKind.FLChat);
            Guid msgid = entities.SendMessage(child.OwnerUserId.Value, child.Id).Id;

            GetSegment handler = new GetSegment();
            SegmentInfoResponse resp =  handler.ProcessRequest(entities, child.OwnerUser, child.Segments.First().Name);
            UserInfoShort info = resp.UserList.Where(u => u.Id == child.Id).Single();

            Assert.IsNotNull(info.LastMessage);
            Assert.AreEqual(msgid, info.LastMessage.Id);
        }


        [TestMethod]
        public void ManageSegmentTest()
        {
            DAL.Model.User user = entities.GetUser(u => u.Enabled, null);                
            var segment = entities.GetSegment(s => s.IsDeleted == false, s => { });
            DAL.Model.User[] userIn = entities.GetUsers(2,
                    u => u.Enabled && segment.Members.Contains(u), u => { u.Enabled = true; segment.Members.Add(u); });
            DAL.Model.User[] userOut = entities.GetUsers(2,
                    u => u.Enabled && !segment.Members.Contains(u), u =>  u.Enabled = true );

            List<Guid>  listAdd = new List<Guid>();
            List<Guid> listRemove = new List<Guid>();
            listAdd.Add(userIn[0].Id);
            listAdd.Add(userOut[0].Id);
            listRemove.Add(userIn[1].Id);
            listRemove.Add(userIn[1].Id);
            SegmentManageRequest smr = new SegmentManageRequest() { Add = listAdd, Remove = listRemove };
            string json = JsonConvert.SerializeObject(smr);
            json = @"{ '" + ManageSegment.Key + "' : '" + segment.Id.ToString() + "', " + (json).Substring(1, json.Length-1);

            ManageSegment handler = new ManageSegment();
            var resp = handler.ProcessRequest(entities, user, JObject.Parse( json));
            Assert.IsTrue(segment.Members.Contains(userIn[0]));
            Assert.IsTrue(segment.Members.Contains(userOut[0]));
            Assert.IsFalse(segment.Members.Contains(userIn[1]));
            Assert.IsFalse(segment.Members.Contains(userOut[1]));
        }

        /// <summary>
        /// Поиск в сегменте всех. Отдельно поиск по шаблону, смещение и количество работает по стандартному варианту.
        /// </summary>
        [TestMethod]        
        public void GetSegmentDBTest()
        {
            DAL.Model.User user = entities.GetUser(u => u.Enabled, null);
            var segment = entities.GetSegment(s => s.IsDeleted == false, s => { });
            DAL.Model.User[] userIn = entities.GetUsers(2,
                    u => u.Enabled && segment.Members.Contains(u), u => { u.Enabled = true; segment.Members.Add(u); });
            DAL.Model.User[] userOut = entities.GetUsers(2,
                    u => !u.Enabled && segment.Members.Contains(u), u => { u.Enabled = false; segment.Members.Add(u); });
            
            string json = segment.Name;                

            GetSegmentDB handler = new GetSegmentDB();
            SegmentDBRequest segmentDBRequest = new SegmentDBRequest() { Segment = json };
            var resp = handler.ProcessRequest(entities, user, segmentDBRequest);

            var usersBefour = segment.Members.Select(x => x.Id).ToList();
            var usersAfter = resp.UserList.Select(x => x.Id).ToList();

            Assert.IsTrue(usersBefour.Count == usersAfter.Count);
            Assert.IsTrue(segment.Members.Count == resp.TotalCount);
            CollectionAssert.AreEquivalent(usersAfter, usersBefour);            
        }
    }
}
