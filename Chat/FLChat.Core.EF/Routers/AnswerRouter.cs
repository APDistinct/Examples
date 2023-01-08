using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.Core.Routers
{
    public class AnswerRouter : IMessageRouter
    {
        public Guid? RouteMessage(ChatEntities entities, IOuterMessage message, Message dbmsg) {
            if (dbmsg.AnswerToId.HasValue) {
                return dbmsg.AnswerTo.FromUserId;
            } else
                return null;
        }
    }
}
