using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using FLChat.DAL.DataTypes;

namespace FLChat.DAL.Model.Tests
{
    [TestClass]
    public class UserTests
    {
        ChatEntities entities;

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void User_Create() {
            User u = new User();
            entities.User.Add(u);
            entities.SaveChanges();

            Assert.AreNotEqual(Guid.Empty, u.Id);
            Assert.IsTrue(Math.Abs((u.InsertDate - DateTime.UtcNow).TotalMinutes) < 1);

            entities.Entry(u).State = System.Data.Entity.EntityState.Deleted;
            entities.SaveChanges();
        }

        /// <summary>
        /// Just check for don't throw exception
        /// </summary>
        [TestMethod]
        public void User_LastDeliveryEventId() {
            long? id = entities.LastDeliveryEventId(Guid.Empty);
        }

        /// <summary>
        /// Get User's Transport id from view
        /// </summary>
        [TestMethod]
        public void User_GetUserTransport()
        {
            int num = 3;
            User[] users = entities.GetUsers
               (num,
               u => u.Enabled ,
               u => { u.Enabled = true; }
               );
            users[0].DefaultTransportTypeId = null;
            var list = users.Select(x => x.Id).ToList();
            var retlist = entities.GetUserTransport(list);
            Assert.AreEqual(num, retlist.Count);
            CollectionAssert.AreEqual
                (users.Select(u => u.Id).OrderBy(x => x).ToList(),
                retlist.Select(x => x.Key).OrderBy(x => x).ToList());
        }

        [TestMethod]
        public void User_AvailableTransports() {
            int[] ttlist = entities.TransportType.Where(tt => tt.Enabled && tt.VisibleForUser == false).Select(tt => tt.Id).ToArray();
            if (ttlist.Length == 0)
                return;

            User user = entities.GetUser(
                u => u.Enabled
                    && u.Transports.Where(t => t.TransportType.VisibleForUser == true).Any()
                    && u.Transports.Where(t => t.TransportType.VisibleForUser == false).Any(),
                u => u.Transports = new Transport[] {
                    new Transport() { Kind = TransportKind.FLChat, Enabled = true },
                    new Transport() {Kind = (TransportKind)ttlist[0], Enabled = true }
                });

            
            Assert.AreEqual(user.Transports.Where(t => t.TransportType.VisibleForUser).Count(), user.AvailableTransports.Count());
            foreach (TransportKind tk in user.AvailableTransports)
                CollectionAssert.DoesNotContain(ttlist, (int)tk);            
        }

        [TestMethod]
        public void User_GetAddresseesForExternalTrans() {
            User user = entities.GetUserQ(
                hasOwner: true,
                ownerTransport: TransportKind.FLChat);
            User[] users = entities.GetAddresseesForExternalTrans(user.Id);
            Assert.IsNotNull(users);
            Assert.IsTrue(users.Select(u => u.Id).Contains(user.OwnerUserId.Value));
        }

        [TestMethod]
        public void UserSelectionCountTest() {
            DAL.Model.User user = entities.GetUser(
                            u => u.OwnerUserId != null && u.OwnerUser.OwnerUserId != null
                                && u.OwnerUser.OwnerUser.BroadcastProhibition.Any() == false,
                            u => u.OwnerUser = entities.GetUser(
                                u2 => u2.OwnerUserId != null,
                                u2 => u2.OwnerUser = entities.GetUser(null, null)));
            DAL.Model.User onwer = user.OwnerUser.OwnerUser;

            Tuple<Guid, int?>[] include_ws = new Tuple<Guid, int?>[] { Tuple.Create(user.OwnerUserId.Value, (int?)null) };
            int cnt = entities.UserSelectionCount(onwer.Id, new UserSelection() { IncludeWithStructure = include_ws });

            int cnt2 = entities.UserSelectionCount(onwer.Id, new UserSelection() {
                IncludeWithStructure = include_ws,
                ExcludeWithStructure = new Guid[] { user.OwnerUserId.Value }
            });
            Assert.AreEqual(0, cnt2);

            int cnt3 = entities.UserSelectionCount(onwer.Id, new UserSelection() {
                IncludeWithStructure = include_ws,
                Exclude = new Guid[] { user.Id }
            });
            Assert.AreEqual(1, cnt - cnt3);

            int cnt4 = entities.UserSelectionCount(onwer.Id, new UserSelection() {
                IncludeWithStructure = include_ws,
                Include = new Guid[] { user.Id },
                Exclude = new Guid[] { user.Id }
            });
            Assert.AreEqual(cnt, cnt4);
        }

        [TestMethod]
        public void UserSelectionCount_Segment() {
            DAL.Model.User user = entities.GetUser(
                u => u.OwnerUserId != null && u.Segments.Any(),
                u => {
                    u.OwnerUser = entities.GetUser(null, null);
                    u.Segments.Add(entities.GetSegment(s => true, null));
                });
            int cnt = entities.UserSelectionCount(user.OwnerUserId.Value, new UserSelection() {
                Segments = new Guid[] { user.Segments.First().Id } });
            Assert.IsTrue(cnt > 0);
        }

        [TestMethod]
        public void User_GetHasChilds() {
            User user1 = entities.GetUserQ(
                q => q.Where(u => u.ChildUsers.Any()),
                u => u.ChildUsers.Add(entities.GetUserQ(null, null)));
            User user2 = entities.GetUserQ(
                q => q.Where(u => u.ChildUsers.Any() == false),
                null);
            Guid[] ids = new Guid[] { user1.Id, user2.Id };
            HashSet<Guid> hs = entities.GetHasChilds(ids);
            Assert.IsTrue(hs.Contains(user1.Id));
            Assert.IsFalse(hs.Contains(user2.Id));
            Assert.AreEqual(1, hs.Count);
        }
    }
}
