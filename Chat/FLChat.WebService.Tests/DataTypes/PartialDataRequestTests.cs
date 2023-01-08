using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

using FLChat.WebService.Utils;
using FLChat.DAL.Model;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class PartialDataRequestTests {
        [TestMethod]
        public void PartialDataRequest_Deserialize() {
            JObject jobj = new JObject() {
                { "count", "10" },
                { "offset", "20" }
            };
            PartialDataRequest data = JsonConvert.DeserializeObject<PartialDataRequest>(jobj.ToString());
            Assert.AreEqual("10", data.CountString);
            Assert.AreEqual("20", data.OffsetString);
            Assert.AreEqual(10, data.Count);
            Assert.AreEqual(20, data.Offset);
        }

        [TestMethod]
        public void PartialDataRequest_Serialize() {
            PartialDataRequest data = new PartialDataRequest() {
                CountString = "1",
                OffsetString = "2"
            };
            string json = JsonConvert.SerializeObject(data);
            JObject jobj = JObject.Parse(json);
            CollectionAssert.AreEquivalent(new string[] { "count", "offset" }, jobj.Properties().Select(p => p.Name).ToArray());
        }

        [TestMethod]
        public void PartialDataRequest_FieldNames() {
            Type t = typeof(PartialDataRequest);
            Assert.AreEqual("count", t.GetJsonPropertyName(nameof(PartialDataRequest.CountString)));
            Assert.AreEqual("offset", t.GetJsonPropertyName(nameof(PartialDataRequest.OffsetString)));
        }

        [TestMethod]
        public void IPartialDataRequestExtentions_Test() {
            PartialDataRequest data = new PartialDataRequest() {
                CountString = "10",
                OffsetString = "20"
            };
            Assert.AreEqual(10, data.Count);
            Assert.AreEqual(20, data.Offset);

            data.CountString = "a";
            data.OffsetString = "b";
            Assert.ThrowsException<FormatException>(() => data.Count);
            Assert.ThrowsException<FormatException>(() => data.Offset);

            data.CountString = null;
            data.OffsetString = null;
            Assert.IsNull(data.Count);
            Assert.IsNull(data.Offset);
        }

        [TestMethod]
        public void PartialDataIdRequest_Test() {
            PartialDataIdRequest data = new PartialDataIdRequest() {
                CountString = "1",
                OffsetString = "2",
                Ids = "3"
            };
            string json = JsonConvert.SerializeObject(data);
            JObject jobj = JObject.Parse(json);
            CollectionAssert.AreEquivalent(new string[] { "count", "offset", "id" }, jobj.Properties().Select(p => p.Name).ToArray());
        }

        [TestMethod]
        public void PartialData_TakePart() {
            int[] ints = Enumerable.Range(0, 100).ToArray();

            PartialDataRequest data = new PartialDataRequest() {
                Count = 10,
                Offset = 5,
            };

            CollectionAssert.AreEqual(Enumerable.Range(5, 10).ToArray(), ints.TakePart(data).ToArray());
            data.MaxCount = 6;
            CollectionAssert.AreEqual(Enumerable.Range(5, 6).ToArray(), ints.TakePart(data).ToArray());
            data.Count = 3;
            CollectionAssert.AreEqual(Enumerable.Range(5, 3).ToArray(), ints.TakePart(data).ToArray());
            data = new PartialDataRequest();
            CollectionAssert.AreEqual(Enumerable.Range(0, 100).ToArray(), ints.TakePart(data).ToArray());
            data.MaxCount = 20;
            CollectionAssert.AreEqual(Enumerable.Range(0, 20).ToArray(), ints.TakePart(data).ToArray());
            data.Offset = 5;
            CollectionAssert.AreEqual(Enumerable.Range(5, 20).ToArray(), ints.TakePart(data).ToArray());
        }


        [TestMethod]
        public void PartialData_TakePart_EF() {
            using (ChatEntities entities = new ChatEntities()) {
                PartialDataRequest partial = new PartialDataRequest() {
                    Count = 2,
                };
                var f = entities.User.OrderBy(u => u.FullName).TakePart(partial).ToArray();
                Assert.AreEqual(2, f.Length);

                partial.Offset = 2;
                var s = entities.User.OrderBy(u => u.FullName).TakePart(partial).ToArray();
                Assert.AreEqual(2, s.Length);

                Assert.AreEqual(0, f.Select(u => u.Id).Intersect(s.Select(u => u.Id)).Count());
            }
        }

        [TestMethod]
        public void PartialDataResponse_Init() {
            PartialDataResponse resp = new PartialDataResponse(new PartialDataRequest() { MaxCount = 100 });
            Assert.AreEqual(100, resp.MaxCount);
            Assert.AreEqual(100, resp.RequestedCount);
            Assert.AreEqual(0, resp.Offset);

            resp = new PartialDataResponse(new PartialDataRequest() { MaxCount = 100, Count = 20, Offset = 40 });
            Assert.AreEqual(100, resp.MaxCount);
            Assert.AreEqual(20, resp.RequestedCount);
            Assert.AreEqual(40, resp.Offset);
        }

        [TestMethod]
        public void PartialDataResponse_Serialize() {
            PartialDataResponse resp = new PartialDataResponse(new PartialDataRequest() { MaxCount = 100, Count = 20, Offset = 40 }) {
                Count = 98
            };
            string json = JsonConvert.SerializeObject(resp);
            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(
                new string[] { "offset", "count", "max_count", "req_count", "total_count" }, 
                jo.Properties().Select(p => p.Name).ToArray());
        }
    }
}
