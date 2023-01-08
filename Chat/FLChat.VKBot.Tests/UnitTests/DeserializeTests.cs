using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.VKBotClient.Requests.Available_Methods.Sending_Messages;
using FLChat.VKBotClient.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace FLChat.VKBot.Tests.UnitTests
{
    [TestClass]
    public class DeserializeTests
    {
        [TestMethod]
        public void Deserialize_Send1messageOK()
        {
            var fileName = "Send_1_ message_OK";
            string json = System.IO.File.ReadAllText(".\\Json\\" + fileName + ".json");
            var result = JsonConvert.DeserializeObject<SendMessageResponse>(json);

            Assert.AreEqual("24", result.Id);
        }

        [TestMethod]
        public void Deserialize_Send3messagesErr()
        {
            var fileName = "Send_3_ messages_Err";
            string json = System.IO.File.ReadAllText(".\\Json\\" + fileName + ".json");
            var result = JsonConvert.DeserializeObject<SendMessagesResponse>(json);

            Assert.IsNotNull(result.Error);
        }

        [TestMethod]
        public void Deserialize_Send3messagesOK()
        {
            var fileName = "Send_3_ messages_OK";
            string json = System.IO.File.ReadAllText(".\\Json\\" + fileName + ".json");
            var result = JsonConvert.DeserializeObject<SendMessagesResponse>(json);
            Assert.AreEqual(3, result.Messages.Count);            
        }
    }
}
