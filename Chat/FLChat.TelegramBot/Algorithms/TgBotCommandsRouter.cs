using FLChat.Core.Routers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.Core;
using FLChat.DAL.Model;

namespace FLChat.TelegramBot.Algorithms
{
    public class TgBotCommandsRouter : BotCommandsRouterByCmd
    {
        //static Dictionary<string, string> _commands;

        //static TgBotCommandsRouter() {
        //    using (ChatEntities entities = new ChatEntities()) {
        //        _commands = entities.ExternalTransportButton.ToDictionary(i => i.Caption, i => i.Command);
        //    }            
        //}

        public class CommandExtrator : ICommandExtractor
        {
            public string ExtractCommandValue(ChatEntities entities, IOuterMessage msg) {
                return entities.ExternalTransportButton.Where(b => b.Caption == msg.Text).Select(b => b.Command).FirstOrDefault();
            }
        }

        public TgBotCommandsRouter() : base(new CommandExtrator()) {
        }

        //public const string SELECT_ADDRESSEE_MENU = "select_addressee_menu";        

        //protected override ol IsItCommand(ChatEntities entities, IOuterMessage message, Message dbmsg, out string arg) {
        //    cmd = entities.ExternalTransportButton.Where(b => b.Caption == message.Text).Select(b => b.Command).FirstOrDefault();
        //    if (cmd != null)
        //    return (cmd != null);
        //    //return _commands.TryGetValue(message.Text, out cmd);
        //}

        //protected override void PerformSelectAddressee(ChatEntities entities, IOuterMessage message, Message dbmsg) {
        //    Message reply = new Message() {
        //        Kind = DAL.MessageKind.Personal,
        //        FromTransport = entities.SystemBotTransport,
        //        AnswerTo = dbmsg,
        //        Text = "Выберите адресата для сообщений",
        //        ToTransport = dbmsg.FromTransport,
        //        Specific = SELECT_ADDRESSEE_MENU
        //    };
        //    entities.Message.Add(reply);
        //}
    }
}
