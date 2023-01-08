using System;
using FLChat.DAL;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.SmsBott.Tests
{
    [TestClass]
    public class SmsBotTest
    {
        SmsBot handler = new SmsBot();

        [TestMethod]
        public void SmsBotCreateTest()
        {
            Assert.AreEqual(handler.SmsBotUser.UserId, Global.SmsBotId);
            Assert.AreEqual(handler.SmsBotTransport.Kind, TransportKind.Sms);
        }

        [TestMethod]
        public void GetAddresseeBotTest()
        {
            string phone = Guid.NewGuid().ToString();
            MessageToUser mtu = new MessageToUser()
            {
              //  ToUserId = Global.SmsBotId,
                ToTransport = handler.SmsBotTransport,
                Message = new Message()
                {
                    Specific = phone,
                    Text = "",
                }
            };
            string retph = handler.GetAddressee(mtu);
            Assert.AreEqual(retph, phone);
        }

        [TestMethod]
        public void GetAddresseeNotBotTest()
        {
            string phone = Guid.NewGuid().ToString();
            MessageToUser mtu = new MessageToUser()
            {
                ToTransport = new Transport()
                {
                    User = new User()
                    {
                        Phone = phone,
                        Id = Guid.NewGuid(),
                    }
                },
                
                Message = new Message()
                {
                    Specific = "111",
                    Text = "",
                }
            };
            string retph = handler.GetAddressee(mtu);
            Assert.AreEqual(retph, phone);
        }

        [TestMethod]
        public void SendSmsMessageTest()
        {
            string phone = Guid.NewGuid().ToString();
            string text = "Test for Bot sending";
            Message mess = handler.SendSmsMessage(phone, text);
            Assert.AreEqual(mess.Text, text);
            Assert.AreEqual(mess.Specific, phone);
            Assert.AreEqual(mess.Kind, MessageKind.Personal);
            Assert.AreEqual(mess.ToTransport.Kind, TransportKind.Sms);
        }
    }
}
