using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.VKBotClient.Callback;
using FLChat.VKBotClient.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace FLChat.VKBot.Tests.UnitTests
{
    [TestClass]
    public class CallbackTests
    {
        [TestMethod]
        public void DeserializeCallbackResponse()
        {
            var fileName = "message_new_with_payload";
            string json = System.IO.File.ReadAllText(".\\Json\\Callback\\" + fileName + ".json");
            var response = JsonConvert.DeserializeObject<CallbackResponse>(json);
            var type = MessageTypeHelper.GetType(response.Type);            
            Assert.AreEqual(type, ResponseMessageType.MessageNew);
            var result = JsonConvert.DeserializeObject<CallbackResponse<Message>>(json);             
            Assert.AreEqual(response.GroupId, 179649792);
            string strPayload = "{\"button\":\"url:www.ya.ru\"}";
            Assert.AreEqual(result.Object.Payload, strPayload);
        }        
    }
}
