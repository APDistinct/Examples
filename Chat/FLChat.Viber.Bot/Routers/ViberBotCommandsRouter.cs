using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.Core;
using FLChat.Core.Routers;
using FLChat.DAL.Model;
using FLChat.DAL;

namespace FLChat.Viber.Bot.Routers
{
    public class ViberBotCommandsRouter : BotCommandsRouterByCmd
    {
        public class CommandExtrator : ICommandExtractor
        {
            public string ExtractCommandValue(ChatEntities entities, IOuterMessage msg) {
                return msg.Text;
            }
        }

        public ViberBotCommandsRouter() : base(new CommandExtrator()) {
        }

        public override Guid? RouteMessage(ChatEntities entities, IOuterMessage message, Message dbmsg) {
            CommandsEnum? cmd = IsItCommand(entities, message, dbmsg, out string arg);
            if (cmd.HasValue) {
                if (cmd.Value == CommandsEnum.Url)
                    return Global.SystemBotId;

                if (PerformCommand(entities, message, dbmsg, cmd.Value, arg))
                    return Global.SystemBotId;
            }
            return null;
        }
    }
}
