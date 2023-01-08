using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.WebService.DataTypes
{
    public enum MessageStatus
    {
        [EnumMember(Value = "transport_not_found")]
        TransportNotFound,

        [EnumMember(Value = "failed")]
        Failed,

        [EnumMember(Value = "deleted")]
        Deleted,

        [EnumMember(Value = "quequed")]
        Quequed,

        [EnumMember(Value = "sent")]
        Sent,

        [EnumMember(Value = "delivered")]
        Delivered,

        [EnumMember(Value = "read")]
        Read,

        [EnumMember(Value = "cancelled")]
        Cancelled
    }

    public static class MessageExtentions
    {
        /// <summary>
        /// Get message status
        /// </summary>
        /// <param name="msg">Message addressee</param>
        /// <returns>Message status</returns>
        public static MessageStatus GetMessageStatus(this MessageToUser msg) {
            if (msg.Message.IsDeleted)
                return MessageStatus.Deleted;
            if (msg.IsFailed)
                return MessageStatus.Failed;
            if (msg.IsRead)
                return MessageStatus.Read;
            if (msg.IsDelivered)
                return MessageStatus.Delivered;
            if (msg.IsSent)
                return MessageStatus.Sent;
            if (msg.Message.DalayedCancelled != null)
                return MessageStatus.Cancelled;

            return MessageStatus.Quequed;
        }
    }
}
