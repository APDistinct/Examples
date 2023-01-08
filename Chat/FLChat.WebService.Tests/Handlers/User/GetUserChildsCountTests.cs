using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL.Model;
using System.Linq;
using System.Net;

namespace FLChat.WebService.Handlers.User.Tests
{
    [TestClass]
    public class GetUserChildsCountTests
    {
        ChatEntities entities;
        GetUserChildsCount handler = new GetUserChildsCount();

        DAL.Model.User _user;

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();

            _user = entities.GetUserQ(
                where: q => q.Where(u => u.OwnerUserId != null && u.ChildUsers.Any()),
                create: u => {
                    u.OwnerUser = entities.GetUserQ();
                    u.ChildUsers.Add(entities.GetUserQ(where: q => q.Where(u2 => u2.Id != u.OwnerUser.Id)));
                }
                );
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void GetUserChildsCount_UserId() {
            var resp = handler.ProcessRequest(entities, null, _user.Id.ToString());
            Assert.AreEqual(_user.Id, resp.UserId);
            Assert.AreEqual(entities.User_GetChilds(_user.Id, null, null).Count(), resp.Count);
        }

        [TestMethod]
        public void GetUserChildsCount_Profile() {
            var resp = handler.ProcessRequest(entities, _user, null);
            Assert.AreEqual(_user.Id, resp.UserId);
            Assert.AreEqual(entities.User_GetChilds(_user.Id, null, null).Count(), resp.Count);
        }

        [TestMethod]
        public void GetUserChildsCount_InvalidGuid() {
            var e = Assert.ThrowsException<ErrorResponseException>(() => handler.ProcessRequest(entities, _user, "123"));
            Assert.AreEqual((int)HttpStatusCode.BadRequest, e.GetHttpCode());
            Assert.AreEqual(ErrorResponse.Kind.uri_key_error, e.Error.Error);            
        }
    }
}
