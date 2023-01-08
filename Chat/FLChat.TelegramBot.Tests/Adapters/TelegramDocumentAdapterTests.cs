using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telegram.Bot.Types;
using FLChat.DAL;

namespace FLChat.TelegramBot.Adapters.Tests
{
    [TestClass]
    public class TelegramDocumentAdapterTests
    {
        [TestMethod]
        public void TelegramDocumentAdapterTests_Test() {
            Message msg = new Message() {
                Document = new Document() {
                    FileId = "123",
                    FileName = "somename",
                    FileSize = 100,
                    MimeType = "application/pdf",
                    Thumb = null
                }
            };
            TelegramDocumentAdapter adapter = new TelegramDocumentAdapter(msg);
            Assert.AreEqual(msg.Document.FileId, adapter.Media);
            Assert.AreEqual(msg.Document.FileName, adapter.FileName);
            Assert.AreEqual(msg.Document.MimeType, adapter.MediaType);
            Assert.AreEqual(MediaGroupKind.Document, adapter.Type);
        }

        [TestMethod]
        public void TelegramPhotoAdapter_WrongType() {
            Message msg = new Message();
            Assert.ThrowsException<ArgumentException>(() => new TelegramDocumentAdapter(msg));
        }
    }
}
