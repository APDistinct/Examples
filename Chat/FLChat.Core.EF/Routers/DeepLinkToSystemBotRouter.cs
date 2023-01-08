using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;

namespace FLChat.Core.Routers
{
    public class DeepLinkToSystemBotRouter : IMessageRouter
    {
        public Guid? RouteMessage(ChatEntities entities, IOuterMessage message, Message dbmsg) {
            if (message.DeepLink != null)
                return Global.SystemBotId;
            else
                return null;
        }
    }
}
