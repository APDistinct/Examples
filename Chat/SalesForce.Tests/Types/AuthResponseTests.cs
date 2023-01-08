using System;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SalesForce.Types.Tests
{
    [TestClass]
    public class AuthResponseTests
    {
        [TestMethod]
        public void FieldNames() {
            AuthResponse resp = new AuthResponse() {
                AccessToken = "123",
                Id = "",
                InstanceUrl = "",
                IssuedAt = "",
                Signature = "",
                TokenType = "Bearer"
            };
            string json = JsonConvert.SerializeObject(resp);
            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(
                new string[] { "access_token", "instance_url", "id", "token_type", "issued_at", "signature" },
                jo.Properties().Select(p => p.Name).ToArray());
        }

        [TestMethod]
        public void Deserialize() {
            string json = File.ReadAllText(".\\json\\AuthResponse.json");
            AuthResponse resp = JsonConvert.DeserializeObject<AuthResponse>(json);
            Assert.AreEqual("123", resp.AccessToken);
            Assert.AreEqual("https://vicadotest.my.salesforce.com", resp.InstanceUrl);
            Assert.AreEqual("https://login.salesforce.com/id/00D6g0000022hCUEAY/0056g000001XZyTAAW", resp.Id);
            Assert.AreEqual("Bearer", resp.TokenType);
            Assert.AreEqual("sD1n6lvJuvWB11hIOSzviI7Lvx8Fi4isUINXhRclOZo=", resp.Signature);
        }
    }
}
