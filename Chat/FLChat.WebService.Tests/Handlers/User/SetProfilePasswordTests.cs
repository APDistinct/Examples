using System;
using System.Linq;
using System.Net;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using FLChat.WebService.Handlers.User;
using FLChat.WebService.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.WebService.Handlers.User.Tests
{
    [TestClass]
    public class SetProfilePasswordTests
    {
        public class FakePasswordChecker : IPasswordChecker
        {            
            public void CheckPassword(string psw)
            {
            }
        }

        ChatEntities entities;
            SetProfilePassword handler = new SetProfilePassword(new FakePasswordChecker());

            DAL.Model.User user;

        [TestInitialize]
        public void Init()
        {
            entities = new ChatEntities();
            user = entities.GetUser(u => u.Enabled && u.PswHash != null, u => { u.Enabled = true; u.PswHash = Guid.NewGuid().ToString(); });
        }

        [TestCleanup]
        public void Clean()
        {
            entities.Dispose();
        }

        [TestMethod]
        public void SetProfilePasswordTests_NoOld_OK()
        {
            var oldPswHash = user.PswHash;
            user.PswHash = null;
            string newPsw = new Random().Next().ToString();
            handler.ProcessRequest(entities, user, new PasswordRequest() {Password = newPsw });
            entities.Entry(user).Reload();
            Assert.AreEqual(user.PswHash, newPsw.ComputeMD5());
            user.PswHash = oldPswHash;
            entities.SaveChanges();
        }
        
        [TestMethod]
        public void SetProfilePasswordTests_IsOld_OK()
        {
            var oldPswHash = user.PswHash;
            string oldPsw = new Random().Next().ToString();
            user.PswHash = oldPsw.ComputeMD5();
            string newPsw = new Random().Next().ToString();
            handler.ProcessRequest(entities, user, new PasswordRequest() { Password = newPsw, OldPassword = oldPsw });
            entities.Entry(user).Reload();
            Assert.AreEqual(user.PswHash, newPsw.ComputeMD5());
            user.PswHash = oldPswHash;
            entities.SaveChanges();
        }

        [TestMethod]
        public void SetProfilePasswordTests_IsOld_NO()
        {
            var oldPswHash = user.PswHash;
            string oldPsw = new Random().Next().ToString();
            //user.PswHash = oldPsw.ComputeMD5();
            string newPsw = new Random().Next().ToString();

            var e = Assert.ThrowsException<ErrorResponseException>(() => 
                handler.ProcessRequest(entities, user, new PasswordRequest() { Password = newPsw, OldPassword = oldPsw }));
            Assert.AreEqual((int)HttpStatusCode.BadRequest, e.GetHttpCode());
            Assert.AreEqual(ErrorResponse.Kind.wrong_old_password, e.Error.Error);
        }

        [TestMethod]
        public void SetProfilePasswordTests_BadOld_NO()
        {
            var oldPswHash = user.PswHash;
            //string oldPsw = new Random().Next().ToString();
            //user.PswHash = oldPsw.ComputeMD5();
            string newPsw = new Random().Next().ToString();

            var e = Assert.ThrowsException<ErrorResponseException>(() => 
                handler.ProcessRequest(entities, user, new PasswordRequest() { Password = newPsw }));
            Assert.AreEqual((int)HttpStatusCode.BadRequest, e.GetHttpCode());
            Assert.AreEqual(ErrorResponse.Kind.wrong_old_password, e.Error.Error);
        }
    }
}
