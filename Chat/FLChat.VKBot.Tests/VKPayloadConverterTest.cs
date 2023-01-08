using System;
using FLChat.VKBotClient.Callback;
using FLChat.VKBotClient.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.VKBot.Tests
{
    [TestClass]
    public class VKPayloadConverterTest
    {
        [TestMethod]
        public void ConvertFromPayloadOK()
        {
            var fileName = "message_new_with_payload";
            string json = System.IO.File.ReadAllText(".\\Json\\Callback\\" + fileName + ".json");            
            var result = JsonConvert.DeserializeObject<CallbackResponse<Message>>(json);
            var mAdapt = new VKMessageAdapter(result.Object);
            string strPayload = "url:www.ya.ru";
            Assert.AreEqual(mAdapt.Command, strPayload);
        }   
        
        [TestMethod]
        public void ConvertFromPayloadOK_Message()
        {
            var fileName = "message_new_with_payload";
            string json = System.IO.File.ReadAllText(".\\Json\\Callback\\" + fileName + ".json");            
            var result = JsonConvert.DeserializeObject<CallbackData>(json);
            var message = ((JObject) result.Object).ToObject<Message>();
            var mAdapt = new VKMessageAdapter(message);
            string strPayload = "url:www.ya.ru";
            Assert.AreEqual(mAdapt.Command, strPayload);
        } 
        
        [TestMethod]
        public void ConvertFromPayloadOK_TypeConformation()
        {
            var fileName = "confirmation";
            string json = System.IO.File.ReadAllText(".\\Json\\Callback\\" + fileName + ".json");            
            var callbackData = JsonConvert.DeserializeObject<CallbackData>(json);
            var handler = new VKUpdateHandler();
            var result = handler.MakeUpdate(null, callbackData);
            var a = result.ToString();
        }

        [TestMethod]
        public void ConvertFromPayloadNO()
        {
            var fileName = "message_new_without_payload";
            string json = System.IO.File.ReadAllText(".\\Json\\Callback\\" + fileName + ".json");
            var result = JsonConvert.DeserializeObject<CallbackResponse<Message>>(json);
            var mAdapt = new VKMessageAdapter(result.Object);
            //string strPayload = "url:www.ya.ru";
            //Assert.AreEqual(mAdapt.Command, strPayload);
            Assert.IsNull(mAdapt.Command);
        }
        [TestMethod]
        public void ConvertToPayload()
        {
            string test = "url:www.ya.ru";
            string strPayload = "{ \"button\" : \"" + $"{test}" + "\" }";
            string strRet = (new VKPayloadConverter(test)).GetJson();
            var ret1 = JObject.Parse(strPayload);
            var ret2 = JObject.Parse(strRet);
            Assert.AreEqual(ret1.ToString(), ret2.ToString());
        }
    }
}
