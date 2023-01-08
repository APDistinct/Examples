using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL;

namespace FLChat.Core.Media.Tests

{
    [TestClass]
    public class MediaTypeExtentionsTests
    {
        [TestMethod]
        public void GetFileMediaGroupTest_OK()
        {            
            string fileType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            byte[] binfile = { 0x50, 0x4B, 0x03, 0x04 };
            var ret = binfile.GetFileMediaGroup(fileType);
            Assert.IsNotNull(ret);
            Assert.AreEqual(ret.Value, DAL.MediaGroupKind.Document);
        }

        [TestMethod]
        public void GetFileMediaGroupTest_null()
        {
            string fileType = Guid.NewGuid().ToString();
            byte[] binfile = { 0x50, 0x4B, 0x03, 0x04 };
            var ret = binfile.GetFileMediaGroup(fileType);
            Assert.IsNull(ret);            
        }

        [TestMethod]
        public void IsCorrectTypeTest_true()
        {
            string fileType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            byte[] binfile = { 0x50, 0x4B, 0x03, 0x04 };
            bool? ret = binfile.IsCorrectType(fileType);
            Assert.IsNotNull(ret);
            Assert.IsTrue(ret.Value);
        }

        [TestMethod]
        public void IsCorrectTypeTest_false()
        {
            string fileType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            byte[] binfile = { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 };
            bool? ret = binfile.IsCorrectType(fileType);
            Assert.IsNotNull(ret);
            Assert.IsFalse(ret.Value);
        }

        [TestMethod]
        public void IsCorrectTypeTest_null()
        {
            string fileType = Guid.NewGuid().ToString();
            byte[] binfile = { 0x50, 0x4B, 0x03, 0x04 };
            bool? ret = binfile.IsCorrectType(fileType);
            Assert.IsNull(ret);            
        }

        [TestMethod]
        public void GetImageSizeTest_OK()
        {
            string[] fileNames = { "Chat.bmp", "Chat.jpg", "Chat.png" };
            foreach (var fileName in fileNames)
            {
                GetImageSizeTest_OK(fileName);
            }
        }

        [TestMethod]
        public void GetImageSizeTest_OK(string fileName)
        {            
            byte[] binfile = System.IO.File.ReadAllBytes(".\\Image\\" + fileName);
            bool ret = binfile.GetImageSize(out int? width, out int? height);
            Assert.IsTrue(ret);
            Assert.IsNotNull(width); Assert.IsTrue(width > 0);
            Assert.IsNotNull(height); Assert.IsTrue(height > 0);
        }

        [TestMethod]
        public void GetImageSizeTest_NO()
        {
            string fileName = "ChatNotImage.bmp";
            byte[] binfile = System.IO.File.ReadAllBytes(".\\Image\\" + fileName);
            bool ret = binfile.GetImageSize(out int? width, out int? height);
            Assert.IsFalse(ret);
            Assert.IsNull(width);
            Assert.IsNull(height);
            
        }
    }
}
