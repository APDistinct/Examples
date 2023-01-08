using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using System.Collections.Generic;

namespace FLChat.WebService.Handlers.User.Tests
{
    [TestClass]
    public class UserSelectionCountTests
    {
        ChatEntities entities;
        UserSelectionCount handler = new UserSelectionCount();

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }


        [TestMethod]
        public void UserSelectionCount_Test() {
            DAL.Model.User user = entities.GetUser(
                u => u.OwnerUserId != null && u.OwnerUser.OwnerUserId != null 
                    && u.OwnerUser.OwnerUser.BroadcastProhibition.Any() == false,
                u => u.OwnerUser = entities.GetUser(
                    u2 => u2.OwnerUserId != null, 
                    u2 => u2.OwnerUser = entities.GetUser(null, null)));
            DAL.Model.User onwer = user.OwnerUser.OwnerUser;

            var include_ws = new List<UserSelection.UserStructureSelection>() {
                new UserSelection.UserStructureSelection() { UserId = user.OwnerUserId.Value, Type = UserSelection.SelectionType.Deep}
            };
            int cnt = handler.ProcessRequest(entities, onwer, new UserSelection() {
                IncludeWithStructure = include_ws
            }).Count;
            int totalChilds = entities.User_GetChilds(user.OwnerUserId.Value, null, null).Count();
            Assert.AreEqual(totalChilds + 1, cnt);

            int cnt2 = handler.ProcessRequest(entities, onwer, new UserSelection() {
                IncludeWithStructure = include_ws,
                ExcludeWithStructure = new List<Guid> { user.OwnerUserId.Value }
            }).Count;            
            Assert.AreEqual(0, cnt2);

            int cnt3 = handler.ProcessRequest(entities, onwer, new UserSelection() {
                IncludeWithStructure = include_ws,
                Exclude = new List<Guid> { user.OwnerUserId.Value }
            }).Count;
            Assert.AreEqual(1, cnt - cnt3);

            DAL.Model.User anyuser = entities.GetUser(u => u.ChildUsers.Any() == false && u.Id != user.Id, null);
            int cnt4 = handler.ProcessRequest(entities, onwer, new UserSelection() {
                Exclude = new List<Guid> { anyuser.Id },
                Include = new List<Guid> { anyuser.Id }
            }).Count;
            Assert.AreEqual(1, cnt4);
        }
    }
}
