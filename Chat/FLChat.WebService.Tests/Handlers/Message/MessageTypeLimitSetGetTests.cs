using System;
using System.Linq;
using System.Net;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.Handlers.Message.Tests
{
    [TestClass]
    public class MessageTypeLimitSetGetTests
    {
        ChatEntities entities;
        MessageTypeLimitSet handler = new MessageTypeLimitSet();
        DAL.Model.User user;

        [TestInitialize]
        public void Init()
        {
            entities = new ChatEntities();
            user = entities.GetUser(u => u.Enabled , u => { u.Enabled = true; });
        }

        [TestCleanup]
        public void Clean()
        {
            entities.Dispose();
        }

        private string Read(string fn)
        {
            string json = System.IO.File.ReadAllText("./Json/" + fn + ".json");
            return json;
        }        

        [TestMethod]
        public void MessageTypeLimitGet_OK()
        {
            var handlerGet = new MessageTypeLimitGet();
            var dbList = entities.MessageType.ToList();
            
            var retList = handlerGet.ProcessRequest(entities, user, null);

            Assert.IsTrue(retList.MessageType.Count() <= dbList.Count);
            foreach(var ml in retList.MessageType)
            {
                var limit = dbList.Where(x => x.Id == (int)ml.Kind).First();
                Assert.AreEqual(limit.LimitForDay, ml.LimitForDay);
                Assert.AreEqual(limit.LimitForOnce, ml.LimitForOnce);
            }
        }

        [TestMethod]
        public void MessageTypeLimitSet_OK()
        {
            //MessageType type = new MessageType();            
            var type = entities.MessageType.Where(x => x.Id == (int)DAL.MessageKind.Broadcast).FirstOrDefault();
            int? lfd = type.LimitForDay;
            int? lfm = type.LimitForOnce;
            type.LimitForDay = 100;
            type.LimitForOnce = null;
            MessageTypeLimit mtl = new MessageTypeLimit(type);
            var ret = handler.ProcessRequest(entities, user, mtl);
            entities.Entry(type).Reload();
            Assert.AreEqual(type.LimitForDay, 100);
            Assert.IsNull(type.LimitForOnce);
            type.LimitForDay = lfd;
            type.LimitForOnce = lfm;
            entities.SaveChanges();
        }

        [TestMethod]
        public void MessageTypeLimitSet_NO_Personal()
        {
            //MessageType type = new MessageType();            
            var type = entities.MessageType.Where(x => x.Id == (int)DAL.MessageKind.Personal).FirstOrDefault();
            int? lfd = type.LimitForDay;
            int? lfm = type.LimitForOnce;
            type.LimitForDay = 100;
            type.LimitForOnce = null;
            MessageTypeLimit mtl = new MessageTypeLimit(type);
            try
            {
                var ret = handler.ProcessRequest(entities, user, mtl);                
                Assert.Fail($"Not throwing exception on attempt to set for Personal");
            }
            catch (ErrorResponseException e)
            {
                Assert.AreEqual((int)HttpStatusCode.BadRequest, e.GetHttpCode());
            }            
        }

        [TestMethod]
        public void MessageTypeLimitSet_NO_NegativeDay()
        {
            //MessageType type = new MessageType();            
            var type = entities.MessageType.Where(x => x.Id == (int)DAL.MessageKind.Broadcast).FirstOrDefault();
            
            type.LimitForDay = -100;
            type.LimitForOnce = null;
            MessageTypeLimit mtl = new MessageTypeLimit(type);
            try
            {
                var ret = handler.ProcessRequest(entities, user, mtl);
                Assert.Fail($"Not throwing exception on attempt to set negative for LimitForDay");
            }
            catch (ErrorResponseException e)
            {
                Assert.AreEqual((int)HttpStatusCode.BadRequest, e.GetHttpCode());
            }
        }

        [TestMethod]
        public void MessageTypeLimitSet_NO_NegativeOnce()
        {
            //MessageType type = new MessageType();            
            var type = entities.MessageType.Where(x => x.Id == (int)DAL.MessageKind.Broadcast).FirstOrDefault();
            
            type.LimitForOnce = -1;
            MessageTypeLimit mtl = new MessageTypeLimit(type);
            try
            {
                var ret = handler.ProcessRequest(entities, user, mtl);
                Assert.Fail($"Not throwing exception on attempt to set negative for LimitForOnce");
            }
            catch (ErrorResponseException e)
            {
                Assert.AreEqual((int)HttpStatusCode.BadRequest, e.GetHttpCode());
            }
        }

        [TestMethod]
        public void MessageTypeLimitConvert_OK()
        {
            string json = Read("MessageTypeLimit_OK");
            var job = JsonConvert.DeserializeObject<JObject>(json);
            var limit = JsonConvert.DeserializeObject<MessageTypeLimit>(json);
            Assert.IsNotNull(limit.LimitForDay);
            Assert.IsNotNull(limit.LimitForOnce);            
        }

        [TestMethod]
        public void MessageTypeLimitConvert_NO()
        {
            string json = Read("MessageTypeLimit_NO");
            var job = JsonConvert.DeserializeObject<JObject>(json);
            //try
            //{
            //    var limit = JsonConvert.DeserializeObject<MessageTypeLimit>(json);
            //    Assert.Fail($"Not throwing exception on attempt to set password with bad old");
            //}
            //catch (Exception e)
            //{
            //    Assert.ThrowsException()
            //    //Assert.AreEqual((int)HttpStatusCode.Unauthorized, e.GetHttpCode());
            //}
            var e = Assert.ThrowsException<JsonSerializationException>(() => JsonConvert.DeserializeObject<MessageTypeLimit>(json));            
        }
    }
}
