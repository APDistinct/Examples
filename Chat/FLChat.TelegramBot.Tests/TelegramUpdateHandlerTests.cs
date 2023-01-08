using System;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telegram.Bot.Types;

namespace FLChat.TelegramBot.Tests
{
    [TestClass]
    public class TelegramUpdateHandlerTests
    {
        //[TestMethod]
        //public void TestMethod1()
        //{
        //    TelegramUpdateHandler handler = new TelegramUpdateHandler();
        //    ChatEntities entities = new ChatEntities();
        //    //TelegramMessageAdapter msg = ResourceHelper.Read("message_deep_link");
        //    Update update = new Update()
        //    {
        //        Message = new Telegram.Bot.Types.Message()
        //        {
        //            MessageId = -1,
        //            From = new Telegram.Bot.Types.User()
        //            {
        //                Id = 1,
        //                IsBot = false,
        //                FirstName = "",
        //                LastName = "",
        //            },
        //            Chat = new Chat()
        //            {
        //                Id = 1,
        //                FirstName = "",
        //                LastName = "",
        //                Type = Telegram.Bot.Types.Enums.ChatType.Private,
        //            },
        //            Date = DateTime.UtcNow,
        //            Text = "/start com12345",
        //            Entities = new MessageEntity[] 
        //            { new MessageEntity() { Offset = 0, Length = 6, Type = Telegram.Bot.Types.Enums.MessageEntityType.BotCommand} },
        //        },
        //    };
        //    handler.MakeUpdate(entities, update);
        //}

//        {
//    "update_id": 38335582,
//    "message": {
//        "message_id": 418,
//        "from": {
//            "id": 836798453,
//            "is_bot": false,
//            "first_name": "Savelyeva",
//            "last_name": "Natalya"
//        },
//        "chat": {
//            "id": 836798453,
//            "first_name": "Savelyeva",
//            "last_name": "Natalya",
//            "type": "private"
//        },
//        "date": 1563283987,
//        "text": "/start gEoWf4fQw2dNJYSjB58U",
//        "entities": [
//            {
//                "offset": 0,
//                "length": 6,
//                "type": "bot_command"
//            }
//        ]
//    }
//}
    }
}
