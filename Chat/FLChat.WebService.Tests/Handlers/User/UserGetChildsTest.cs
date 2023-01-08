using System;
using System.Linq;
using System.Net;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.WebService.DataTypes;
using System.Collections.Generic;

namespace FLChat.WebService.Handlers.User.Tests
{
    [TestClass]
    public class UserGetChildsTest
    {
        ChatEntities entities;
        GetUserChilds handler = new GetUserChilds() { MaxCount = int.MaxValue };

        DAL.Model.User user;
        DAL.Model.User[] users;

        [TestInitialize]
        public void Init()
        {
            entities = new ChatEntities();

            user = entities.GetUser(
                u => u.Enabled,
                u => {
                    u.Enabled = true;
                    u.Phone = "1234567890";
                    u.Email = "fltest@ya.ru";
                });
            var ownerUserId = user.Id;
            users = entities.GetUsers
                (2,
                u => u.Enabled && (u.OwnerUserId == ownerUserId),
                u => { u.OwnerUserId = ownerUserId; u.Enabled = true; }
                );
        }

        [TestCleanup]
        public void Clean()
        {
            entities.Dispose();
        }

        //[TestMethod]
        //public void TestChilds()
        //{
        //    var list1 = entities.User
        //       .Where(u => (u.OwnerUserId == user.Id)).OrderBy(z => z.Id).ToList();
        //    var list2 = user.ChildUsers.OrderBy(z => z.Id).ToList();
        //    CollectionAssert.AreEqual(list1, list2);
        //}

        [TestMethod]
        public void GetUserChilds_Constructor() {
            var handler = new GetUserChilds();
            Assert.IsFalse(handler.CalcalulateStructureCapacity);
            Assert.AreEqual(100, handler.MaxCount);
            Assert.IsTrue(handler.IsReusable);
        }

        [TestMethod]
        public void GetUserChildsForActiveGetTest()
        {
            string userId = user.Id.ToString();
            var userChilds = handler.ProcessRequest(entities, user, userId);
            //test count
            Assert.IsTrue(users.Count() <= userChilds.UserList.Count());
            //test items are equals
            var u3 = users.Select(k => k.Id);
            var u1 = userChilds.UserList.Select(c => c.Id)
                .Intersect(u3)
                .OrderBy(k => k)
                .ToArray();
            var u2 = users.Select(c => c.Id).OrderBy(k => k).ToArray();

            CollectionAssert.AreEqual(u1, u2);
            foreach (var v in u2)
            {
                Assert.IsTrue(u1.Contains(v));
            }
        }        

        [TestMethod]
        public void GetUserChilds_GetAllParamTest()
        {
            var user0 = entities.GetUser(
               u => u.OwnerUserId == user.Id,
               u => u.OwnerUserId = user.Id,
               enabled: false);

            string userId = user.Id.ToString();

            Assert.IsTrue( new GetUserChilds(true).ProcessRequest(entities, user, userId).UserList.Where(i => i.Id == user0.Id).Any());
            Assert.IsFalse(new GetUserChilds(false).ProcessRequest(entities, user, userId).UserList.Where(i => i.Id == user0.Id).Any());
        }

        [TestMethod]
        public void GetUserChildsForNotActiveNotGetTest()
        {
            var user0 = entities.GetUser(null, null, enabled: false);

            try
            {
                string userId = user0.Id.ToString();
                var userChilds = handler.ProcessRequest(entities, user, userId);
                Assert.Fail("Not throwing exception on creation profile for non active user ");
            }
            catch (ErrorResponseException e)
            {
                Assert.AreEqual((int)HttpStatusCode.NotFound, e.GetHttpCode());
            }
        }

        [TestMethod]
        public void GetProfileChildsForActiveGetTest()
        {            
            var userChilds = handler.ProcessRequest(entities, user, (string)null);
            //test count            
            Assert.IsTrue(users.Count() <= userChilds.UserList.Count());
            //test items are equals
            var u1 = userChilds.UserList.Select(c => c.Id)
                .OrderBy(k => k)
                .ToArray();
            var u2 = users.Select(c => c.Id).OrderBy(k => k).ToArray();
            foreach (var v in u2)
            {
                Assert.IsTrue(u1.Contains(v));
            }
        }

