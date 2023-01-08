using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telegram.Bot.Types;
using FLChat.DAL;

namespace FLChat.TelegramBot.Adapters.Tests
{
    [TestClass]
    public class TelegramPhotoAdapterTests
    {
        [TestMethod]
        public void TelegramPhotoAdapter_Test() {
            Message msg = new Message() {
                Photo = new PhotoSize[] {
                    new PhotoSize() { FileId = "123", Height = 120, Width = 200, FileSize = 100 },
                    new PhotoSize() { FileId = "124", Height = 240, Width = 400, FileSize = 200 },
                }
            };
            TelegramPhotoAdapter adapter = new TelegramPhotoAdapter(msg);
            Assert.AreEqual(MediaGroupKind.Image, adapter.Type);
            Assert.AreEqual("124", adapter.Media);
            Assert.AreEqual("124", adapter.FileName);
            Assert.IsNull(adapter.MediaType);
        }

        [TestMethod]
        public void TelegramPhotoAdapter_WrongType() {
            Message msg = new Message();
            Assert.ThrowsException<ArgumentException>(() => new TelegramPhotoAdapter(msg));
        }
    }
}
