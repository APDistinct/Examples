using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL.Model;
using FLChat.DAL;

namespace FLChat.WebService.Handlers.User.Tests
{
    [TestClass]
    public class SearchUserAllTests
    {
        ChatEntities entities;
        SearchUserAll handler = new SearchUserAll();

        DAL.Model.User user;
        //DAL.Model.User child;

        [TestInitialize]
        public void Init()
        {
            entities = new ChatEntities();

            user = entities.GetUser(null,u => {}, TransportKind.FLChat);            
        }

        [TestCleanup]
        public void Clean()
        {
            entities.Dispose();
        }

        [TestMethod]
        public void SearchUserAll_Search()
        {
            string value = Guid.NewGuid().ToString();
            var users = entities.GetUsers(2,
                c => c.Id != user.Id && !c.Enabled && c.FullName != null && c.FullName.Contains(value),
                c => { c.FullName = value + Guid.NewGuid().ToString(); c.Enabled = false; });
            var reusers = users.Concat(entities.GetUsers(2,
                c => c.Id != user.Id && c.Enabled && c.FullName != null && c.FullName.Contains(value),
                c => { c.FullName = value + Guid.NewGuid().ToString(); c.Enabled = true; }));
            
            var response = handler.ProcessRequest(entities, user, new DataTypes.SearchUserRequest()
            {
                SearchValue = value
            });
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.UserList);
            Assert.IsTrue(response.UserList.Any());

            foreach (var user in response.UserList.Select(u => u._user))
            {
                Assert.IsTrue((user.FullName != null && user.FullName.Contains(value))
                    || (user.Phone != null && user.Phone.Contains(value))
                    || (user.Email != null && user.Email.Contains(value)));
            }
            int numtrue = reusers.Where(x => x.Enabled == true).Count();
            int numfalse = reusers.Where(x => x.Enabled == false).Count();
            Assert.IsTrue(numtrue > 1);
            Assert.IsTrue(numfalse > 1);
        }

        [TestMethod]
        public void SearchUserAll_Partial()
        {
            DAL.Model.User[] users =  entities.GetUsers(5, 
                c => c.Id != user.Id && c.FullName != null && c.FullName.Length > 5,
                c => c.FullName = Guid.NewGuid().ToString());
            string value = users[0].FullName.Substring(0, 5);
            
            DataTypes.SearchUserRequest input = new DataTypes.SearchUserRequest()
            {
                Count = 2,
                SearchValue = value
            };

            var resp = handler.ProcessRequest(entities, user, input);
            Assert.AreEqual(input.Count, resp.UserList.Count());
            Assert.AreEqual(handler.MaxCount, resp.MaxCount);
            Assert.AreEqual(0, resp.Offset);
            Assert.AreEqual(input.Count, resp.RequestedCount);
            Assert.IsNotNull(resp.TotalCount);
            Assert.AreNotEqual(0, resp.TotalCount);
            Guid[] firstIds = resp.UserList.Select(u => u.Id).ToArray();

            input = new DataTypes.SearchUserRequest()
            {
                Count = 2,
                Offset = 2,
                SearchValue = value
            };
            resp = handler.ProcessRequest(entities, user, input);
            Assert.AreEqual(input.Count, resp.UserList.Count());
            Assert.AreEqual(handler.MaxCount, resp.MaxCount);
            Assert.AreEqual(input.Offset, resp.Offset);
            Assert.AreEqual(input.Count, resp.RequestedCount);
            Assert.IsNull(resp.TotalCount);
            Guid[] secondIds = resp.UserList.Select(u => u.Id).ToArray();

            Assert.AreEqual(0, firstIds.Intersect(secondIds).Count());
        }

    }
}
