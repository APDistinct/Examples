using System;
using FLChat.Core.Algorithms;
using FLChat.Core.Routers;
using FLChat.DAL.Model;
using FLChat.VKBotClient.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.VKBot.Tests
{
    [TestClass]
    public class VKWebhookTest
    {
        //[TestMethod]
        //public void VKUpdateHandlerTest()
        //{
        //    IReceiveUpdateStrategy<ChatEntities, VKMessageAdapter> NewMessageHandler 
        //   = new NewMessageStrategy<VKMessageAdapter>(RouterFactory.CreateDefaultRouters(/*new AskPhoneRouter()*/));
        //    ChatEntities entities = new ChatEntities();
        //    Update update = new Update();
        //    //update.Type = UpdateType.Message;
        //    VKBotClient.Types.Message message = new VKBotClient.Types.Message();
        //    message.Date = DateTime.UtcNow;
        //    message.FromId = 534672230;
        //    message.Id = 33;
        //    message.Ref = "12345";
        //    message.Text = "Tessss  - Тссс!";
        //    message.PeerId = 123456;
        //    message.RandomId = 3456;
        //    update.Message = message;
        //    update.Id = message.Id;
        //    NewMessageHandler.Process(entities, new VKMessageAdapter(update.Message));
        //}

    }
}

