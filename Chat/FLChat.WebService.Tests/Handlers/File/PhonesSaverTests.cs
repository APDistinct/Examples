using System;
using FLChat.DAL.Model;
using System.Collections.Generic;
using System.Linq;
using FLChat.WebService.Handlers.File;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.WebService.Tests.Handlers.File
{
    [TestClass]
    public class PhonesSaverTests
    {
        ChatEntities entities;
        PhonesSaver handler;
        DAL.Model.User user;
        DAL.Model.User[] users;

        [TestInitialize]
        public void Init()
        {
            entities = new ChatEntities();
            handler = new PhonesSaver();

            user = entities.GetUserQ();
            users = entities.GetUsers(2,
               u => u.Enabled && u.OwnerUserId == user.Id && !string.IsNullOrWhiteSpace(u.Phone),
               u => {
                   u.Enabled = true; u.OwnerUserId = user.Id; u.Phone = Guid.NewGuid().ToString();
               });

        }

        [TestMethod]
        public void PhonesSaveTest()
        {
            var list = users.Select(x => x.Id).ToList();
            handler.Save(entities, user.Id, list);
            entities.Entry(user).Reload();
            entities.Entry(user).Collection(u => u.MatchedPhonesAddr).Load();
            CollectionAssert.AreEquivalent(list, user.MatchedPhonesAddr.Select(m => m.Id).ToList());
        }
    }
}
