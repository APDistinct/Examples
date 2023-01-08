using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;
using FLChat.DAL;

namespace FLChat.Core.Routers
{
    /// <summary>
    /// Route message to nearest parent with FLChat transport
    /// </summary>
    public class NearestParentRouter : IMessageRouter
    {
        public Guid? RouteMessage(ChatEntities entities, IOuterMessage message, Message dbmsg) {
            const int FLChat = (int)TransportKind.FLChat;
            Guid user = (
                from p in entities.User_GetParents(dbmsg.FromUserId, null)
                join t in entities.Transport on p.UserId equals t.UserId
                where t.TransportTypeId == FLChat && t.Enabled
                orderby p.Deep descending
                select p.UserId).FirstOrDefault();
            return user == Guid.Empty ? null : (Guid?)user;            
        }
    }
}
