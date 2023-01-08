using System;
using System.Linq;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.DAL.Model.Tests
{
    [TestClass]
    public class UserDataTests
    {
        ChatEntities entities;
        private string mainKey = "12345";

        [TestInitialize]
        public void Init()
        {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Clean()
        {
            entities.Dispose();
        }

        [TestMethod]
        public void TestMethod1()
        {
            DAL.Model.User user = entities.GetUserQ();
            //string key = "12345";
            string data = "Test string";

            entities.SetUserData(user.Id, mainKey, data);

            //UserData userData = new UserData();
            var list = entities.UserData.Where(x => x.UserId == user.Id).Select(z => z.UserId).ToList();
            Assert.IsTrue(list.Contains(user.Id));
            string ret = entities.GetUserData(user.Id, mainKey);
            Assert.AreEqual(data, ret);
            //var s = entities.SetUserData();
            entities.DelUserData(user.Id, mainKey);
            long? keyId = entities.UserDataKey.Where(x => x.Key == mainKey).Select(z => z.Id).FirstOrDefault();
            Assert.IsNotNull(keyId);
            list = entities.UserData.Where(x => x.UserId == user.Id && x.KeyId == keyId).Select(z => z.UserId).ToList();
            Assert.AreEqual(list.Count(), 0);
            //var d = entities.get
        }
    }
}
