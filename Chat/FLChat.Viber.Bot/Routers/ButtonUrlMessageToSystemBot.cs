using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.Core;
using FLChat.DAL.Model;
using FLChat.DAL;
using FLChat.Core.Routers;
using FLChat.Core.Buttons;

namespace FLChat.Viber.Bot.Routers
{
    public class ButtonUrlMessageToSystemBot : IMessageRouter
    {
        //private readonly ITransportButtonsSource _buttons;

        public ButtonUrlMessageToSystemBot() {//ITransportButtonsSource buttons = null) {
            //_buttons = buttons ?? new TransportButtonsSourceBuffered();
        }

        public Guid? RouteMessage(ChatEntities entities, IOuterMessage message, Message dbmsg) {
            if (message.Text != null && message.Text.StartsWith("http")) {
                bool exists = entities.ExternalTransportButton.Where(b => b.Command == BotCommandsRouter.URL_PREFIX + message.Text).Any();
                if (exists)
                //foreach (var row in _buttons.GetButtons(null)) {
                //    foreach (var col in row) {
                //        if (BotCommandsRouter.GetCommandType(col.Command, out string arg) == BotCommandsRouter.CommandsEnum.Url
                //            && arg == message.Text)
                            return Global.SystemBotId;
                //    }
                //}
            }
            return null;
        }
    }
}
