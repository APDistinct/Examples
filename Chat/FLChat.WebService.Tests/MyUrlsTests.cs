using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.WebService.Tests
{
    [TestClass]
    public class MyUrlsTests
    {
        const string BA = "http://localhost/FLChat";

        private static Uri u(string addr) {
            return new Uri(BA + addr);
        }

        [TestMethod()]
        public void MyUrlsTest() {
            MyUrls urls = new MyUrls(BA);
            string key1;
            Uri uri;

            uri = u("/users/10");
            Assert.IsTrue(urls.IsUser(uri, out key1));
            Assert.AreEqual("10", key1);
        }

        [TestMethod()]
        public void UserAvatarTest()
        {
            MyUrls urls = new MyUrls(BA);
            string key1;
            Uri uri;

            uri = u("/users/10/avatar");
            Assert.IsTrue(urls.IsUserAvatar(uri, out key1));
            Assert.AreEqual("10", key1);
        }
    }
}
