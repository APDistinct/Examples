using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL.Model;
using Newtonsoft.Json.Linq;
using FLChat.WebService.DataTypes;
using System.Linq;
using System.Threading;
using FLChat.Core;
using FLChat.Core.Algorithms;
using FLChat.Core.MsgCompilers;
using FLChat.DAL;
using FLChat.TelegramBot;
using FLChat.TelegramBot.Adapters;
using FLChat.TelegramBot.Algorithms;
using FLChat.Viber.Bot;
using FLChat.Viber.Bot.Adapters;
using FLChat.Viber.Bot.Algorithms;
using FLChat.Viber.Sender;
using FLChat.VKBot;
using FLChat.VKBot.Adapters;
using FLChat.VKBotClient.Types;
using FLChat.VKBotClient.Types.Enums;
using FLChat.WebService.Handlers.Message;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace FLChat.WebService.Handlers.User.Tests
{
    [TestClass]
    public class GetUserAvatarTests
    {
        ChatEntities entities;
        CreateUser handler;

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
            handler = new CreateUser();
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        [Ignore("Отключен поскольку содержит персональные пользователи юзера которых может не быть")]
        public void GetUserAvatarFromTelegramTest()
        {
            DAL.Model.User sender = entities.GetUser(u => u.Enabled, null);
            var senderTra = sender.Transports.Get(TransportKind.FLChat);

            JObject input = new JObject() {
                ["phone"] = "380989913825",
                ["full_name"] = "Andrew Andrew"
            };

            UserProfileInfo response = handler.ProcessRequest(entities, sender, input);
            var recipient = entities.User.Where(u => u.Id == response.Id).FirstOrDefault();

            DAL.Model.Transport t = recipient.Transports.Get(TransportKind.Telegram);
            if (t == null) {
                t = new DAL.Model.Transport();
                recipient.Transports.Add(t);
                t.TransportTypeId = (int)TransportKind.Telegram;
                t.Enabled = true;
                t.TransportOuterId = "890198357";
                entities.SaveChanges();

            }

            Telegram.Bot.Types.Message msg = new Telegram.Bot.Types.Message()
            {
                Document = new Document()
                {
                    FileId = "123",
                    FileName = "somename",
                    FileSize = 100,
                    MimeType = "application/pdf",
                    Thumb = null
                }
            };
            var adapter = new TelegramMessageAdapter(msg);

            var client = CreateTelegramClient();
            // Action
            new AvatarLoader(new TelegramAvatarProvider(client)).TryLoadAvatar(entities, adapter, t);

            // Assert
            var changedRecipient = entities.User.FirstOrDefault(f => f.Id == recipient.Id);
            
            Assert.IsNotNull(changedRecipient.UserAvatar);
            Assert.IsNotNull(changedRecipient.UserAvatar.Data);
            Assert.IsTrue(changedRecipient.UserAvatar.Data.Length > 0);
            Assert.AreEqual(2, changedRecipient.UserAvatar.MediaTypeId);
        } 
        
        [TestMethod]
        [Ignore("Отключен поскольку содержит персональные пользователи юзера которых может не быть")]
        public void GetUserAvatarFromVKTest()
        {
            var faker = new Bogus.Faker();

            DAL.Model.User sender = entities.GetUser(u => u.Enabled, null);
            var senderTra = sender.Transports.Get(TransportKind.FLChat);

            JObject input = new JObject() {
                ["phone"] = faker.Random.Number().ToString(),
                ["full_name"] = faker.Name.FullName()
            };

            UserProfileInfo response = handler.ProcessRequest(entities, sender, input);
            var recipient = entities.User.Where(u => u.Id == response.Id).FirstOrDefault();

            DAL.Model.Transport t = recipient.Transports.Get(TransportKind.VK);
            if (t == null) {
                t = new DAL.Model.Transport();
                recipient.Transports.Add(t);
                t.TransportTypeId = (int)TransportKind.VK;
                t.Enabled = true;
                t.TransportOuterId = "890198357";
                entities.SaveChanges();

            }

            var msg = new FLChat.VKBotClient.Types.Message();
            var adapter = new VKMessageAdapter(msg);

            var client = CreateVKClient();
            // Action
            new AvatarLoader(new VKAvatarProvider(client)).TryLoadAvatar(entities, adapter, t);

            // Assert
            var changedRecipient = entities.User.FirstOrDefault(f => f.Id == recipient.Id);
            
            Assert.IsNotNull(changedRecipient.UserAvatar);
            Assert.IsNotNull(changedRecipient.UserAvatar.Data);
            Assert.IsTrue(changedRecipient.UserAvatar.Data.Length > 0);
            Assert.IsTrue(changedRecipient.UserAvatar.MediaTypeId > 0);
        }

        [TestMethod]
        [Ignore("Отключен поскольку содержит персональные пользователи юзера которых может не быть")]
        public void GetUserAvatarFromViberTest()
        {
            // Arrange
            var faker = new Bogus.Faker();

            var recipient = new DAL.Model.User()
            {
                Phone = faker.Random.Number().ToString(),
                FullName = faker.Name.FullName(),
                UserAvatar = null,
                Enabled = true
            };
            entities.User.Add(recipient);

            var transport = new DAL.Model.Transport();
            transport.TransportTypeId = (int)TransportKind.Viber;
            transport.Enabled = true;
            transport.TransportOuterId = faker.Random.String();
            recipient.Transports.Add(transport);

            entities.SaveChanges();

            
            Assert.IsNull(recipient.UserAvatar);

            ViberMessageAdapter adaptedMessage = new ViberMessageAdapter(new FLChat.Viber.Client.Types.CallbackData() {
                Event = FLChat.Viber.Client.Types.CallbackEvent.Message,
                Message = new FLChat.Viber.Client.Types.Message() {
                    Type = FLChat.Viber.Client.Types.MessageType.Text
                },
                User = new FLChat.Viber.Client.Types.User() {Avatar = faker.Image.PicsumUrl()}
            });
            var fileLoader = new FileLoaderByUrl(); 

            // Action
            new AvatarLoader(new ViberAvatarProvider()).TryLoadAvatar(entities, adaptedMessage, transport);

            // Assert
            var changedRecipient = entities.User.FirstOrDefault(f => f.Id == recipient.Id);
            
            Assert.IsNotNull(changedRecipient.UserAvatar);
            Assert.IsNotNull(changedRecipient.UserAvatar.Data);
            Assert.IsTrue(changedRecipient.UserAvatar.Data.Length > 0);
            Assert.IsTrue(changedRecipient.UserAvatar.MediaTypeId > 0);
        }


        private TelegramClient CreateTelegramClient()
        {
            string token = "986521594:AAH47EhhJT1yB013VJ_Lz3_s07cMhxPjM6M";
            string proxy = "185.17.120.252";
            var port = 1080;
            string usr = "tguser";
            string psw = "d72c76bcf19db9aa458a9862cdca9f3b";

            TelegramClient client = new TelegramClient(token, proxy, port, usr, psw);
            client.Log.OnLogError += (s, e) => {
                Console.Write("Write transport log error: ", e);
            };
            return client;
        }

        private VKClient CreateVKClient()
        {
            string token = "0be1ca4d1f355216067f88a68a3f57a5a8267d3b2b336e5fea1cf6235880203f118c9e346dbf173f07271";
            
            VKClient client = new VKClient(token);
            client.Log.OnLogError += (s, e) => {
                Console.Write("Write transport log error: ", e);
            };
            return client;
        }
    }
}
