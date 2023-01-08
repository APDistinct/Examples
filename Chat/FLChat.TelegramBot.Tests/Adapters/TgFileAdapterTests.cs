using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.TelegramBot.Adapters.Tests
{
    [TestClass]
    public class TgFileAdapterTests
    {
        [TestMethod]
        public void FileName() {
            TelegramMessageAdapter msg = ResourceHelper.Read("message_with_file");
            Telegram.Bot.Types.File tgFile = ResourceHelper.ReadFile("file");

            TgFileAdapter adapter = new TgFileAdapter(msg.File, tgFile);

            Assert.AreEqual("file_0.jpg", adapter.FileName);
            Assert.AreEqual(msg.File.Media, adapter.Media);
            Assert.IsNull(adapter.MediaType);
            Assert.AreEqual(DAL.MediaGroupKind.Image, adapter.Type);
        }
    }
}
