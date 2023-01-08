using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.Core;
using FLChat.DAL.Model;
using FLChat.TelegramBot.Adapters;

namespace FLChat.TelegramBot.Algorithms
{
    //public class SelectAddresseeRouter : IMessageRouter
    //{
    //    public const string SELECT_ADDRESSEE_MENU = "select_addressee_menu";

    //    public Guid? RouteMessage(ChatEntities entities, IOuterMessage message, Message dbmsg) {
    //        if (message.Text == TelegramMessageAdapter.SelectAddressee) {
    //            SendSelectAddressee(entities, dbmsg);
    //            return DAL.Global.SystemBotId;
    //        }
    //        else
    //            return null;
    //    }

    //    private void SendSelectAddressee(ChatEntities entities, Message dbmsg) {
    //        Message reply = new Message() {
    //            Kind = DAL.MessageKind.Personal,
    //            FromTransport = entities.SystemBotTransport,
    //            AnswerTo = dbmsg,
    //            Text = "Выберите адресата для сообщений",
    //            ToTransport = dbmsg.FromTransport,
    //            Specific = SELECT_ADDRESSEE_MENU
    //        };
    //        entities.Message.Add(reply);
    //    }
    //}
}
