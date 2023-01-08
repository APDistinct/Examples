using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL.Model;
using FLChat.DAL;
using FLChat.WebService.DataTypes;

namespace FLChat.WebService.Handlers.User.Tests
{
    [TestClass]
    public class SearchUserTests
    {
        ChatEntities entities;
        SearchUser handler = new SearchUser() { MaxCount = int.MaxValue };

        DAL.Model.User user;
        DAL.Model.User child;

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();

            child = entities.GetUser(
                u => u.OwnerUserId != null && u.FullName != null && u.FullName.Length > 5,
                u => {
                    u.OwnerUser = entities.GetUser(null, null, TransportKind.FLChat);
                    u.FullName = Guid.NewGuid().ToString();
                    },
                TransportKind.FLChat);            
            user = child.OwnerUser;
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void SearchUser_Search() {
            string value = child.FullName.Substring(0, 5);
            var response = handler.ProcessRequest(entities, user, new DataTypes.SearchUserRequest() {
                SearchValue = value
            });
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.UserList);
            Assert.IsTrue(response.UserList.Any());

            foreach (var user in response.UserList.Select(u => u._user)) {
                Assert.IsTrue((user.FullName != null && user.FullName.ToUpper().Contains(value.ToUpper()))
                    || (user.Phone != null && user.Phone.Contains(value))
                    || (user.Email != null && user.Email.ToUpper().Contains(value.ToUpper())));
            }
        }

        [TestMethod]
        public void SearchUser_Partial() {
            DAL.Model.User user = entities.GetUser(
                u => u.ChildUsers.Where(c => c.Enabled && c.FullName.Length > 5).Count() >= 5,
                u => u.ChildUsers = entities.GetUsers(5, c => c.Id != u.Id, null));
            string value = user.ChildUsers.Where(c => c.Enabled && c.FullName.Length > 5).First().FullName.Substring(0, 5);
            DataTypes.SearchUserRequest input = new DataTypes.SearchUserRequest() {
                Count = 2,
                SearchValue = value
            };

            var resp = handler.ProcessRequest(entities, user, input);
            Assert.AreEqual(input.Count, resp.UserList.Count());
            Assert.AreEqual(handler.MaxCount, resp.MaxCount);
            Assert.AreEqual(0, resp.Offset);
            Assert.AreEqual(input.Count, resp.RequestedCount);
            Assert.IsNotNull(resp.TotalCount);
            Assert.AreNotEqual(0, resp.TotalCount);
            Guid[] firstIds = resp.UserList.Select(u => u.Id).ToArray();

            input = new DataTypes.SearchUserRequest() {
                Count = 2,
                Offset = 2,
                SearchValue = value
            };
            resp = handler.ProcessRequest(entities, user, input);
            Assert.AreEqual(input.Count, resp.UserList.Count());
            Assert.AreEqual(handler.MaxCount, resp.MaxCount);
            Assert.AreEqual(input.Offset, resp.Offset);
            Assert.AreEqual(input.Count, resp.RequestedCount);
            Assert.IsNull(resp.TotalCount);
            Guid[] secondIds = resp.UserList.Select(u => u.Id).ToArray();

            Assert.AreEqual(0, firstIds.Intersect(secondIds).Count());
        }

        [TestMethod]
        public void SearchUser__Tags() {
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

            var resp = handler.ProcessRequest(entities, user.OwnerUser, 
                new SearchUserRequest() { SearchValue = user.FullName });
            UserInfoShort ui = resp.UserList.Where(u => u.Id == user.Id).Single();
            Assert.IsNotNull(ui.Tags);
            Assert.IsTrue(ui.Tags.Contains(segment.Name));
        }

        [TestMethod]
        public void SearchUser_TagsEmpty() {
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
            var resp = handler.ProcessRequest(entities, user.OwnerUser, 
                new SearchUserRequest() { SearchValue = user.FullName });
            UserInfoShort ui = resp.UserList.Where(u => u.Id == user.Id).Single();
            Assert.IsNull(ui.Tags);
        }

        [TestMethod]
        public void SearchUser_BroadcastProhibitionStructure() {
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
            DAL.Model.User child = user.ChildUsers.Where(u3 => u3.Enabled).First();
            child.FullName = "test search user";
            entities.SaveChanges();

            var resp = handler.ProcessRequest(entities, user.OwnerUser, new SearchUserRequest() { SearchValue = "test search user" });

            Assert.IsTrue(resp.UserList.Where(u => u.Id == child.Id).Select(u => u.BroadcastProhibitionStructure.Value).Single());
        }

        [TestMethod]
        public void SearchUser_BroadcastProhibitionStructure_false() {
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
            DAL.Model.User child = user.ChildUsers.Where(u3 => u3.Enabled).First();
            child.FullName = "test search user";
            entities.SaveChanges();

            var resp = handler.ProcessRequest(entities, user.OwnerUser, new SearchUserRequest() { SearchValue = "test search user" });

            Assert.IsFalse(resp.UserList.Where(u => u.Id == child.Id).Select(u => u.BroadcastProhibitionStructure.Value).Single());
        }

        [TestMethod]
        public void SearchUser_BroadcastProhibition() {
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
            bpu.FullName = "test search user";
            bpu_wo.FullName = "test search user";
            entities.SaveChanges();

            var resp = handler.ProcessRequest(entities, user.OwnerUser, new SearchUserRequest() { SearchValue = "test search user" });

            Assert.IsTrue(resp.UserList.Where(u => u.Id == bpu.Id).Single().BroadcastProhibition);
            Assert.IsFalse(resp.UserList.Where(u => u.Id == bpu_wo.Id).Single().BroadcastProhibition);
        }

        [TestMethod]
        public void SearchUser_HasChilds() {
            //get user: who has owner and childs
            DAL.Model.User user = entities.GetUserQ(
                q => q.Where(u =>
                    u.OwnerUserId != null
                    && u.ChildUsers.Where(u3 => u3.Enabled).Any()),
                u => {
                    DAL.Model.User[] users = entities.GetUsers(2, u2 => u2.OwnerUserId == null, null);
                    u.OwnerUserId = users[0].Id;
                    u.ChildUsers.Add(users[1]);
                }
                );
            user.FullName = "test search user";
            entities.SaveChanges();

            var resp = handler.ProcessRequest(entities, user.OwnerUser, new SearchUserRequest() { SearchValue = "test search user" });

            Assert.IsTrue(resp.UserList.Where(u => u.Id == user.Id).Select(u => u.HasChilds.Value).Single());
        }

        [TestMethod]
        public void SearchUser_HasChilds_false() {
            //get user: 1. who has owner and has not childs
            DAL.Model.User user = entities.GetUserQ(
                q => q.Where(u =>
                    u.OwnerUserId != null
                    && u.ChildUsers.Where(u3 => u3.Enabled).Any() == false),
                u => {
                    DAL.Model.User[] users = entities.GetUsers(1, u2 => u2.OwnerUserId == null, null);
                    u.OwnerUserId = users[0].Id;
                }
                );
            user.FullName = "test search user";
            entities.SaveChanges();

            var resp = handler.ProcessRequest(entities, user.OwnerUser, new SearchUserRequest() { SearchValue = "test search user" });

            Assert.IsFalse(resp.UserList.Where(u => u.Id == user.Id).Select(u => u.HasChilds.Value).Single());
        }
    }
}
