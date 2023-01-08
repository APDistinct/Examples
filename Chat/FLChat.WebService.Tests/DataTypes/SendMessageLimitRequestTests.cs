using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class SendMessageLimitRequestTests
    {
        [TestMethod]
        public void SendMessageLimitRequest_FieldNames() {
            SendMessageLimitRequest request = new SendMessageLimitRequest() { };
            string json = JsonConvert.SerializeObject(request);
            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(new string[] { "type", "selection" }, 
                jo.Properties().Select(p => p.Name).ToArray());
        }
    }
}
