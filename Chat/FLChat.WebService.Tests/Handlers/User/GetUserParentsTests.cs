using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using FLChat.DAL;

namespace FLChat.WebService.Handlers.User.Tests
{
    [TestClass]
    public class GetUserParentsTests
    {
        ChatEntities entities;
        DAL.Model.User child;
        GetUserParents handler = new GetUserParents();

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
            child = entities.GetUser(
                u => u.OwnerUserId != null && u.OwnerUser.OwnerUserId != null
                    && u.OwnerUser.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any()
                    && u.OwnerUser.OwnerUser.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any(),
                u => u.OwnerUserId = entities.GetUser(
                    u1 => u1.OwnerUserId != null 
                        && u1.OwnerUser.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any(),
                    u1 => u1.OwnerUserId = entities.GetUser(null, null, DAL.TransportKind.FLChat).Id,
                    DAL.TransportKind.FLChat
                    ).Id,
                DAL.TransportKind.FLChat
                );
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void GetUserParents() {            
            UserParentResponse resp = handler.ProcessRequest(entities, child, child.Id.ToString());
            Assert.IsNotNull(resp);
            Assert.IsNotNull(resp.UserList);
            Guid[] guids = resp.UserList.Select(u => u.Id).ToArray();
            Assert.IsTrue(guids.Contains(child.OwnerUserId.Value));
            Assert.IsTrue(guids.Contains(child.OwnerUser.OwnerUserId.Value));
            Assert.IsFalse(guids.Contains(child.Id));
            Assert.AreEqual(child.Id, resp.User.Id);

            int index1 = Array.IndexOf<Guid>(guids, child.OwnerUserId.Value);
            int index2 = Array.IndexOf<Guid>(guids, child.OwnerUser.OwnerUserId.Value);
            Assert.IsTrue(index1 < index2);
            Assert.AreEqual(0, index1);
            Assert.AreEqual(1, index2);
        }

        [TestMethod]
        public void GetUserParents_HasChilds() {
            UserParentResponse resp = handler.ProcessRequest(entities, child, child.Id.ToString());
            bool?[] values = resp.UserList.Select(u => u.HasChilds).Distinct().ToArray();
            Assert.AreEqual(1, values.Length);
            Assert.IsTrue(values[0].Value);
        }

        [TestMethod]
        public void GetUserParents_LastMessage() {
            UserParentResponse resp = handler.ProcessRequest(entities, child, child.Id.ToString());
            UserInfoShort inf = resp.UserList.Where(u => u.Id == child.OwnerUserId.Value).Single();
            int unread = inf.UnreadCount;

            DAL.Model.Message msg = entities.SendMessage(child.OwnerUserId.Value, child.Id);

            resp = handler.ProcessRequest(entities, child, child.Id.ToString());
            inf = resp.UserList.Where(u => u.Id == child.OwnerUserId.Value).Single();
            Assert.AreEqual(msg.Id, inf.LastMessage.Id);
            Assert.AreEqual(unread + 1, inf.UnreadCount);

            //request from other user
            DAL.Model.User user = entities.GetUser(u => u.Enabled && u.Id != child.Id, null);
            resp = handler.ProcessRequest(entities, user, child.Id.ToString());
            inf = resp.UserList.Where(u => u.Id == child.OwnerUserId.Value).Single();
            Assert.AreNotEqual(msg.Id, inf.LastMessage.Id);
        }

        [TestMethod]
        public void GetUserParents_DisabledUser() {
            DAL.Model.User user = entities.GetUser(null, null, enabled: false);
            ErrorResponseException e = Assert.ThrowsException<ErrorResponseException>(
                () => handler.ProcessRequest(entities, child, user.Id.ToString()));
            Assert.AreEqual(404, e.GetHttpCode());
            Assert.AreEqual(ErrorResponse.Kind.user_not_found, e.Error.Error);
        }

