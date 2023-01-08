using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.Core.Routers
{
    public class BotCommandsRouterByCmd : BotCommandsRouter
    {
        /// <summary>
        /// Get button's command value from message
        /// </summary>
        public interface ICommandExtractor
        {
            string ExtractCommandValue(ChatEntities entities, IOuterMessage msg);
        }

        private readonly ICommandExtractor _extractor;

        public const string SELECT_ADDRESSEE_MENU = "select_addressee_menu";

        public BotCommandsRouterByCmd(ICommandExtractor extractor) {
            _extractor = extractor;
        }

        protected override CommandsEnum? IsItCommand(ChatEntities entities, IOuterMessage message, Message dbmsg, out string arg) {
            string cmd = _extractor.ExtractCommandValue(entities, message);
            if (string.IsNullOrEmpty(cmd)) {
                arg = null;
                return null;
            }
            return GetCommandType(cmd, out arg);
        }

        protected override void PerformSelectAddressee(ChatEntities entities, IOuterMessage message, Message dbmsg) {
            Message reply = new Message() {
                Kind = DAL.MessageKind.Personal,
                FromTransport = entities.SystemBotTransport,
                AnswerTo = dbmsg,
                Text = "Выберите адресата для сообщений",
                ToTransport = dbmsg.FromTransport,
                Specific = SELECT_ADDRESSEE_MENU
            };
            entities.Message.Add(reply);
        }
    }
}
