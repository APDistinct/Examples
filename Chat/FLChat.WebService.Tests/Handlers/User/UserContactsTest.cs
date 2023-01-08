using System;
using System.Linq;
using System.Net;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.WebService.DataTypes;

namespace FLChat.WebService.Handlers.User.Tests
{
    [TestClass]
    public class UserContactsTest
    {
        ChatEntities entities;
        GetUserContacts handler = new GetUserContacts();

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
        public void GetUserContactsForNotActiveNotGetTest()
        {
            var user0 = entities.GetUser(null, null, enabled: false);

            try
            {
                string userId = user0.Id.ToString();
                var userContacts = handler.ProcessRequest(entities, user0, (string)null);
                Assert.Fail("Not throwing exception on creation profile for non active user ");
            }
            catch (ErrorResponseException e)
            {
                Assert.AreEqual((int)HttpStatusCode.NotFound, e.GetHttpCode());
            }           
        }

        [TestMethod]
        public void GetUserContacts_Test()
        {
            DAL.Model.User[] users = entities.GetUsers(3, u => u.Enabled, null, TransportKind.FLChat);
            Guid from = users[0].Id;
            Guid[] contacts = users.Skip(1).Select(u => u.Id).ToArray();
            Guid msgOutcome = Guid.Empty;
            Guid msgIncome = Guid.Empty;
            Guid[] messages = contacts.Reverse().Select(u => {
                if (Array.IndexOf(contacts, u) % 2 == 0) {
                    msgOutcome = entities.SendMessage(from, u).Id;
                    return msgOutcome;
                } else {
                    msgIncome = entities.SendMessage(u, from).Id;
                    return msgIncome;
                }
            }).Reverse().ToArray();

            var handler = new GetUserContacts() { MaxCount = int.MaxValue };
            UserContactsResponse resp = handler.ProcessRequest(entities, users[0], (string)null);
            UserInfoShort[] infoList = resp.UserList.ToArray();

            Assert.IsNotNull(resp);
            Assert.IsTrue(resp.UserList.Count() > contacts.Length);

            //contacts and their messages contain in response
            CollectionAssert.IsSubsetOf(contacts, 
                resp.UserList.Select(u => u.Id).ToArray());
            CollectionAssert.IsSubsetOf(messages, 
                resp.UserList.Select(u => u.LastMessage.Id).ToArray());

            //check order
            //var indexOutcome = Array.IndexOf(resp.UserList.Select(i => i.LastMessage.Id).ToArray(), msgOutcome);
            //var indexIncome = Array.IndexOf(resp.UserList.Select(i => i.LastMessage.Id).ToArray(), msgIncome);

            //becouse income has not IsRead flag
            //Assert.IsTrue(indexOutcome > indexIncome);

            //check all contacts 
            var query = entities.LastMessageView.Where(m => m.UserId == from && m.UserId != m.UserOppId).OrderByDescending(m => m.MsgToUserIdx);           
            CollectionAssert.AreEquivalent(
                query.Select(m => m.UserOppId).Take(handler.MaxCount).ToArray(), 
                resp.UserList.Select(u => u.Id).ToArray());
            CollectionAssert.AreEquivalent(
                entities.LastMessageView
                    .Where(m => m.UserId == from && m.UserId != m.UserOppId)
                    .OrderByDescending(m => m.MsgToUserIdx)
                    .Select(m => m.MsgId).Take(handler.MaxCount)
                    .ToArray(),
                resp.UserList.Select(u => u.LastMessage.Id).ToArray());

            //set message to read
            var dbmsg = entities.MessageToUser.Where(m => m.MsgId == msgIncome).Single();
            dbmsg.IsRead = true;
            entities.SaveChanges();

            //request data again
            resp = handler.ProcessRequest(entities, users[0], (string)null);
            //check order
            //indexOutcome = Array.IndexOf(resp.UserList.Select(i => i.LastMessage.Id).ToArray(), msgOutcome);
            //indexIncome = Array.IndexOf(resp.UserList.Select(i => i.LastMessage.Id).ToArray(), msgIncome);

            //order var changed
            //Assert.IsTrue(indexOutcome < indexIncome);
        }

        [TestMethod]
        public void GetUserContacts_Partial() {
            DAL.Model.User[] users = entities.GetUsers(3, u => u.Enabled, null, TransportKind.FLChat);
            Guid from = users[0].Id;
            Guid[] contacts = users.Skip(1).Select(u => u.Id).ToArray();
            Guid[] messages = contacts.Reverse().Select(u => {
                if (Array.IndexOf(contacts, u) % 2 == 0)
                    return entities.SendMessage(from, u).Id;
                else
                    return entities.SendMessage(u, from).Id;
            }).Reverse().ToArray();

            UserContactsResponse resp = handler.ProcessRequest(entities, users[0], new PartialDataIdRequest() {
                Count = 2
            });
            Assert.AreEqual(2, resp.UserList.Count());

            Assert.AreEqual(2, resp.UserList.Count());
            Assert.AreEqual(handler.MaxCount, resp.MaxCount);
            Assert.AreEqual(0, resp.Offset);
            Assert.AreEqual(2, resp.RequestedCount);
            Assert.IsNotNull(resp.TotalCount);
            Assert.AreNotEqual(0, resp.TotalCount);

            UserContactsResponse resp2 = handler.ProcessRequest(entities, users[0], new PartialDataIdRequest() {
                Offset = 2,
                Count = 5
            });
            Assert.AreEqual(handler.MaxCount, resp2.MaxCount);
            Assert.AreEqual(2, resp2.Offset);
            Assert.AreEqual(5, resp2.RequestedCount);
            Assert.IsNull(resp2.TotalCount);

            Assert.AreEqual(0, resp.UserList.Select(u => u.Id).Intersect(resp2.UserList.Select(u => u.Id)).Count());
        }

