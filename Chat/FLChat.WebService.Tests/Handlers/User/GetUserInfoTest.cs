using System;
using System.Linq;
using System.Net;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using FLChat.WebService.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.Handlers.User.Tests
{
    [TestClass]
    public class GetUserInfoTest
    {
        ChatEntities entities;
        GetUserInfo handler = new GetUserInfo();

        DAL.Model.User user;

        [TestInitialize]
        public void Init()
        {
            entities = new ChatEntities();        

            user = entities.GetUser(
                u => u.Enabled && u.Transports.Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.FLChat).Any(),
                u => {
                    u.Enabled = true;
                    u.Phone = "1234567890";
                    u.Email = "fltest@ya.ru";
                    u.Transports.Add(new DAL.Model.Transport()
                    {
                        TransportTypeId = (int)TransportKind.FLChat                        
                    });
                });
        }

        [TestCleanup]
        public void Clean()
        {
            entities.Dispose();
        }

        [TestMethod]
        public void GetUserInfoForActiveTest()
        {
            string userId = user.Id.ToString();
            var getUserInfo = new GetUserInfo();
            UserProfileInfo userPI = getUserInfo.ProcessRequest(entities, user, userId);
            Assert.AreEqual(userPI.Id, user.Id);
            Assert.AreEqual(userPI.Phone, user.Phone);
            Assert.AreEqual(userPI.Email, user.Email);
            var list = user.AvailableTransports;
            //test count            
            Assert.AreEqual(user.AvailableTransports.Count(), userPI.Transports.Count());
        }

        [TestMethod]
        public void GetUserInfoForNotActiveGetTest()
        {
            var user0 = entities.GetUser(null, null, enabled: false);
            string userId = user0.Id.ToString();
            var getUserInfo = new GetUserInfo(true);
            UserProfileInfo userPI1 = getUserInfo.ProcessRequest(entities, user0, userId);
            Assert.AreEqual(user0.Id, userPI1.Id);
            //Assert.AreEqual(userPI1. .Id, userPI2.Id);
        }

        [TestMethod]
        public void GetUserInfoForNotActiveNotGetTest()
        {
            var user0 = entities.GetUser(null, null, enabled: false);

            try
            {
                string userId = user0.Id.ToString();
                var getUserInfo = new GetUserInfo(false);
                UserProfileInfo userPI1 = getUserInfo.ProcessRequest(entities, user0, userId);
                Assert.Fail("Not throwing exception on creation profile for non active user - 1");
            }
            catch (ErrorResponseException e)
            {
                Assert.AreEqual((int)HttpStatusCode.NotFound, e.GetHttpCode());
            }

            try
            {                
                UserProfileInfo userPI2 = handler.ProcessRequest(entities, user0, null);
                Assert.Fail("Not throwing exception on creation profile for non active user - 2");
            }
            catch (ErrorResponseException e)
            {
                Assert.AreEqual((int)HttpStatusCode.NotFound, e.GetHttpCode());
            }
        }
        
        [TestMethod]
        public void GetUserInfo_Comment() {
            DAL.Model.User user = entities.GetUser(
                u => u.Enabled && u.CommentsOnMe.Where(c => c.User.Enabled && c.Text != "").Any(),
                u => u.CommentsOnMe = new Comment[] {
                    new Comment() {
                        UserId = entities.GetUser(u2 => u2.Enabled && u2.Id != u.Id, null).Id,
                        Text = "some text"
                    }
                });
            Comment cmt = user.CommentsOnMe.Where(c => c.UponUser.Enabled && c.User.Enabled && c.Text != "").First();

            UserProfileInfo info = handler.ProcessRequest(entities, cmt.User, user.Id.ToString());
            Assert.IsNotNull(info.Comment);
            Assert.IsFalse(String.IsNullOrEmpty(info.Comment));
            Assert.AreEqual(cmt.Text, info.Comment);

            //get user who has't comment on user
            DAL.Model.User user2 = entities.GetUser(
                u => u.Enabled && u.Comments.Where(c => c.UponUserId == user.Id).Any() == false,
                null);
            UserProfileInfo info2 = handler.ProcessRequest(entities, user2, user.Id.ToString());
            Assert.IsNotNull(info2.Comment);
            Assert.IsTrue(String.IsNullOrEmpty(info2.Comment));
        }

        [TestMethod]
        public void GetUserInfo_CurrentUserComment() {
            DAL.Model.User user = entities.GetUser(u => u.Enabled, null);

            UserProfileInfo info = handler.ProcessRequest(entities, user, user.Id.ToString());
            Assert.IsNull(info.Comment);
        }

        [TestMethod]
        public void GetUserInfo_BroadcastProhibition_true() {
            DAL.Model.User user = entities.GetUser(u => u.BroadcastProhibitionBy.Any(),
                u => u.BroadcastProhibitionBy.Add(entities.GetUser(null, null))
                );
            DAL.Model.User owner = user.BroadcastProhibitionBy.First();
            UserProfileInfo info = handler.ProcessRequest(entities, owner, user.Id.ToString());
            Assert.IsTrue(info.BroadcastProhibition);
            info = handler.ProcessRequest(entities, owner, owner.Id.ToString());
            Assert.IsTrue(info.BroadcastProhibitionList.Select(x => x.Id).Contains(user.Id));

            //get any user, who has not restrict broadcast message for user structure
            DAL.Model.User another = entities.GetUser(
                u => u.BroadcastProhibition.Select(bp => bp.Id).Contains(user.Id) == false,
                null                
                );
            info = handler.ProcessRequest(entities, another, user.Id.ToString());
            Assert.IsFalse(info.BroadcastProhibition);
            info = handler.ProcessRequest(entities, another, another.Id.ToString());
            Assert.IsFalse(info.BroadcastProhibitionList.Select(x => x.Id).Contains(user.Id));
        }

        [TestMethod]
        public void GetUserInfo_BroadcastProhibition_false() {
            DAL.Model.User user = entities.GetUser(u => u.BroadcastProhibitionBy.Any() == false,
                null
                );
            DAL.Model.User owner = entities.GetUser(u => u.Id != user.Id, null);
            UserProfileInfo info = handler.ProcessRequest(entities, owner, user.Id.ToString());
            Assert.IsFalse(info.BroadcastProhibition);
            info = handler.ProcessRequest(entities, owner, owner.Id.ToString());
            Assert.IsFalse(info.BroadcastProhibitionList.Select(x => x.Id).Contains(user.Id));
        }


        [TestMethod]
        public void GetUserInfo_PersonalProhibition_true()
        {
            DAL.Model.User user = entities.GetUser(u => u.PersonalProhibitionMain.Any(),
                u => u.PersonalProhibitionMain.Add(entities.GetUser(null, null))
                );
            DAL.Model.User owner = user.PersonalProhibitionMain.First();
            UserProfileInfo info = handler.ProcessRequest(entities, owner, user.Id.ToString());
            Assert.IsTrue(info.PersonalProhibition);
            info = handler.ProcessRequest(entities, owner, owner.Id.ToString());
            Assert.IsTrue(info.PersonalProhibitionList.Select(x => x.Id).Contains(user.Id));

            //get any user, who has not restrict broadcast message for user structure
            DAL.Model.User another = entities.GetUser(
                u => u.PersonalProhibitionMain.Select(bp => bp.Id).Contains(user.Id) == false,
                null
                );
            info = handler.ProcessRequest(entities, another, user.Id.ToString());
            Assert.IsFalse(info.PersonalProhibition);
            info = handler.ProcessRequest(entities, another, another.Id.ToString());
            Assert.IsFalse(info.PersonalProhibitionList.Select(x => x.Id).Contains(user.Id));
        }

        [TestMethod]
        public void GetUserInfo_PersonalProhibition_false()
        {
            DAL.Model.User user = entities.GetUser(u => u.PersonalProhibitionMain.Any() == false,
                null
                );
            DAL.Model.User owner = entities.GetUser(u => u.Id != user.Id, null);
            UserProfileInfo info = handler.ProcessRequest(entities, owner, user.Id.ToString());
            Assert.IsFalse(info.PersonalProhibition);
            info = handler.ProcessRequest(entities, owner, owner.Id.ToString());
            Assert.IsFalse(info.PersonalProhibitionList.Select(x => x.Id).Contains(user.Id));
        }

        [TestMethod]
        public void GetUserInfo_HasChilds() {
            DAL.Model.User user = entities.GetUserQ(
                q => q.Where(u => u.OwnerUserId != null),
                u => u.OwnerUserId = entities.GetUserQ(null, null).Id);
            UserProfileInfo info = handler.ProcessRequest(entities, user.OwnerUser, user.OwnerUserId.ToString());
            Assert.IsTrue(info.HasChilds.Value);
        }

        [TestMethod]
        public void GetUserInfo_HasChilds_false() {
            DAL.Model.User user = entities.GetUserQ(
                q => q.Where(u => u.ChildUsers.Any() == false),
                null);
            UserProfileInfo info = handler.ProcessRequest(entities, user, user.Id.ToString());
            Assert.IsFalse(info.HasChilds.Value);
        }

        [TestMethod]
        public void GetUserInfo_ChildParentDepth() {
            DAL.Model.User user = entities.GetUserQ(
                where: q => q.Where(u => u.OwnerUserId != null && u.ChildUsers.Any()),
                create: u => {
                    u.OwnerUser = entities.GetUserQ();
                    u.ChildUsers.Add(entities.GetUserQ(where: q => q.Where(u2 => u2.Id != u.OwnerUser.Id)));
                }
                );
            UserProfileInfo pi;

            //owner request his child
            pi = handler.ProcessRequest(entities, user.OwnerUser, user.Id.ToString());
            Assert.AreEqual(1, pi.ChildDepth);
            Assert.IsNull(pi.ParentDepth);

            //child request his owner
            pi = handler.ProcessRequest(entities, user.ChildUsers.First(), user.Id.ToString());
            Assert.IsNull(pi.ChildDepth);
            Assert.AreEqual(1, pi.ParentDepth);

            //owner request child of child
            pi = handler.ProcessRequest(entities, user.OwnerUser, user.ChildUsers.First().Id.ToString());
            Assert.AreEqual(2, pi.ChildDepth);
            Assert.IsNull(pi.ParentDepth);

            //child request his parent of parent
            pi = handler.ProcessRequest(entities, user.ChildUsers.First(), user.OwnerUserId.ToString());
            Assert.IsNull(pi.ChildDepth);
            Assert.AreEqual(2, pi.ParentDepth);

            //requst own profile
            pi = handler.ProcessRequest(entities, user, null);
            Assert.IsNull(pi.ChildDepth);
            Assert.IsNull(pi.ParentDepth);

            //requst own profile
            pi = handler.ProcessRequest(entities, user, user.Id.ToString());
            Assert.IsNull(pi.ChildDepth);
            Assert.IsNull(pi.ParentDepth);
        }

        [TestMethod]
        public void GetUserInfo_IsMe() {
            DAL.Model.User user = entities.GetUserQ();
            DAL.Model.User user2 = entities.GetUserQ(where: q => q.Where(u => u.Id != user.Id));
            UserProfileInfo pi;

            //owner request his child
            pi = handler.ProcessRequest(entities, user, null);
            Assert.IsTrue(pi.IsMe.Value);

            pi = handler.ProcessRequest(entities, user, user.Id.ToString());
            Assert.IsTrue(pi.IsMe.Value);

            pi = handler.ProcessRequest(entities, user, user2.Id.ToString());
            Assert.IsFalse(pi.IsMe.Value);
        }

        [TestMethod]
        public void InviteLink() {
            DAL.Model.User user = entities.GetUserQ();
            UserProfileInfo pi = handler.ProcessRequest(entities, user, null);
            Assert.IsNotNull(pi.InviteLink);
            Assert.IsNotNull(pi.InviteLink.Code);
            Assert.IsNotNull(pi.InviteLink.Url);
            Assert.IsNotNull(pi.InviteLink.InviteButtons);
        }
    }
}
