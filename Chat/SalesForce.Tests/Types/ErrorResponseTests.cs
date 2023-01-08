using System;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SalesForce.Types.Tests
{
    [TestClass]
    public class ErrorResponseTests
    {
        [TestMethod]
        public void FieldNames() {
            ErrorResponse resp = new ErrorResponse() { Error = "1", Description = "2" };
            string json = JsonConvert.SerializeObject(resp);
            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(
                new string[] { "error", "error_description" },
                jo.Properties().Select(p => p.Name).ToArray());
        }

        [TestMethod]
        public void Deserialize() {
            string json = File.ReadAllText(".\\json\\ErrorResponse.json");
            ErrorResponse err = JsonConvert.DeserializeObject<ErrorResponse>(json);
            Assert.AreEqual("invalid_grant", err.Error);
            Assert.AreEqual("authentication failure", err.Description);
        }
    }
}
