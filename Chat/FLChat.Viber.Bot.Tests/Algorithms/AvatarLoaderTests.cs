using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using FLChat.Core;
using FLChat.Core.Algorithms;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.Viber.Bot.Adapters;
using FLChat.Viber.Bot.Algorithms;
using FLChat.Viber.Bot.Algorithms.Tests;
using FLChat.Viber.Client.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Message = FLChat.DAL.Model.Message;
using MessageType = FLChat.Viber.Client.Types.MessageType;
using User = FLChat.DAL.Model.User;

namespace FLChat.Viber.Bot.Tests.Algorithms
{
    [TestClass]
    public class AvatarLoaderTests
    {
        ChatEntities entities;

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void TryLoadAvatar_WhenMessageHasAvatarUrl_ThenImageShouldBeLoadedAndSetToUser()
        {
            // Arrange
            var faker = new Faker();

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

            ViberMessageAdapter adaptedMessage = new ViberMessageAdapter(new CallbackData() {
                Event = CallbackEvent.Message,
                Message = new Client.Types.Message() {
                    Type = MessageType.Text
                },
                User = new Client.Types.User() {Avatar = faker.Image.PicsumUrl()}
            });
            var fileLoader = new FileLoaderByUrl(); 

            // Action
            new AvatarLoader(new ViberAvatarProvider()).TryLoadAvatar(entities, adaptedMessage, transport);

            // Assert
            var changedRecipient = entities.User.FirstOrDefault(f => f.Id == recipient.Id);
            
            Assert.IsNotNull(changedRecipient.UserAvatar);
            Assert.IsNotNull(changedRecipient.UserAvatar.Data);
            Assert.IsTrue(changedRecipient.UserAvatar.Data.Length > 0);
            Assert.AreEqual(2, changedRecipient.UserAvatar.MediaTypeId);
        }

        [TestMethod]
        public void TryLoadAvatar_WhenMessageDontHaveAvatarUrl_ThenUserAvatarShouldRemainUnchanged()
        {
            // Arrange
            var faker = new Faker();

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

            ViberMessageAdapter adaptedMessage = new ViberMessageAdapter(new CallbackData()
            {
                Event = CallbackEvent.Message,
                Message = new Client.Types.Message()
                {
                    Type = MessageType.Text
                },
                User = new Client.Types.User() {}
            });
            var fileLoader = new FileLoaderByUrl();

            // Action
            new AvatarLoader(new ViberAvatarProvider()).TryLoadAvatar(entities, adaptedMessage, transport);

            // Assert
            var changedRecipient = entities.User.FirstOrDefault(f => f.Id == recipient.Id);

            Assert.IsNull(changedRecipient.UserAvatar);
        }

        [TestMethod]
        public void TryLoadAvatar_WhenAvatarUrlNotAvailable_ThenUserAvatarShouldRemainUnchanged()
        {
            // Arrange
            var faker = new Faker();

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

            ViberMessageAdapter adaptedMessage = new ViberMessageAdapter(new CallbackData()
            {
                Event = CallbackEvent.Message,
                Message = new Client.Types.Message()
                {
                    Type = MessageType.Text
                },
                User = new Client.Types.User() {Avatar = $"http://my.com/avatar-image.jpg"}
            });
            var fileLoader = new FileLoaderByUrl();

            // Action
            new AvatarLoader(new ViberAvatarProvider()).TryLoadAvatar(entities, adaptedMessage, transport);

            // Assert
            var changedRecipient = entities.User.FirstOrDefault(f => f.Id == recipient.Id);

            Assert.IsNull(changedRecipient.UserAvatar);
        }

        [TestMethod]
        public void TryLoadAvatar_WhenUserHasAvatar_ThenItShouldRemainUnchanged()
        {
            // Arrange
            var faker = new Faker();

            var fakeAvatarData = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
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

            recipient.UserAvatar = new UserAvatar()
            {
                UserId = recipient.Id,
                Data = fakeAvatarData,
                MediaTypeId = 2
            };
            entities.SaveChanges();

            ViberMessageAdapter adaptedMessage = new ViberMessageAdapter(new CallbackData()
            {
                Event = CallbackEvent.Message,
                Message = new Client.Types.Message()
                {
                    Type = MessageType.Text
                },
                User = new Client.Types.User() { Avatar = faker.Image.PicsumUrl() }
            });
            var fileLoader = new FileLoaderByUrl();

            // Action
            new AvatarLoader(new ViberAvatarProvider()).TryLoadAvatar(entities, adaptedMessage, transport);

            // Assert
            var changedRecipient = entities.User.FirstOrDefault(f => f.Id == recipient.Id);

            Assert.IsNotNull(changedRecipient.UserAvatar);
            Assert.AreEqual(changedRecipient.UserAvatar.Data, fakeAvatarData);
        }
    }
}
