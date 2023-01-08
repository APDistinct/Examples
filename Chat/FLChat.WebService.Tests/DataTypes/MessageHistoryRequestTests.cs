using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class MessageHistoryRequestTests
    {
        [TestMethod]
        public void MessageHistoryRequest_Order() {
            string json = "{\"user_id\": \"b9b19eb0-904f-e911-82e7-1c1b0dafbcae\", \"order\": \"asc\"}";
            MessageHistoryRequest req = JsonConvert.DeserializeObject<MessageHistoryRequest>(json);
            Assert.AreEqual(OrderEnum.Ascending, req.Order.Value);

            json = "{\"user_id\": \"b9b19eb0-904f-e911-82e7-1c1b0dafbcae\", \"order\": \"desc\"}";
            req = JsonConvert.DeserializeObject<MessageHistoryRequest>(json);
            Assert.AreEqual(OrderEnum.Descending, req.Order.Value);

            json = "{\"user_id\": \"b9b19eb0-904f-e911-82e7-1c1b0dafbcae\", \"order\": null}";
            req = JsonConvert.DeserializeObject<MessageHistoryRequest>(json);
            Assert.IsNull(req.Order);

            json = "{\"user_id\": \"b9b19eb0-904f-e911-82e7-1c1b0dafbcae\"}";
            req = JsonConvert.DeserializeObject<MessageHistoryRequest>(json);
            Assert.IsNull(req.Order);
        }
    }
}
