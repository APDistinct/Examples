using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using System.Net;

namespace FLChat.WebService.Handlers.Auth.Tests
{
    [TestClass]
    public class GetTokenSmsCodeTests
    {
        ChatEntities entities;
        FakeGenerator fakeGenerator = new FakeGenerator();
        GetToken tokenGetter;
        DAL.Model.User user;

        public class FakeGenerator : ITokenRecoverFactory<TokenPayload>
        {
            public string LastToken { get; private set; }

            public TokenPayload Decode(string token) {
                string[] parts = token.Split('_');
                return new TokenPayload() {
                    UserId = Guid.Parse(parts[0]),
                    Iss = DateTime.Parse(parts[1]),
                    Exp = int.Parse(parts[2])
                };
            }

            public string Gen(Guid id, DateTime issueDate, int expireBy) {
                LastToken = id.ToString() + "_" + Guid.NewGuid() + "_" + expireBy.ToString();
                return LastToken;
            }
        }

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
            tokenGetter = new GetToken(fakeGenerator);

            user = entities.GetUser(
                u => u.Enabled && u.IsConsultant && u.Phone != null,
                u => {
                    u.Enabled = true;
                    u.IsConsultant = true;
                    u.Phone = new Random().Next(1000000).ToString();
                });
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void GetToken_CheckInput() {
            try {
                tokenGetter.ProcessRequest(entities, null, new TokenRequest() { Phone = "", SmsCode = "1111" });
                Assert.Fail("Has't failed on empty phone");
            } catch (ErrorResponseException e) {
                Assert.AreEqual((int)HttpStatusCode.BadRequest, e.GetHttpCode());
                Assert.AreEqual(ErrorResponse.Kind.input_data_error, e.Error.Error);
            }

            try {
                tokenGetter.ProcessRequest(entities, null, new TokenRequest() { Phone = "111", SmsCode = "a" });
                Assert.Fail("Has't failed on invalid sms code");
            } catch (ErrorResponseException e) {
                Assert.AreEqual((int)HttpStatusCode.BadRequest, e.GetHttpCode());
                Assert.AreEqual(ErrorResponse.Kind.input_data_error, e.Error.Error);
            }
        }

        [TestMethod]
        public void GetToken_SmsCodeNotFound() {
            entities.Entry(user).Reload();
            CreateSmsCode(user);

            try {
                tokenGetter.ProcessRequest(entities, null, new TokenRequest() {
                    Phone = user.Phone,
                    SmsCode = (user.SmsCode.Code + 1).ToString() });
                Assert.Fail("Exception has not thrown on unknown sms code");
            } catch (ErrorResponseException e) {
                Assert.AreEqual((int)HttpStatusCode.BadRequest, e.GetHttpCode());
                Assert.AreEqual(ErrorResponse.Kind.not_found, e.Error.Error);
            }
        }

        [TestMethod]
        public void GetToken_SmsCodeExpired() {
            entities.Entry(user).Reload();
            CreateSmsCode(user);

            user.SmsCode.IssueDate = user.SmsCode.IssueDate.AddSeconds(-1 * (user.SmsCode.ExpireBySec + 1));
            entities.SaveChanges();
            

            try {
                tokenGetter.ProcessRequest(entities, null, new TokenRequest() {
                    Phone = user.Phone,
                    SmsCode = user.SmsCode.Code.ToString() });
                Assert.Fail("Exception has not thrown on expired sms code");
            } catch (ErrorResponseException e) {
                Assert.AreEqual((int)HttpStatusCode.BadRequest, e.GetHttpCode());
                Assert.AreEqual(ErrorResponse.Kind.expired, e.Error.Error);
            }
        }

        [TestMethod]
        public void GetToken_SmsCodeSuccess() {
            entities.Entry(user).Reload();
            CreateSmsCode(user);

            user.SmsCode.IssueDate = DateTime.Now;
            entities.SaveChanges();

            TokenResponse responce = tokenGetter.ProcessRequest(entities, null, new TokenRequest() {
                Phone = user.Phone,
                SmsCode = user.SmsCode.Code.ToString()
            });
            Assert.IsNotNull(responce);
            Assert.AreEqual(fakeGenerator.LastToken, responce.Token);

            //check database
            SmsCode code = entities.SmsCode.Where(c => c.UserId == user.Id).FirstOrDefault();
            Assert.IsNull(code); //sms code was deleted
            AuthToken dbtoken = entities.AuthToken.Where(t => t.Token == fakeGenerator.LastToken).Single();
            Assert.AreEqual(user.Id, dbtoken.UserId);
            Assert.AreEqual(tokenGetter.ExpireBy, dbtoken.ExpireBy);
            Assert.IsTrue((DateTime.Now - dbtoken.IssueDate).TotalMinutes < 1);
            Assert.AreEqual(fakeGenerator.LastToken, dbtoken.Token);
        }

        [TestMethod]
        public void GetTokenBySms_UpdatePsw_AndLoginByPsw() {
            DAL.Model.User user = entities.GetUser(
                u => u.PswHash == null && u.Phone != null && u.IsConsultant,
                u => { u.Phone = new Random().Next(1000000).ToString(); u.IsConsultant = true; });
            CreateSmsCode(user);

            string code = user.SmsCode.Code.ToString();

            TokenRequest request = new TokenRequest() {
                Phone = user.Phone,
                SmsCode = code,
            };

            try {
                TokenResponse responce = tokenGetter.ProcessRequest(entities, null, request);

                entities.Entry(user).Reload();
                Assert.IsNotNull(user.PswHash);
                Assert.IsNull(user.SmsCode);

                //System.Threading.Tasks.Task.Delay(1000);

                responce = tokenGetter.ProcessRequest(entities, null, request);
                Assert.IsNotNull(responce.Token);
            } finally {
                user.PswHash = null;
                entities.SaveChanges();
            }
        }

        private void CreateSmsCode(DAL.Model.User user) {
            SmsCode sms = entities.SmsCode.Where(s => s.UserId == user.Id).FirstOrDefault();
            if (sms != null) {
                entities.Entry(sms).State = System.Data.Entity.EntityState.Deleted;
                entities.SaveChanges();
            }

            user.SmsCode = new SmsCode() {
                Code = new Random().Next(0, 999999),
                ExpireBySec = 1000,
                IssueDate = DateTime.Now,
                UserId = user.Id
            };
            entities.SaveChanges();
            entities.Entry(user).Reload();
        }
    }
}
