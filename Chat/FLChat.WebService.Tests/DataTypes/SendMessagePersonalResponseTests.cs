using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class SendMessagePersonalResponseTests
    {
        [TestMethod]
        public void SendMessagePersonalResponse_Serialize() {
            SendMessagePersonalResponse resp = new SendMessagePersonalResponse() {
                MessageId = Guid.NewGuid(),
                User = new SendMessagePersonalInfo() {
                    Status = MessageStatus.Sent,
                    ToTransport = DAL.TransportKind.FLChat,
                    ToUser = Guid.NewGuid()
                }
            };
            string json = JsonConvert.SerializeObject(resp);
            JObject jo = JObject.Parse(json);
            Assert.AreEqual(resp.MessageId, (Guid)jo["message_id"]);
            Assert.AreEqual("sent", (string)jo["user"]["status"]);
            Assert.AreEqual("FLChat", (string)jo["user"]["to_transport"]);
            Assert.AreEqual(resp.User.ToUser, (Guid)jo["user"]["user_id"]);
        }
    }
}