        [TestMethod]
        public void GetUserChilds_LastMessage() {
            DAL.Model.User user = entities.GetUserWithOwner();
            entities.SendMessage(user.Id, user.OwnerUserId.Value);
            Guid msgid = entities.LastMessageView
                .Where(m => m.UserId == user.Id && m.UserOppId == user.OwnerUserId.Value)
                .Select(m => m.MsgId).Single();

            UserChildResponse resp = handler.ProcessRequest(entities, user.OwnerUser, user.OwnerUserId.Value.ToString());

            //check message in child
            UserInfoShort info = resp.UserList.Where(u => u.Id == user.Id).Single();
            Assert.IsNotNull(info.LastMessage);
            Assert.AreEqual(msgid, info.LastMessage.Id);

            //check message in user
            resp = handler.ProcessRequest(entities, user.OwnerUser, user.Id.ToString());
            Assert.AreEqual(user.Id, resp.User.Id);
            Assert.IsNotNull(resp.User.LastMessage);
            Assert.AreEqual(msgid, resp.User.LastMessage.Id);
        }

        [TestMethod]
        public void GetUserChilds_Partial() {
            DAL.Model.User user = entities.GetUser(
                u => u.ChildUsers.Where(c => c.Enabled).Count() >= 5,
                u => u.ChildUsers = entities.GetUsers(5, c => c.Id != u.Id, null));
            PartialDataIdRequest input = new PartialDataIdRequest() {
                Count = 2
            };

            UserChildResponse resp = handler.ProcessRequest(entities, user, input);
            Assert.AreEqual(user.Id, resp.User.Id);
            Assert.AreEqual(input.Count, resp.UserList.Count());
            Assert.AreEqual(handler.MaxCount, resp.MaxCount);
            Assert.AreEqual(0, resp.Offset);
            Assert.AreEqual(input.Count, resp.RequestedCount);
            Assert.IsNotNull(resp.TotalCount);
            Assert.AreNotEqual(0, resp.TotalCount);
            Guid[] firstIds = resp.UserList.Select(u => u.Id).ToArray();

            input = new PartialDataIdRequest() {
                Count = 2,
                Offset = 2,
            };
            resp = handler.ProcessRequest(entities, user, input);
            Assert.AreEqual(user.Id, resp.User.Id);
            Assert.AreEqual(input.Count, resp.UserList.Count());
            Assert.AreEqual(handler.MaxCount, resp.MaxCount);
            Assert.AreEqual(input.Offset, resp.Offset);
            Assert.AreEqual(input.Count, resp.RequestedCount);
            Assert.IsNull(resp.TotalCount);
            Guid[] secondIds = resp.UserList.Select(u => u.Id).ToArray();

            Assert.AreEqual(0, firstIds.Intersect(secondIds).Count());
        }

        [TestMethod]
        public void GetUserChilds_ProfileAndUser() {
            DAL.Model.User []users = entities.GetUsers(2, null, null);
            UserChildResponse resp;

            resp = handler.ProcessRequest(entities, users[0], new PartialDataIdRequest() { Ids = users[1].Id.ToString() });
            Assert.AreEqual(users[1].Id, resp.User.Id);

            resp = handler.ProcessRequest(entities, users[0], new PartialDataIdRequest() { });
            Assert.AreEqual(users[0].Id, resp.User.Id);
        }

        [TestMethod]
        public void GetUserChilds_TotalChildCount() {
            DAL.Model.User another = entities.GetUser(u => u.Id != user.Id, null);
            var handler = new GetUserChilds() { CalcalulateStructureCapacity = true };
            Assert.IsNotNull(handler.ProcessRequest(entities, user, (string)null).TotalChildsCount);
            Assert.IsNotNull(handler.ProcessRequest(entities, user, user.Id.ToString()).TotalChildsCount);
            Assert.IsNull(handler.ProcessRequest(entities, user, another.Id.ToString()).TotalChildsCount);

            handler = new GetUserChilds();
            Assert.IsNull(handler.ProcessRequest(entities, user, (string)null).TotalChildsCount);
            Assert.IsNull(handler.ProcessRequest(entities, user, user.Id.ToString()).TotalChildsCount);
            Assert.IsNull(handler.ProcessRequest(entities, user, another.Id.ToString()).TotalChildsCount);
        }

