using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.WebService.Handlers.Auth.Tests
{
    [TestClass]
    public class AuthTokenFactoryTests
    {
        AuthTokenFactory f = new AuthTokenFactory();

        [TestMethod]
        public void AuthTokenFactory_CodeDecode() {
            Guid id = Guid.NewGuid();
            DateTime dt = DateTime.Now;
            int expireBy = 1000;
            string token = f.Gen(id, dt, expireBy);

            Assert.IsFalse(String.IsNullOrWhiteSpace(token));
            Assert.IsTrue(token.Length > 10 && token.Length <= 255);

            TokenPayload tokenInfo = f.Decode(token);
            Assert.AreEqual(id, tokenInfo.UserId);
            Assert.AreEqual(dt, tokenInfo.Iss);
            Assert.AreEqual(expireBy, tokenInfo.Exp);
        }

        [TestMethod]
        public void AuthTokenFactory_InvalidToken() {
            Guid id = Guid.NewGuid();
            DateTime dt = DateTime.Now;
            int expireBy = 1000;
            string token = f.Gen(id, dt, expireBy);
            int halflen = token.Length / 2;
            //spoil token
            string badToken = token.Substring(0, halflen) + (token[halflen] == '3' ? "4" : "3") + token.Substring(halflen + 1);
            Assert.AreEqual(badToken.Length, token.Length);

            try {
                f.Decode(badToken);
                Assert.Fail("Must be fail on invalid token");
            } catch (Exception) {
            }
        }
    }
}
