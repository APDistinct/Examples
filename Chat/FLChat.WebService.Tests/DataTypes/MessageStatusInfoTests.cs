using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class MessageStatusInfoTests
    {
        [TestMethod]
        public void MessageStatusInfo_Serialize() {
            MessageStatusInfo ev = new MessageStatusInfo(
                new DAL.Model.Event() {
                    CausedByTransportKind = DAL.TransportKind.FLChat,
                    Message = new DAL.Model.Message() {
                        FromTransportKind = DAL.TransportKind.Telegram
                    }
                });
            string json = JsonConvert.SerializeObject(ev);
            Assert.IsTrue(json.Contains("FLChat"));
        }
    }
}
