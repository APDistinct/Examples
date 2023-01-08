using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.DAL.Tests
{
    [TestClass]
    public class NameValueStringExtentionsTests
    {
        [TestMethod]
        public void NameValueStringExtentions_ToDictionary() {
            string origin = "name1=val1;name2;name3=val3;";
            Dictionary<string, string> dict = origin.ToDictionary();
            Assert.AreEqual(3, dict.Count);
            Assert.AreEqual("val1", dict["name1"]);
            Assert.AreEqual(null, dict["name2"]);
            Assert.IsTrue(dict.ContainsKey("name2"));
            Assert.AreEqual("val3", dict["name3"]);

            origin = null;
            Assert.IsNull(origin.ToDictionary());
        }
    }
}
