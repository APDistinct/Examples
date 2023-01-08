using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.Core.Routers
{
    public class ReplyRouter : IMessageRouter
    {
        public Guid? RouteMessage(ChatEntities entities, IOuterMessage message, Message dbmsg) {
            if (message.ReplyToMessageId == null)
                return null;

            Message replyTo = entities.MessageTransportId
                .Where(mid => mid.TransportTypeId == (int)message.TransportKind && mid.TransportId == message.ReplyToMessageId)
                .Select(mid => mid.Message)
                .SingleOrDefault();
            if (replyTo == null)
                return null; //may be throw exception?

            dbmsg.AnswerTo = replyTo;
            dbmsg.AnswerToId = replyTo.Id;
            return replyTo.FromUserId;
        }
    }
}
