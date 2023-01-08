using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.Core.Algorithms;
using FLChat.DAL.Model;

namespace FLChat.Core.Routers
{
    /// <summary>
    /// Route to sender owner's user
    /// </summary>
    public class OwnerRouter : IMessageRouter //where TMessage : IOuterMessage
    {
        public Guid? RouteMessage(ChatEntities entities, IOuterMessage message, Message dbmsg) {
            User usr = dbmsg.FromTransport.User;
            if (usr.OwnerUser != null && usr.OwnerUser.Transports.Get(DAL.TransportKind.FLChat) != null)
                return usr.OwnerUserId.Value;
            else
                return null;
        }
    }
}
