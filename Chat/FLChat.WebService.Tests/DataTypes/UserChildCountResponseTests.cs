using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class UserChildCountResponseTests
    {
        [TestMethod]
        public void UserChildCountResponse_Serialize() {
            UserChildCountResponse resp = new UserChildCountResponse() {
                Count = 100,
                UserId = Guid.NewGuid()
            };
            string json = JsonConvert.SerializeObject(resp);
            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(
                new string[] { "user_id", "count" },
                jo.Properties().Select(p => p.Name).ToArray());

            Assert.AreEqual(resp.UserId, (Guid)jo["user_id"]);
        }
    }
}
