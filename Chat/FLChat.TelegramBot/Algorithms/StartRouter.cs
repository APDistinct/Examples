using FLChat.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.TelegramBot.Adapters;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FLChat.TelegramBot.Algorithms
{
    public class StartRouter : IMessageRouter
    {
        public Guid? RouteMessage(ChatEntities entities, IOuterMessage message, DAL.Model.Message dbmsg) {
            if (message is TelegramMessageAdapter tgmsg && tgmsg.Message != null && tgmsg.Message.Entities != null) {
                MessageEntity entity = tgmsg.Message.Entities
                    .Where(e => e.Type == MessageEntityType.BotCommand && tgmsg.Message.Text?.Substring(e.Offset, e.Length) == "/start")
                    .FirstOrDefault();
                if (entity != null && message.DeepLink == null && dbmsg.FromTransport.User.IsTemporary == false)
                {
                    if (!entities.Transport.Where(t => t.TransportOuterId == message.FromId && t.TransportTypeId == (int)message.TransportKind).Any())
                    {
                        DAL.Model.Message updmsg = new DAL.Model.Message()
                        {
                            Kind = DAL.MessageKind.Personal,
                            FromTransport = entities.SystemBotTransport,
                            Text = Settings.Values.GetValue("TEXT_TG_START_MSG", "Обновление кнопок"),
                            Specific = TelegramClient.SEND_BUTTONS_MENU,
                            ToTransport = dbmsg.FromTransport
                        };
                        entities.Message.Add(updmsg);
                        return Global.SystemBotId;
                    }
                }
            }
            return null;
        }
    }
}
