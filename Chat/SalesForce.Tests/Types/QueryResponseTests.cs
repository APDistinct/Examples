using System;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SalesForce.Types.Tests
{
    [TestClass]
    public class QueryResponseTests
    {
        [TestMethod]
        public void FieldNames() {
            QueryResponse<JObject> resp = new QueryResponse<JObject>() {
                Done = true,
                TotalSize = 19,
                NextRecordsUrl = "qqqddd",
                Records = new JObject[] { }
            };
            string json = JsonConvert.SerializeObject(resp);
            JObject jo = JObject.Parse(json);

            CollectionAssert.AreEquivalent(
                new string[] { "totalSize", "done", "records", "nextRecordsUrl" },
                jo.Properties().Select(p => p.Name).ToArray());
        }

        [TestMethod]
        public void Deserialize() {
            string json = File.ReadAllText(".\\json\\QueryResponse.json");
            QueryResponse<JObject> resp = JsonConvert.DeserializeObject<QueryResponse<JObject>>(json);
            Assert.IsTrue(resp.Done);
            Assert.AreEqual(19, resp.TotalSize);
            Assert.AreEqual(@"/services/data/v20.0/query/01gD0000002HU6KIAW-2000", resp.NextRecordsUrl);
            Assert.IsNotNull(resp.Records);
            Assert.AreEqual(19, resp.Records.Length);
        }
    }
}
