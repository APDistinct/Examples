using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.Core.Routers
{
    public class MsgAddresseeRouter : IMessageRouter
    {
        public Guid? RouteMessage(ChatEntities entities, IOuterMessage message, Message dbmsg) {
            if (dbmsg.FromTransport.MsgAddressee != null)
                return dbmsg.FromTransport.MsgAddressee.Id;
            else
                return null;
        }
    }
}
