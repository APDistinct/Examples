using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL.Model;
using FLChat.Viber.Client.Requests;

namespace FLChat.Viber.Bot.Tests
{
    [TestClass]
    public class FileInfoExtentions
    {
        [TestMethod]
        public void FileInfoExtentions_IsViberPicture() {
            FileInfo fi = new FileInfo() {
                MediaType = new MediaType() {
                    Name = "image/jpeg"
                },
                FileLength = 100
            };
            Assert.IsTrue(fi.IsViberPicture());
            fi.FileLength = SendPictureMessageRequest.MaxImageSize + 1;
            Assert.IsFalse(fi.IsViberPicture());
            fi.FileLength = SendPictureMessageRequest.MaxImageSize;
            Assert.IsTrue(fi.IsViberPicture());
            fi.MediaType.Name = "image/png";
            Assert.IsFalse(fi.IsViberPicture());
        }
    }
}