        [TestMethod]
        public void GetUserChilds_HasChilds_True() {
            DAL.Model.User user = entities.GetUser(
                u => u.OwnerUserId != null && u.ChildUsers.Any(),
                u => {
                    u.OwnerUser = entities.GetUser(null, null);
                    u.ChildUsers.Add(new DAL.Model.User());
                });
            UserChildResponse resp = handler.ProcessRequest(entities, user.OwnerUser, user.OwnerUser.Id.ToString());
            UserInfoShort ui = resp.UserList.Where(u => u.Id == user.Id).Single();
            Assert.IsNotNull(ui.HasChilds);
            Assert.IsTrue(ui.HasChilds.Value);
        }

        [TestMethod]
        public void GetUserChilds_HasChilds_False() {
            DAL.Model.User user = entities.GetUser(
                u => u.OwnerUserId != null && u.ChildUsers.Any() == false,
                u => {
                    u.OwnerUser = entities.GetUser(null, null);
                });
            UserChildResponse resp = handler.ProcessRequest(entities, user.OwnerUser, user.OwnerUser.Id.ToString());
            UserInfoShort ui = resp.UserList.Where(u => u.Id == user.Id).Single();
            Assert.IsNotNull(ui.HasChilds);
            Assert.IsFalse(ui.HasChilds.Value);
        }

        [TestMethod]
        public void GetUserChilds_Tags() {
            Segment segment = entities.GetSegment(
                s => s.IsDeleted == false && s.ShowInShortProfile == true,
                s => {
                    s.ShowInShortProfile = true;
                });

            DAL.Model.User user = entities.GetUser(
                u => u.Segments.Contains(segment) && u.OwnerUserId != null,
                u => {
                    u.Segments.Add(segment);
                    u.OwnerUser = entities.GetUser(null, null);
                });

            var resp = handler.ProcessRequest(entities, user.OwnerUser, (string)null);
            UserInfoShort ui = resp.UserList.Where(u => u.Id == user.Id).Single();
            Assert.IsNotNull(ui.Tags);
            Assert.IsTrue(ui.Tags.Contains(segment.Name));
        }

        [TestMethod]
        public void GetUserChilds_TagsEmpty() {
            Segment segment = entities.GetSegment(
                s => s.IsDeleted == false && s.ShowInShortProfile == true,
                s => {
                    s.ShowInShortProfile = true;
                });

            DAL.Model.User user = entities.GetUser(
                u => u.Segments.Contains(segment) == false && u.OwnerUserId != null,
                u => {
                    u.Segments.Add(segment);
                    u.OwnerUser = entities.GetUser(null, null);
                });
            var resp = handler.ProcessRequest(entities, user.OwnerUser, (string)null);
            UserInfoShort ui = resp.UserList.Where(u => u.Id == user.Id).Single();
            Assert.IsNull(ui.Tags);
        }

        [TestMethod]
        public void GetUserChilds_BroadcastProhibitionStructure() {
            //get user: 1. who has owner and childs
            //          2. owner set broadcast prohibition for this users
            DAL.Model.User user = entities.GetUserQ(
                q => q.Where(u => 
                    u.OwnerUserId != null 
                    && u.ChildUsers.Where(u3 => u3.Enabled).Any() 
                    && u.BroadcastProhibitionBy.Contains(u.OwnerUser)),
                u => {
                    DAL.Model.User[] users = entities.GetUsers(3, u2 => u2.OwnerUserId == null, null);
                    u.OwnerUserId = users[0].Id;
                    u.ChildUsers.Add(users[1]);
                    u.ChildUsers.Add(users[2]);
                    u.BroadcastProhibitionBy.Add(users[0]);
                }
                );
            //owner reqwuest user's childs, they all must has flag <BroadcastProhibitionStructure>
            var resp = handler.ProcessRequest(entities, user.OwnerUser, user.Id.ToString());

            bool?[] flags = resp.UserList.Select(u => u.BroadcastProhibitionStructure).Distinct().ToArray();
            Assert.AreEqual(1, flags.Length);
            Assert.IsTrue(flags[0].Value);

            Assert.IsFalse(resp.User.BroadcastProhibitionStructure.Value);
            Assert.IsTrue(resp.User.BroadcastProhibition);
        }

