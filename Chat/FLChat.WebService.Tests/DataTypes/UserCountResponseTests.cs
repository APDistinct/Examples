using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class UserCountResponseTests
    {
        [TestMethod]
        public void UserCountResponse_Serialize() {
            UserCountResponse resp = new UserCountResponse(3);
            string json = JsonConvert.SerializeObject(resp);
            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(
                new string[] { "count" }, 
                jo.Properties().Select(p => p.Name).ToArray());
        }
    }
}
