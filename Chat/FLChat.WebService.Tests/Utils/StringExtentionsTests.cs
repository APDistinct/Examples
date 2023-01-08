using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.WebService.Utils.Tests
{
    [TestClass]
    public class StringExtentionsTests
    {
        [TestMethod]
        public void StringExtentions_MediaTypeExtention() {
            Assert.AreEqual("png", "image/png".MediaTypeExtention());
            Assert.AreEqual("jpeg", "image/jpeg".MediaTypeExtention());
            Assert.AreEqual("", "image/".MediaTypeExtention());
            Assert.IsNull("image".MediaTypeExtention());
        }

        [TestMethod]
        public void StringExtentions_CyrilicToLatin()
        {
            string rusL = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            string rusU = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
            string test = rusL + rusU;
            var result = test.CyrilicToLatin();
            foreach(var c in test)
            {
                Assert.IsFalse(result.Contains(c.ToString()));
            }            
        }
    }
}
