using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class UserParentResponseTests
    {
        [TestMethod]
        public void UserParentResponse_Serialize() {
            DAL.Model.User currUser = new DAL.Model.User() { Id = Guid.NewGuid() };
            UserParentResponse resp = new UserParentResponse(new DAL.Model.User(), new DAL.Model.User[] { }, currUser, null, null);
            string json = JsonConvert.SerializeObject(resp);

            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(new string[] { "parents", "user" }, jo.Properties().Select(p => p.Name).ToArray());
        }
    }
}
