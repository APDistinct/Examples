using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL;
using System.Collections.Generic;
using FLChat.DAL.Model;

using FLChat.Core;
using FLChat.Core.Buttons;
using FLChat.Core.Routers;

namespace FLChat.Viber.Bot.Routers.Tests
{
    [TestClass]
    public class ButtonUrlMessageToSystemBotTests
    {
        const string url = @"https://faberlic.com/index.php?option=com_articles&view=category&id=1000289777314";

        //class Buttons : ITransportButtonsSource
        //{
        //    public IEnumerable<IEnumerable<ITransportButton>> GetButtons(MessageToUser mtu) {
        //        var res = new List<List<ITransportButton>>();
        //        res.Add(new List<ITransportButton>());
        //        res[0].Add(new TransportButton(new ExternalTransportButton() { Command = @"url:https://faberlic.com/index.php?option=com_articles&view=category&id=1000289777314" }));
        //        return res;
        //    }
        //}

        ChatEntities entities;

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
            ExternalTransportButton btn = entities.ExternalTransportButton
                .Where(b => b.Command.StartsWith(BotCommandsRouter.URL_PREFIX + "http")).FirstOrDefault();
            if (btn == null) {
                entities.ExternalTransportButton.Add(new ExternalTransportButton() {
                    Caption = "test",
                    Command = BotCommandsRouter.URL_PREFIX + url,
                });
                entities.SaveChanges();
            }
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void ButtonUrlMessageToSystemBot_Url() {
            ButtonUrlMessageToSystemBot router = new ButtonUrlMessageToSystemBot();//new Buttons());
            Guid? guid = router.RouteMessage(entities, new FakeOuterMessage() { Text = url }, null);
            Assert.IsNotNull(guid);
            Assert.AreEqual(Global.SystemBotId, guid.Value);
            Assert.IsNull(router.RouteMessage(entities, new FakeOuterMessage() { Text = @"https://faberlic.com/" }, null));
            Assert.IsNull(router.RouteMessage(entities, new FakeOuterMessage() { Text = "some text" }, null));
        }
    }
}
