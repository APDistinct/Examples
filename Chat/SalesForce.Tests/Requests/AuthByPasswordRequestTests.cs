using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SalesForce.Requests.Tests
{
    [TestClass]
    public class AuthByPasswordRequestTests
    {
        [TestMethod]
        public void AuthByPasswordRequest() {
            AuthByPasswordRequest req = new AuthByPasswordRequest("clid", "secret1", "user2", "psw3");
            Assert.AreEqual(System.Net.Http.HttpMethod.Post, req.Method);
            Assert.AreEqual("/services/oauth2/token", req.MethodName);
            Assert.IsNull(req.RequestBody);
            Assert.IsNotNull(req.QueryParams);
            CollectionAssert.AreEquivalent(
                new string[] { "client_id", "client_secret", "grant_type", "username", "password" },
                req.QueryParams.Keys.ToArray()
                );
            Assert.AreEqual("clid", req.QueryParams["client_id"]);
            Assert.AreEqual("secret1", req.QueryParams["client_secret"]);
            Assert.AreEqual("user2", req.QueryParams["username"]);
            Assert.AreEqual("psw3", req.QueryParams["password"]);
            Assert.AreEqual("password", req.QueryParams["grant_type"]);
        }
    }
}
