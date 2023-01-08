using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class SendMessagePersonalInfoTests
    {
        [TestMethod]
        public void SendMessagePersonalInfo_Serialize() {
            SendMessagePersonalInfo info = new SendMessagePersonalInfo() {
                Status = MessageStatus.Sent,
                ToTransport = DAL.TransportKind.FLChat,
                ToUser = Guid.NewGuid()
            };
            string json = JsonConvert.SerializeObject(info);
            JObject jo = JObject.Parse(json);
            Assert.AreEqual("sent", (string)jo["status"]);
            Assert.AreEqual("FLChat", (string)jo["to_transport"]);
            Assert.AreEqual(info.ToUser, (Guid)jo["user_id"]);
        }
    }
}
