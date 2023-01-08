using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.WebService.Tests
{
    [TestClass]
    public class PasswordCheckerTests
    {
        PasswordChecker handler = new PasswordChecker();
        [TestMethod]
        public void PasswordCheckerTest_Null()
        {
            var e = Assert.ThrowsException<ErrorResponseException>(() => handler.CheckPassword(null));
            Assert.AreEqual((int)HttpStatusCode.BadRequest, e.GetHttpCode());
        }

        [TestMethod]
        public void PasswordCheckerTest_Empty()
        {
            var e = Assert.ThrowsException<ErrorResponseException>(() => handler.CheckPassword(""));
            Assert.AreEqual((int)HttpStatusCode.BadRequest, e.GetHttpCode());
        }

        [TestMethod]
        public void PasswordCheckerTest_Short()
        {
            var e = Assert.ThrowsException<ErrorResponseException>(() => handler.CheckPassword("12345"));
            Assert.AreEqual((int)HttpStatusCode.BadRequest, e.GetHttpCode());
        }
    }


}
