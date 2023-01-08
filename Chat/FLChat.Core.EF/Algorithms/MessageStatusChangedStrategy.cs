using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;

namespace FLChat.Core.Algorithms
{
    /// <summary>
    /// Manage message status
    /// </summary>
    public class MessageStatusChangedStrategy : IMessageStatusChangedStrategy<ChatEntities>
    {
        public void Process(ChatEntities entities, IOuterMessageStatus message) {
            //search transport for incoming message
            Transport fromTransport = entities
                .Transport
                .GetTransportByOuterId(message.TransportKind, message.UserId, null);
            if (fromTransport == null)
                return;

            //search message
            MessageToUser mtu = entities
                .MessageTransportId
                .Where(mt => mt.TransportId == message.MessageId
                    && mt.TransportTypeId == (int)message.TransportKind
                    && mt.ToUserId == fromTransport.UserId)
                .Select(mt => mt.MessageToUser)
                .SingleOrDefault();
            if (mtu == null)
                return;

            if (message.IsFailed) {
                mtu.IsFailed = true;
                entities.MessageError.Add(new MessageError() {
                    MsgId = mtu.MsgId,
                    ToUserId = mtu.ToUserId,
                    ToTransportTypeId = mtu.ToTransportTypeId,
                    Type = nameof(MessageStatusChangedStrategy),
                    Descr = message.FailureReason
                });
            }
            if (message.IsDelivered)
                mtu.IsDelivered = true;
            if (message.IsRead)
                mtu.IsRead = true;
            entities.SaveChanges();
        }
    }
}
