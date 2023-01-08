using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FLChat.DAL;
using FLChat.DAL.Model;

namespace FLChat.WebService.Handlers.Auth.Tests
{
    [TestClass]
    public class VerifyTokenStrategyTests
    {
        ChatEntities entities;
        AuthTokenFactory factory;
        VerifyTokenStrategy vt;
        DAL.Model.User user;

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
            factory = new AuthTokenFactory();
            vt = new VerifyTokenStrategy(factory);

            user = entities.GetUser(
                u => u.Enabled && u.IsConsultant && u.FullName == "test_token_user",
                u => {
                    u.Enabled = true;
                    u.FullName = "test_token_user";
                    u.IsConsultant = true;
                });
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void VerifyTokenStrategy_Success() {
            string token = AddToken(user, DateTime.Now, 600);            
            IUserAuthInfo ui = vt.CheckToken(token, entities, out bool isExpired);
            Assert.IsNotNull(ui);
            Assert.AreEqual(user.Id, ui.UserId);
        }

        [TestMethod]
        public void VerifyTokenStrategy_InvalidToken() {
            string token = "123";
            IUserAuthInfo ui = vt.CheckToken(token, entities, out bool isExpired);
            Assert.IsNull(ui);
            Assert.IsFalse(isExpired);
        }

        [TestMethod]
        public void VerifyTokenStrategy_ExpiredToken() {
            string token = AddToken(user, DateTime.Now.AddSeconds(-601), 600);
            IUserAuthInfo ui = vt.CheckToken(token, entities, out bool isExpired);
            Assert.IsNull(ui);
            Assert.IsTrue(isExpired);
        }

        /// <summary>
        /// Token is correct, but database has not it
        /// </summary>
        [TestMethod]
        public void VerifyTokenStrategy_CorrectTokenAndMissItInDB() {
            string token = AddToken(user, DateTime.Now, 600);
            entities.Entry(entities.AuthToken.Where(t => t.Token == token).Single()).State = System.Data.Entity.EntityState.Deleted;
            entities.SaveChanges();

            IUserAuthInfo ui = vt.CheckToken(token, entities, out bool isExpired);
            Assert.IsNull(ui);
            Assert.IsFalse(isExpired);
        }

        [TestMethod]
        public void VerifyTokenStrategy_Bot() {
            string token = AddToken(user, DateTime.Now, 600);
            var vtb = new VerifyTokenStrategy(true);

            IUserAuthInfo ui = vtb.CheckToken(token, entities, out bool isExpired);
            Assert.IsNull(ui);

            AuthToken dbtoken = entities.AuthToken.Where(t => t.UserId == Guid.Empty).FirstOrDefault();
            if (dbtoken == null)
                return;
            ui = vtb.CheckToken(dbtoken.Token, entities, out isExpired);
            Assert.IsNotNull(ui);
        }

        private string AddToken(DAL.Model.User u, DateTime dt, int exp) {
            string token = factory.Gen(u.Id, dt, exp);

            AuthToken dbt = user.AuthToken.Where(t => t.Token == token).FirstOrDefault();
            if (dbt == null) {
                dbt = new AuthToken() {
                    UserId = u.Id,
                    IssueDate = dt,
                    ExpireBy = exp,
                    Token = token
                };
                entities.AuthToken.Add(dbt);
                entities.SaveChanges();
            }
            return token;
        }
    }
}
