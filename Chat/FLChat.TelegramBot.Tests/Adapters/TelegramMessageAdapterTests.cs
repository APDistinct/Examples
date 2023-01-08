using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telegram.Bot.Types;
using Newtonsoft.Json;
using FLChat.DAL;

namespace FLChat.TelegramBot.Adapters.Tests
{
    [TestClass]
    public class TelegramMessageAdapterTests
    {
        [TestMethod]
        public void TelegramMessageAdapter_Message() {
            TelegramMessageAdapter msg = ResourceHelper.Read("message");

            Assert.AreEqual("461", msg.MessageId);
            Assert.AreEqual("836798453", msg.FromId);
            Assert.AreEqual("Savelyeva Natalya", msg.FromName);
            Assert.AreEqual(TransportKind.Telegram, msg.TransportKind);
            Assert.AreEqual("Мой профиль", msg.Text);
            Assert.IsNull(msg.PhoneNumber);
            Assert.IsNull(msg.DeepLink);
        }

        [TestMethod]
        public void TelegramMessageAdapter_Message_WithDeepLink() {
            TelegramMessageAdapter msg = ResourceHelper.Read("message_deep_link");
            Assert.AreEqual("gEoWf4fQw2dNJYSjB58U", msg.DeepLink);
            Assert.AreEqual("/start gEoWf4fQw2dNJYSjB58U", msg.Text);
        }

        [TestMethod]
        public void TelegramMessageAdapter_Message_Start() {
            TelegramMessageAdapter msg = ResourceHelper.Read("message_start");
            Assert.IsNull(msg.DeepLink);
            Assert.AreEqual("/start", msg.Text);
        }

        [TestMethod]
        public void TelegramMessageAdapter_Message_Reply() {
            Assert.AreEqual("562", ResourceHelper.Read("message_reply").ReplyToMessageId);
            Assert.IsNull(ResourceHelper.Read("message").ReplyToMessageId);
        }
    }
}
