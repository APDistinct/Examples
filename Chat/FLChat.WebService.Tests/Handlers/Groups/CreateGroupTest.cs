using System;
using System.Linq;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using FLChat.WebService.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.WebService.Tests.Handlers.Groups
{
    [TestClass]
    public class CreateGroupTest
    {

        ChatEntities entities;
        DAL.Model.User[] users;

        [TestInitialize]
        public void Init()
        {
            entities = new ChatEntities();

            users = entities.GetUsers(3, u => u.Enabled , u => {});
        }

        [TestCleanup]
        public void Clean()
        {
            entities.Dispose();
        }

        [TestMethod]
        public void TestMethod1()
        {
            //string str = "{\"name\": \"Test\", \"is_equal\": \"0\",\"members\": [ ";
            //for (int i = 1; i < users.Count(); ++i)
            //{
            //    str += "\"" + users[i].Id.ToString() + "\", ";
            //}
            //str = str.Substring(0, str.Length - 1);
            //str += "]}";

            GroupInfoSet groupInfoSet = new GroupInfoSet();
            Guid[] guids = new Guid[2];
            guids[0] = users[1].Id;
            guids[1] = users[2].Id;
            groupInfoSet.Members = guids;
            groupInfoSet.Id = users[0].Id.ToString();
            groupInfoSet._commonInfo.Add("name", "Test");
            groupInfoSet._commonInfo.Add("is_equal", false);
            CreateGroup cgr = new CreateGroup();
            var retGr = cgr.ProcessRequest(entities, users[0], groupInfoSet);
            Assert.AreEqual(retGr.Name, "Test");
            Assert.AreEqual(retGr.IsEqual, false);
            //CollectionAssert.AreEqual(retGr.Members, groupInfoSet.Members);
            Assert.IsNotNull(retGr.Admins);
        }
    }
}
 