        [TestMethod]
        public void GetUserContacts_Tags() {
            Segment segment = entities.GetSegment(
                s => s.IsDeleted == false && s.ShowInShortProfile == true,
                s => s.ShowInShortProfile = true);

            DAL.Model.User user = entities.GetUser(
                u => u.Segments.Contains(segment) && u.OwnerUserId != null 
                    && u.OwnerUser.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any(),
                u => {
                    u.Segments.Add(segment);
                    u.OwnerUser = entities.GetUser(null, null, TransportKind.FLChat);
                },                
                TransportKind.FLChat);
            entities.SendMessage(user.OwnerUserId.Value, user.Id);

            var resp = handler.ProcessRequest(entities, user.OwnerUser, (string)null);
            UserInfoShort ui = resp.UserList.Where(u => u.Id == user.Id).Single();
            Assert.IsNotNull(ui.Tags);
            Assert.IsTrue(ui.Tags.Contains(segment.Name));
        }

        [TestMethod]
        public void GetUserContacts_TagsEmpty() {
            Segment segment = entities.GetSegment(
                s => s.IsDeleted == false && s.ShowInShortProfile == true,
                s => s.ShowInShortProfile = true);

            DAL.Model.User user = entities.GetUser(
                u => u.Segments.Contains(segment) == false && u.OwnerUserId != null
                    && u.OwnerUser.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any(),
                u => {
                    u.Segments.Add(segment);
                    u.OwnerUser = entities.GetUser(null, null, TransportKind.FLChat);
                },
                TransportKind.FLChat);
            var resp = handler.ProcessRequest(entities, user.OwnerUser, (string)null);
            UserInfoShort ui = resp.UserList.Where(u => u.Id == user.Id).Single();
            Assert.IsNull(ui.Tags);
        }

        [TestMethod]
        public void GetUserContacts_BroadcastProhibitionStructure() {
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
            entities.SendMessage(user.OwnerUser.Id, child.Id, TransportKind.WebChat, TransportKind.WebChat);

            var resp = handler.ProcessRequest(entities, user.OwnerUser, (string)null);

            Assert.IsTrue(resp.UserList.Where(u => u.Id == child.Id).Select(u => u.BroadcastProhibitionStructure.Value).Single());
        }

        [TestMethod]
        public void GetUserContacts_BroadcastProhibitionStructure_false() {
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
            entities.SendMessage(user.OwnerUser.Id, child.Id, TransportKind.WebChat, TransportKind.WebChat);

            var resp = handler.ProcessRequest(entities, user.OwnerUser, (string)null);

            Assert.IsFalse(resp.UserList.Where(u => u.Id == child.Id).Select(u => u.BroadcastProhibitionStructure.Value).Single());
        }

        [TestMethod]
        public void GetUserContacts_BroadcastProhibition() {
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

            entities.SendMessage(user.OwnerUser.Id, bpu.Id, TransportKind.WebChat, TransportKind.WebChat);
            entities.SendMessage(user.OwnerUser.Id, bpu_wo.Id, TransportKind.WebChat, TransportKind.WebChat);

            var resp = handler.ProcessRequest(entities, user.OwnerUser, (string)null);

            Assert.IsTrue(resp.UserList.Where(u => u.Id == bpu.Id).Single().BroadcastProhibition);
            Assert.IsFalse(resp.UserList.Where(u => u.Id == bpu_wo.Id).Single().BroadcastProhibition);
        }

        [TestMethod]
        public void GetUserContacts_HasChilds() {
            //get user: 1. who has owner and childs
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
            entities.SendMessage(user.OwnerUser.Id, user.Id, TransportKind.WebChat, TransportKind.WebChat);

            var resp = handler.ProcessRequest(entities, user.OwnerUser, (string)null);

            Assert.IsTrue(resp.UserList.Where(u => u.Id == user.Id).Select(u => u.HasChilds.Value).Single());
        }

        [TestMethod]
        public void GetUserContacts_HasChilds_false() {
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
            entities.SendMessage(user.OwnerUser.Id, user.Id, TransportKind.WebChat, TransportKind.WebChat);

            var resp = handler.ProcessRequest(entities, user.OwnerUser, (string)null);

            Assert.IsFalse(resp.UserList.Where(u => u.Id == user.Id).Select(u => u.HasChilds.Value).Single());
        }

        [TestMethod]
        public void DeletedUser() {
            DAL.Model.User user = entities.GetUserQ(enabled: false, transport: TransportKind.FLChat, hasOwner: true, ownerTransport: TransportKind.FLChat);
            DAL.Model.Message msg = entities.SendMessage(user.OwnerUserId.Value, user.Id);

            var resp = handler.ProcessRequest(entities, user.OwnerUser, (string)null);
            Assert.IsFalse(resp.UserList.Where(i => i.Id == user.Id).Any());
        }
    }
}
