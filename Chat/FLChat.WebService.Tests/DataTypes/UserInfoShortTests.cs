using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using FLChat.DAL.Model;
using System.Collections.Generic;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class UserInfoShortTests
    {
        [TestMethod]
        public void UserInfoShort_Online_Null() {
            UserInfoShort ui = new UserInfoShort(new User() { LastGetEvents = null }, null, 0);
            Assert.IsNull(ui.Online);
            string s = JsonConvert.SerializeObject(ui);
            JObject jobj = JObject.Parse(s);
            Assert.IsTrue(jobj.ContainsKey("online"));
            Assert.AreEqual(JTokenType.Null, jobj["online"].Type);
        }

        [TestMethod]
        public void UserInfoShort_Online_Online() {
            UserInfoShort ui = new UserInfoShort(new User() { LastGetEvents = DateTime.UtcNow }, null, 0);
            Assert.AreEqual(UserOnlineStatus.Online, ui.Online);
            string s = JsonConvert.SerializeObject(ui);
            JObject jobj = JObject.Parse(s);
            Assert.IsTrue(jobj.ContainsKey("online"));
            Assert.AreEqual("online", (string)jobj["online"]);
        }

        [TestMethod]
        public void UserInfoShort_Online_Offline() {
            UserInfoShort ui = new UserInfoShort(new User() { LastGetEvents = DateTime.UtcNow - TimeSpan.FromHours(1) }, null, 0);
            Assert.AreEqual(UserOnlineStatus.Offline, ui.Online);
            string s = JsonConvert.SerializeObject(ui);
            JObject jobj = JObject.Parse(s);
            Assert.IsTrue(jobj.ContainsKey("online"));
            Assert.AreEqual("offline", (string)jobj["online"]);
        }

        [TestMethod]
        public void UserInfoShortExtentions_ToUserInfoShort_WithMsg() {
            User currUser = new User() { Id = Guid.NewGuid() };
            User user = new User() { Id = Guid.NewGuid() };
            Dictionary<Guid, MessageToUser> lastMesssages = new Dictionary<Guid, MessageToUser>() {
                { user.Id, new MessageToUser() { MsgId = Guid.NewGuid(), ToUserId = currUser.Id, Message = new Message() { FromUserId = currUser.Id } }  }
            };
            UserInfoShort info = user.ToUserInfoShort(currUser, lastMesssages, null, null);

            Assert.AreEqual(info.Id, user.Id);
            Assert.IsNotNull(info.LastMessage);
        }

        [TestMethod]
        public void UserInfoShortExtentions_ToUserInfoShort_WithoutMsg() {
            User currUser = new User() { Id = Guid.NewGuid() };
            User user = new User() { Id = Guid.NewGuid() };
            Dictionary<Guid, MessageToUser> lastMesssages = new Dictionary<Guid, MessageToUser>() {
                { Guid.NewGuid(), new MessageToUser() { MsgId = Guid.NewGuid(), ToUserId = currUser.Id, Message = new Message() { FromUserId = currUser.Id } }  }
            };
            UserInfoShort info = user.ToUserInfoShort(currUser, lastMesssages, null, null);

            Assert.AreEqual(info.Id, user.Id);
            Assert.IsNull(info.LastMessage);
        }

        [TestMethod]
        public void UserInfoShortExtentions_ToUserInfoShort_List() {
            User currUser = new User() { Id = Guid.NewGuid() };
            User[] users = new User[] {
                new User() { Id = Guid.NewGuid() },
                new User() { Id = Guid.NewGuid() }
            };
            Dictionary<Guid, MessageToUser> lastMesssages = new Dictionary<Guid, MessageToUser>() {
                { users[0].Id, new MessageToUser() { MsgId = Guid.NewGuid(), ToUserId = currUser.Id, Message = new Message() { FromUserId = currUser.Id } }  }
            };
            UserInfoShort[] info = users.ToUserInfoShort(currUser, lastMesssages, null, null).ToArray();

            CollectionAssert.AreEqual(
                users.Select(u => u.Id).ToArray(),
                info.Select(i => i.Id).ToArray()
                );
            Assert.IsNotNull(info[0].LastMessage);
            Assert.IsNull(info[1].LastMessage);
        }

        [TestMethod]
        public void UserInfoShortExtentions_HasChilds() {
            {
                UserInfoShort ui = new UserInfoShort(new User(), null, 0) { HasChilds = true };
                String json = JsonConvert.SerializeObject(ui);
                JObject jo = JObject.Parse(json);
                Assert.IsTrue(jo.ContainsKey("has_childs"));
            }
            {
                UserInfoShort ui = new UserInfoShort(new User(), null, 0);
                String json = JsonConvert.SerializeObject(ui);
                JObject jo = JObject.Parse(json);
                Assert.IsFalse(jo.ContainsKey("has_childs"));
            }
        }

        [TestMethod]
        public void UserInfoShortExtentions_Tags() {
            User currUser = new User() { Id = Guid.NewGuid() };
            User user = new User() { Id = Guid.NewGuid() };
            Dictionary<Guid, List<string>> tags = new Dictionary<Guid, List<string>>() {
                [user.Id] = new List<string>() { "abc" },
            };
            UserInfoShort ui = user.ToUserInfoShort(currUser, null, null, tags);
            Assert.IsNotNull(ui.Tags);
            Assert.IsTrue(ui.Tags.Any());
            CollectionAssert.AreEqual(tags[user.Id], ui.Tags);

            UserInfoShort ui2 = new User() { Id = Guid.NewGuid() }.ToUserInfoShort(currUser, null, null, tags);
            Assert.IsNull(ui2.Tags);
        }

        [TestMethod]
        public void UserInfoShortExtentions_BroadcastProhibitionStructure() {
            User currUser = new User() { Id = Guid.NewGuid() };
            User user = new User() { Id = Guid.NewGuid() };
            HashSet<Guid> bps = new HashSet<Guid> { user.Id };
            UserInfoShort ui = user.ToUserInfoShort(currUser, null, null, broadcastProhibitionStructure: bps);
            Assert.IsNotNull(ui.BroadcastProhibitionStructure);
            Assert.IsTrue(ui.BroadcastProhibitionStructure.Value);            

            UserInfoShort ui2 = new User() { Id = Guid.NewGuid() }.ToUserInfoShort(currUser, null, null, broadcastProhibitionStructure: bps);
            Assert.IsNotNull(ui2.BroadcastProhibitionStructure);
            Assert.IsFalse(ui2.BroadcastProhibitionStructure.Value);

            UserInfoShort ui3 = new User() { Id = Guid.NewGuid() }.ToUserInfoShort(currUser, null, null);
            Assert.IsNull(ui3.BroadcastProhibitionStructure);
        }

        [TestMethod]
        public void UserInfoShortExtentions_HasChilds_HashSet() {
            User currUser = new User() { Id = Guid.NewGuid() };
            User user = new User() { Id = Guid.NewGuid() };
            HashSet<Guid> hasChilds = new HashSet<Guid> { user.Id };
            UserInfoShort ui = user.ToUserInfoShort(currUser, null, null, hasChilds: hasChilds);
            Assert.IsNotNull(ui.HasChilds);
            Assert.IsTrue(ui.HasChilds.Value);

            UserInfoShort ui2 = new User() { Id = Guid.NewGuid() }.ToUserInfoShort(currUser, null, null, hasChilds: hasChilds);
            Assert.IsNotNull(ui2.HasChilds);
            Assert.IsFalse(ui2.HasChilds.Value);

            UserInfoShort ui3 = new User() { Id = Guid.NewGuid() }.ToUserInfoShort(currUser, null, null);
            Assert.IsNull(ui3.HasChilds);
        }
    }
}
