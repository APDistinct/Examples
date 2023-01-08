using System;
using System.Collections.Generic;
using System.Linq;
using FLChat.DAL.Model;
using FLChat.WebService.Handlers.File;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.WebService.Tests.Handlers.File
{
    [TestClass]
    public class PhonesGetterTests
    {
        ChatEntities entities;
        PhonesGetter handler;
        DAL.Model.User user;
        DAL.Model.User[] users;

        [TestInitialize]
        public void Init()
        {
            entities = new ChatEntities();
            handler = new PhonesGetter();

            user = entities.GetUserQ();
            users = entities.GetUsers(2,
               u => u.Enabled && u.OwnerUserId == user.Id && !string.IsNullOrWhiteSpace(u.Phone),
               u => {
                   u.Enabled = true; u.OwnerUserId = user.Id; u.Phone = Guid.NewGuid().ToString();
               });

        }

        [TestCleanup]
        public void Clean()
        {
            entities.Dispose();
        }

        [TestMethod]
        public void GetPhones_OK()
        {
            List<string> list = new List<string>();
            list.Add(users[0].Phone);
            list.Add(users[1].Phone);
            list.Add(Guid.NewGuid().ToString());
            var ret = handler.GetMatchedPhones(entities, user.Id, list)/*.ToList()*/;
            //var ret = rett.ToList();
            Assert.IsTrue(ret.Count() >= users.Count());
            foreach (var g in ret)
            {
                Assert.IsTrue(users.Select(u => u.Id).ToList().Contains(g));
            }
        }

        [TestMethod]
        public void GetPhones_NoUsers()
        {
            List<string> list = new List<string>();
            list.Add(users[0].Phone);
            list.Add(users[1].Phone);
            //list.Add(Guid.NewGuid().ToString());
            var newuser = entities.GetUserQ(where: q => q.Where(u2 => u2.OwnerUserId == null &&
            !entities.User.Where(u => u.OwnerUserId == u2.Id).Any()
            ));
            var ret = handler.GetMatchedPhones(entities, newuser.Id, list)/*.ToList()*/;
            //var ret = rett.ToList();
            Assert.IsTrue(ret.Count() == 0);
        }
    }
}
