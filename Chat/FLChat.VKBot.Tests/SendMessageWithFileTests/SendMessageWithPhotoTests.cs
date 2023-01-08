using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FLChat.VKBotClient.AttachmentManager;
using FLChat.VKBotClient.Types;
using FLChat.VKBotClient.Types.Attachments;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.VKBot.Tests.SendMessageWithFileTests
{
    [TestClass]
    public class SendMessageWithPhotoTests
    {
        public SendMessageWithPhotoTests()
        {
        }

        //[TestMethod]
        //public async Task SendPhoto()
        //{
        //    var fileName = @"Attach/cat.jpg";
        //    var userId = @"534672230";
        //    var data = File.ReadAllBytes(fileName);

        //    var manager = new VkAttachmentManager();
        //    var files = new List<AttachmentFile>()
        //        {new AttachmentFile() {Bytes = data, Name = /*"Cat.jpg"*/"фото.jpg", Type = AttachmentType.Photo}};
        //    var result = await manager.GetPhotoAttachments(files, userId);

        //}

        //[TestMethod]
        //public async Task SendDoc()
        //{
        //    var fileName = @"Attach/cat.jpg";
        //    var userId = @"534672230";
        //    //    @"C://_83351973_ol23-4713-cadairandrewmcconochie.jpg";
        //    var data = File.ReadAllBytes(fileName);

        //    var manager = new VkAttachmentManager();
        //    var files = new List<AttachmentFile>()
        //        {new AttachmentFile() {Bytes = data, Name = "Cat.jpg", Type = AttachmentType.Doc}};
        //    var result = await manager.GetDocAttachments(files, userId);

        //}

    }
}
