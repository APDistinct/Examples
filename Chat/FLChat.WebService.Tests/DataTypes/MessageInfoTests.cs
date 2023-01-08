using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class MessageInfoTests
    {
        [TestMethod]
        public void MessageInfo_Serialize() {
            Guid userId = Guid.Empty;
            MessageInfo ev = new MessageInfo(new DAL.Model.Message() {
                FromTransportKind = DAL.TransportKind.FLChat                
                }, userId);
            string jsonString = JsonConvert.SerializeObject(ev);

            JObject json = JObject.Parse(jsonString);
            Assert.AreEqual("FLChat", (string)json["transport"]);
        }

        [TestMethod]
        public void MessageInfo_Income() {
            Guid sender = Guid.NewGuid();
            Guid anotherUser = Guid.NewGuid();
            Assert.AreNotEqual(sender, anotherUser, "Couple of generetad guids are equal! Shit happens");

            DAL.Model.Message msg = new DAL.Model.Message() { FromUserId = sender };

            MessageInfo ev = new MessageInfo(msg, anotherUser);
            Assert.IsTrue(ev.Incoming);

            ev = new MessageInfo(msg, sender);
            Assert.IsFalse(ev.Incoming);
        }
    }
}
