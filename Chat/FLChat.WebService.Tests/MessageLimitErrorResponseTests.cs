using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FLChat.DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.Tests
{
    [TestClass]
    public class MessageLimitErrorResponseTests
    {
        [TestMethod]
        public void MessageLimitErrorResponse_Serialize() {
            MessageLimitErrorResponse resp = new MessageLimitErrorResponse(new DataTypes.LimitInfo(
                new DAL.Model.MessageType() { Id = (int)MessageKind.Broadcast }, 0, 0));
            string json = JsonConvert.SerializeObject(resp);
            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(
                new string[] { "error", "error_descr", "limit" },
                jo.Properties().Select(p => p.Name).ToArray());
        }
    }
}
