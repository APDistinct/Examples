using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class UserProfileInfoTests
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
        public void UserProfileInfo_ParentChildDepth() {
            User user = entities.GetUserQ(
                where: q => q.Where(u => u.OwnerUserId != null && u.ChildUsers.Any()),
                create: u => {
                    u.OwnerUser = entities.GetUserQ();
                    u.ChildUsers.Add(entities.GetUserQ(where: q => q.Where(u2 => u2.Id != u.OwnerUser.Id)));
                    }
                );
            //owner request his child
            UserProfileInfo pi = new UserProfileInfo(
                new UserProfileInfo.UserExt() { User = user }, 
                entities, 
                user.OwnerUserId.Value);
            Assert.AreEqual(1, pi.ChildDepth);
            Assert.IsNull(pi.ParentDepth);

            //child request his owner
            pi = new UserProfileInfo(
                new UserProfileInfo.UserExt() { User = user },
                entities,
                user.ChildUsers.First().Id);
            Assert.IsNull(pi.ChildDepth);
            Assert.AreEqual(1, pi.ParentDepth);

            //owner request child of child
            pi = new UserProfileInfo(
                new UserProfileInfo.UserExt() { User = user.ChildUsers.First() },
                entities,
                user.OwnerUserId.Value);
            Assert.AreEqual(2, pi.ChildDepth);
            Assert.IsNull(pi.ParentDepth);

            //child request his parent of parent
            pi = new UserProfileInfo(
                new UserProfileInfo.UserExt() { User = user.OwnerUser },
                entities,
                user.ChildUsers.First().Id);
            Assert.IsNull(pi.ChildDepth);
            Assert.AreEqual(2, pi.ParentDepth);

            //requst own profile
            pi = new UserProfileInfo(
                new UserProfileInfo.UserExt() { User = user },
                entities,
                user.Id);
            Assert.IsNull(pi.ChildDepth);
            Assert.IsNull(pi.ParentDepth);
        }

        [TestMethod]
        public void UserProfileInfo_IsMe() {
            User user = entities.GetUserQ();
            User user2 = entities.GetUserQ(where: q=> q.Where(u => u.Id != user.Id));
            //owner request his child
            UserProfileInfo pi = new UserProfileInfo(
                new UserProfileInfo.UserExt() { User = user },
                entities,
                user.Id);
            Assert.IsTrue(pi.IsMe.Value);

            pi = new UserProfileInfo(
                new UserProfileInfo.UserExt() { User = user },
                entities,
                user2.Id);
            Assert.IsFalse(pi.IsMe.Value);
        }

        [TestMethod]
        public void UserProfileInfo_Serialize() {
            UserProfileInfo pi = new UserProfileInfo(new User()) { InviteLink = new UserProfileInfo.Invite(), };
            string json = JsonConvert.SerializeObject(pi);
            JObject jo = JObject.Parse(json);
            string[] fields = new string[] { "is_me", "parent_depth", "child_depth", "invite_link" };
            CollectionAssert.AreEquivalent(
                fields, 
                jo.Properties().Select(p => p.Name).Intersect(fields).ToArray());            
        }

        [TestMethod]
        public void InviteLink_Make()
        {
            string code = "1A2B3C4D5E";
            var inviteLink = UserProfileInfo.Invite.Make(entities, code, "https://localhost/%code%");

            Assert.AreEqual(code, inviteLink.Code);
            Assert.AreEqual(string.Concat("https://localhost/", code), inviteLink.Url);

            var arr1 = entities
                    .TransportType
                    .Where(tt => tt.Enabled && tt.DeepLink != null)
                    .ToArray()
                    .Select(tt => tt.DeepLink.Replace("%code%", code))
                    .ToArray();
            var arr2 = inviteLink.InviteButtons.Select(x => x.Url).ToArray();
            
            CollectionAssert.AreEquivalent(arr1, arr2);                
        }

        [TestMethod]
        public void InviteLink_FieldNames() {
            string code = "1A2B3C4D5E";
            var inviteLink = UserProfileInfo.Invite.Make(entities, code, "https://localhost/%code%");

            string json = JsonConvert.SerializeObject(inviteLink);
            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(
                new string[] { "code", "url", "invite_buttons" },
                jo.Properties().Select(p => p.Name).ToArray());
        }

        [TestMethod]
        public void InviteLinkButton_FieldNames() {
            UserProfileInfo.InviteLinkButton btn = new UserProfileInfo.InviteLinkButton() {
                Transport = DAL.TransportKind.Telegram,
                Url = "someurl"
            };
            string json = JsonConvert.SerializeObject(btn);
            JObject jo = JObject.Parse(json);

            CollectionAssert.AreEquivalent(
                new string[] { "transport", "url" },
                jo.Properties().Select(p => p.Name).ToArray());
        }

        [TestMethod]
        public void InviteLinkButton_Transport() {
            UserProfileInfo.InviteLinkButton btn = new UserProfileInfo.InviteLinkButton() {
                Transport = DAL.TransportKind.Telegram,
                Url = "someurl"
            };
            string json = JsonConvert.SerializeObject(btn);
            JObject jo = JObject.Parse(json);
            Assert.AreEqual("Telegram", (string)jo["transport"]);
        }
    }
}
