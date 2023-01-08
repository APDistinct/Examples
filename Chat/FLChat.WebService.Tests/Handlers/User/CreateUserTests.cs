using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL.Model;
using Newtonsoft.Json.Linq;
using FLChat.WebService.DataTypes;
using System.Linq;

namespace FLChat.WebService.Handlers.User.Tests
{
    [TestClass]
    public class CreateUserTests
    {
        ChatEntities entities;
        CreateUser handler;

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
            handler = new CreateUser();
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void CreateUserTest() {
            JObject input = new JObject() {
                ["phone"] = DateTime.Now.ToString(),
                ["full_name"] = "test create user"
            };

            DAL.Model.User owner = entities.GetUser(u => u.Enabled, null);
            UserProfileInfo response = handler.ProcessRequest(entities, owner, input);

            Assert.IsNotNull(response);
            Assert.AreEqual((string)input["full_name"], response.FullName);
            Assert.AreEqual((string)input["phone"], response.Phone);
            Assert.AreEqual(owner.Id, response.OwnerUserId.Value);

            DAL.Model.User user = entities.User.Where(u => u.Id == response.Id).FirstOrDefault();
            Assert.IsNotNull(user);
            Assert.AreEqual((string)input["full_name"], user.FullName);
            Assert.AreEqual((string)input["phone"], user.Phone);
            Assert.AreEqual(owner.Id, user.OwnerUserId.Value);

            entities.Entry(user).State = System.Data.Entity.EntityState.Deleted;
        }
    }
}
