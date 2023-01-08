using System;
using System.Linq;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.WebService.Handlers.Auth.Tests
{
    [TestClass]
    public class LogoutTests
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
        public void Logout_Test() {
            DAL.Model.User user = entities.GetUser(
                u => u.Enabled && u.IsConsultant && u.FullName == "test_token_user",
                u => {
                    u.Enabled = true;
                    u.FullName = "test_token_user";
                    u.IsConsultant = true;
                });

            string token = "logout_token_" + DateTime.Now.ToString();
            user.AuthToken.Add(new AuthToken() {
                Token = token,
                IssueDate = DateTime.Now,
                ExpireBy = 200,
                UserId = user.Id
            });
            entities.SaveChanges();

            Assert.IsTrue(entities.AuthToken.Where(t => t.Token == token).Any());

            Logout logout = new Logout();
            logout.ProcessRequest(entities, user, token);

            Assert.IsFalse(entities.AuthToken.Where(t => t.Token == token).Any());
        }
    }
}
