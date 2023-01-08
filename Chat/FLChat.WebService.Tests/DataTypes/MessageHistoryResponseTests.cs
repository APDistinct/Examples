using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class MessageHistoryResponseTests
    {
        [TestMethod]
        public void MessageHistoryResponse_Order() {
            {
                MessageHistoryResponse resp = new MessageHistoryResponse() { Order = OrderEnum.Ascending };
                string json = JsonConvert.SerializeObject(resp);
                JObject jo = JObject.Parse(json);
                Assert.AreEqual("asc", (string)jo["order"]);
            }

            {
                MessageHistoryResponse resp = new MessageHistoryResponse() { Order = OrderEnum.Descending };
                string json = JsonConvert.SerializeObject(resp);
                JObject jo = JObject.Parse(json);
                Assert.AreEqual("desc", (string)jo["order"]);
            }
        }
    }
}
