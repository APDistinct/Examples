using System;
using System.Linq;
using System.Data.Entity;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FLChat.WebService.DataTypes;
using System.Net;

namespace FLChat.WebService.Handlers.Auth.Tests
{
    [TestClass]
    public class GetTokenRefreshTests
    {
        ChatEntities entities;
        GetToken tokenGetter;
        AuthTokenFactory factory;
        DAL.Model.User user;

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
            factory = new AuthTokenFactory();
            tokenGetter = new GetToken(factory);

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
        public void GetTokenRefresh_InvalidToken() {
            try {
                tokenGetter.ProcessRequest(entities, null, new TokenRequest() { Token = "123333" });
                Assert.Fail("Invalid token exception has't thrown");
            } catch (ErrorResponseException e) {
                e.Check(HttpStatusCode.Unauthorized, ErrorResponse.Kind.invalid_auth_token);
            }
        }

        [TestMethod]
        public void GetTokenRefresh_TokenNotExist() {
            string token = factory.Gen(user.Id, DateTime.Now, tokenGetter.ExpireBy);
            try {
                tokenGetter.ProcessRequest(entities, null, new TokenRequest() { Token = token });
                Assert.Fail("Missed token exception has't thrown");
            } catch (ErrorResponseException e) {
                e.Check(HttpStatusCode.Unauthorized, ErrorResponse.Kind.missed_auth_token);
            }
        }

        //can't perform method, because disabled user can't has token
        //[TestMethod]
        //public void GetTokenRefresh_ForDisabledUser() {
        //    User du = entities.GetUser(
        //        u => u.Enabled == false && u.IsConsultant == true,
        //        u => { u.Enabled = false; u.IsConsultant = true; });
        //    string token = AddToken(du, DateTime.Now);

        //    try {
        //        tokenGetter.ProcessRequest(entities, null, new TokenRequest() { Token = token });
        //        Assert.Fail("Disable user has't thrown exception");
        //    } catch(ErrorResponseException e) {
        //        e.Check(HttpStatusCode.Unauthorized, ErrorResponse.Kind.user_not_found);
        //    }
        //}

        [TestMethod]
        public void GetTokenRefresh_ForNotConsultantUser() {
            DAL.Model.User du = entities.GetUser(
                u => u.Enabled == true && u.IsConsultant == false,
                u => { u.Enabled = true; u.IsConsultant = false; });
            string token = AddToken(du, DateTime.Now);

            try {
                tokenGetter.ProcessRequest(entities, null, new TokenRequest() { Token = token });
                Assert.Fail("Non consultant user has't thrown exception");
            } catch (ErrorResponseException e) {
                e.Check(HttpStatusCode.Unauthorized, ErrorResponse.Kind.user_not_found);
            }
        }


        [TestMethod]
        public void GetTokenRefresh_RefreshPeriodGone() {
            string token = AddToken(user, DateTime.Now.AddSeconds(-1 * (100 + tokenGetter.RefreshPeriod + 60)), 100);

            try {
                tokenGetter.ProcessRequest(entities, null, new TokenRequest() { Token = token });
                Assert.Fail("Token refresh period was gone has't thrown exception");
            } catch (ErrorResponseException e) {
                e.Check(HttpStatusCode.Unauthorized, ErrorResponse.Kind.expired);
            }
        }

        [TestMethod]
        public void GetTokenRefresh_Success() {
            string token = AddToken(user, DateTime.Now.AddSeconds(-1 * (tokenGetter.ExpireBy + tokenGetter.RefreshPeriod - 60)));
            TokenResponse newtoken = tokenGetter.ProcessRequest(entities, null, new TokenRequest() { Token = token });

            AuthToken db = entities.AuthToken.Where(t => t.Token == newtoken.Token).Single();
            Assert.AreEqual(user.Id, db.UserId);
            Assert.AreEqual(tokenGetter.ExpireBy, db.ExpireBy);
            Assert.IsTrue((DateTime.Now - db.IssueDate).TotalMinutes < 5);
        }

        private string AddToken(DAL.Model.User u, DateTime dt, int exp = 0) {
            if (exp == 0)
                exp = tokenGetter.ExpireBy;
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