        [TestMethod]
        public void GetUserParents_BroadcastProhibitionStructure() {

            DAL.Model.User user = entities.GetUserQ(
                q => q.Where(u =>
                    u.OwnerUserId != null
                    && u.OwnerUser.OwnerUserId != null
                    && u.OwnerUser.BroadcastProhibitionBy.Contains(u.OwnerUser.OwnerUser)
                    && u.ChildUsers.Where(cu => cu.Enabled).Any()),
                u => {
                    DAL.Model.User uo = entities.GetUser(u2 => u2.OwnerUserId != null, u2 => u2.OwnerUser = entities.GetUserQ(null, null));
                    u.OwnerUser = uo;
                    u.ChildUsers.Add(entities.GetUser(u3 => u3.Id != uo.Id && u3.OwnerUserId == null, null));
                    u.OwnerUser.BroadcastProhibitionBy.Add(u.OwnerUser.OwnerUser);
                }
                );
            var resp = handler.ProcessRequest(entities, user.OwnerUser.OwnerUser, user.ChildUsers.Where(cu => cu.Enabled).First().Id.ToString());

            bool? flag = resp.UserList.Select(u => u.BroadcastProhibitionStructure).First();
            Assert.IsTrue(flag.Value);

            Assert.IsTrue(resp.User.BroadcastProhibitionStructure.Value);
        }

        [TestMethod]
        public void GetUserParents_BroadcastProhibitionStructure_false() {
            //get user: 1. who has owner and childs
            //          2. owner has not set broadcast prohibition for this users
            DAL.Model.User user = entities.GetUserQ(
                q => q.Where(u =>
                    u.OwnerUserId != null
                    && u.OwnerUser.OwnerUserId != null
                    && u.OwnerUser.OwnerUser.OwnerUserId == null
                    && u.BroadcastProhibitionBy.Contains(u.OwnerUser.OwnerUser) == false
                    && u.BroadcastProhibitionBy.Contains(u.OwnerUser) == false),
                u => {
                    DAL.Model.User uo = entities.GetUser(
                        u2 => u2.OwnerUserId != null
                            && u2.OwnerUser.OwnerUserId == null
                            && u2.BroadcastProhibitionBy.Contains(u2.OwnerUser) == false, 
                        u2 => u2.OwnerUser = entities.GetUser(u3 => u3.OwnerUserId == null, null));
                    u.OwnerUser = uo;
                }
                );
            //owner reqwuest user's childs, they all must has flag <BroadcastProhibitionStructure>
            var resp = handler.ProcessRequest(entities, user.OwnerUser, user.Id.ToString());

            bool? flag = resp.UserList.Select(u => u.BroadcastProhibitionStructure).First();
            Assert.IsFalse(flag.Value);
            Assert.IsFalse(resp.User.BroadcastProhibitionStructure.Value);
        }

        [TestMethod]
        public void GetUserParents_BroadcastProhibition() {
            //get user: 1. who has owner and childs
            //          2. owner set broadcast prohibition for one of his childs
            //          3. owner has not set broadcast prohibition for one of his childs
            DAL.Model.User user = entities.GetUserQ(
                q => q.Where(u =>
                    u.OwnerUserId != null
                    && u.OwnerUser.OwnerUserId != null
                    && u.OwnerUser.BroadcastProhibitionBy.Contains(u.OwnerUser.OwnerUser)),
                u => {
                    DAL.Model.User uo = entities.GetUser(u2 => u2.OwnerUserId != null, u2 => u2.OwnerUser = entities.GetUserQ(null, null));
                    u.OwnerUser = uo;
                    u.OwnerUser.BroadcastProhibitionBy.Add(u.OwnerUser.OwnerUser);
                }
                );

            var resp = handler.ProcessRequest(entities, user.OwnerUser.OwnerUser, user.Id.ToString());

            Assert.IsTrue(resp.UserList.First().BroadcastProhibition);
        }
    }
}
