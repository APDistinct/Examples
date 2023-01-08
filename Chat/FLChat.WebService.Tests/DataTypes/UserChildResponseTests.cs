using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class UserChildResponseTests
    {
        [TestMethod]
        public void UserChildResponse_Serialize() {
            DAL.Model.User currUser = new DAL.Model.User() { Id = Guid.NewGuid() };
            UserChildResponse resp = new UserChildResponse(new DAL.Model.User(), new DAL.Model.User[] { }, currUser, null, null, null);
            string json = JsonConvert.SerializeObject(resp);
            JObject jo = JObject.Parse(json);

            CollectionAssert.AreEquivalent(
                new string[] { "user", "childs", "offset", "count", "max_count", "req_count", "total_count" },
                jo.Properties().Select(p => p.Name).ToArray());
        }
    }
}
