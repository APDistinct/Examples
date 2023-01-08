using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using FLChat.DAL;

namespace FLChat.TelegramBot.Adapters.Tests
{
    [TestClass]
    public class TelegramCallbackDataAdapterTests
    {
        [TestMethod]
        public void TelegramCallbackDataAdapter_Test() {
            TelegramCallbackDataAdapter cb = Read("callback_query");
            Assert.AreEqual(TransportKind.Telegram, cb.TransportKind);
            Assert.AreEqual("454", cb.FromMessageId);
            Assert.AreEqual("836798453", cb.FromUserId);
            Assert.AreEqual("switch:4711fb55-f280-e911-a2c0-9f888bb5fde6", cb.Data);
            Assert.AreEqual("3594021990515615984", cb.Id);
        }

        private TelegramCallbackDataAdapter Read(string fn) {
            string json = System.IO.File.ReadAllText(".\\Json\\" + fn + ".json");
            Update upd = JsonConvert.DeserializeObject<Update>(json);
            return new TelegramCallbackDataAdapter(upd.CallbackQuery);
        }
    }
}