        [TestMethod]
        public void GetUserChilds_BroadcastProhibitionStructure_false() {
            //get user: 1. who has owner and childs
            //          2. owner has not set broadcast prohibition for this users
            DAL.Model.User user = entities.GetUserQ(
                q => q.Where(u =>
                    u.OwnerUserId != null
                    && u.ChildUsers.Where(u3 => u3.Enabled).Any()
                    && u.BroadcastProhibitionBy.Contains(u.OwnerUser) == false),
                u => {
                    DAL.Model.User[] users = entities.GetUsers(3, u2 => u2.OwnerUserId == null, null);
                    u.OwnerUserId = users[0].Id;
                    u.ChildUsers.Add(users[1]);
                    u.ChildUsers.Add(users[2]);
                    //u.BroadcastProhibitionBy.Add(users[0]);
                }
                );
            //owner reqwuest user's childs, they all must has flag <BroadcastProhibitionStructure>
            var resp = handler.ProcessRequest(entities, user.OwnerUser, user.Id.ToString());

            bool?[] flags = resp.UserList.Select(u => u.BroadcastProhibitionStructure).Distinct().ToArray();
            Assert.AreEqual(1, flags.Length);
            Assert.IsFalse(flags[0].Value);

            Assert.IsFalse(resp.User.BroadcastProhibitionStructure.Value);
            Assert.IsFalse(resp.User.BroadcastProhibition);
        }

        [TestMethod]
        public void GetUserChilds_BroadcastProhibition() {
            //get user: 1. who has owner and childs
            //          2. owner set broadcast prohibition for one of his childs
            //          3. owner has not set broadcast prohibition for one of his childs
            DAL.Model.User user = entities.GetUserQ(
                q => q.Where(u =>
                    u.OwnerUserId != null
                    && u.ChildUsers.Where(u3 => u3.Enabled && u3.BroadcastProhibitionBy.Contains(u.OwnerUser)).Any()
                    && u.ChildUsers.Where(u3 => u3.Enabled && u3.BroadcastProhibitionBy.Contains(u.OwnerUser) == false).Any()),
                u => {
                    DAL.Model.User[] users = entities.GetUsers(3, u2 => u2.OwnerUserId == null, null);
                    u.OwnerUserId = users[0].Id;
                    u.ChildUsers.Add(users[1]);
                    u.ChildUsers.Add(users[2]);
                    users[1].BroadcastProhibitionBy.Add(users[0]);
                }
                );
            DAL.Model.User bpu = user.ChildUsers.Where(u => u.BroadcastProhibitionBy.Contains(user.OwnerUser)).First();
            DAL.Model.User bpu_wo = user.ChildUsers.Where(u => u.BroadcastProhibitionBy.Contains(user.OwnerUser) == false).First();

            var resp = handler.ProcessRequest(entities, user.OwnerUser, user.Id.ToString());

            Assert.IsTrue(resp.UserList.Where(u => u.Id == bpu.Id).Single().BroadcastProhibition);
            Assert.IsFalse(resp.UserList.Where(u => u.Id == bpu_wo.Id).Single().BroadcastProhibition);
        }   
        
        [TestMethod]
        public void GetUserChilds_UserHasChilds() {
            DAL.Model.User user = entities.GetUserQ(
                q => q.Where(u => u.ChildUsers.Where(uc => uc.Enabled).Any()),
                u => u.ChildUsers.Add(entities.GetUserQ(null, null)));
            var resp = handler.ProcessRequest(entities, user, user.Id.ToString());
            Assert.IsTrue(resp.User.HasChilds.Value);

            user = entities.GetUserQ(
                q => q.Where(u => u.ChildUsers.Where(uc => uc.Enabled).Any() == false),
                null);
            resp = handler.ProcessRequest(entities, user, user.Id.ToString());
            Assert.IsFalse(resp.User.HasChilds.Value);
        }
    }
}
