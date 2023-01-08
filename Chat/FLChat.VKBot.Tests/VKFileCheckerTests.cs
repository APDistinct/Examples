using System;
using FLChat.VKBotClient.Types;
using FLChat.VKBotClient.Types.Attachments;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.VKBot.Tests
{
    [TestClass]
    public class VKFileCheckerTests
    {
        private string fileName1 = "VK_200_66.bmp";
        private string fileName2 = "VK_2000_6.png";
        private VKFileChecker checker = new VKFileChecker();

        private byte[] ReadFromFile(string fileName)
        {
            return System.IO.File.ReadAllBytes(".\\Image\\" + fileName);
        }

        [TestMethod]
        public void TestMethodBadType()
        {            
            byte[] newData = ReadFromFile(fileName1);
            AttachmentFile file = new AttachmentFile
            {
                Name = fileName1,
                Bytes = newData,
                Type = AttachmentType.Photo
            };
            checker.PhotoCheck(file);
            Assert.AreEqual(file.Type, AttachmentType.Doc);
        }

        [TestMethod]
        public void TestMethodBadSize()
        {
            byte[] newData = ReadFromFile(fileName2);
            AttachmentFile file = new AttachmentFile
            {
                Name = fileName2,
                Bytes = newData,
                Type = AttachmentType.Photo
            };
            checker.PhotoCheck(file);
            Assert.AreEqual(file.Type, AttachmentType.Doc);
        }
    }
}
