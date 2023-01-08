using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using FLChat.DAL.Model;

namespace FLChat.Core
{
    /// <summary>
    /// Information abount sent message
    /// </summary>
    public class SentMessageInfo
    {
        public SentMessageInfo(string messageId, DateTime sentTime) {
            MessageId = messageId;
            SentTime = sentTime;
        }

        public SentMessageInfo(string[] messageIds, DateTime sentTime) {
            MessageId = messageIds[0];
            SentTime = sentTime;
            MessageIds = messageIds;
        }

        public string MessageId { get; }
        public DateTime SentTime { get; }

        /// <summary>
        /// List of messages id. Can be null.
        /// </summary>
        public string []MessageIds { get; }
    }

    /// <summary>
    /// Send message interface
    /// </summary>
    public interface IMessageSender
    {
        Task<SentMessageInfo> Send(MessageToUser msg, string msgText, CancellationToken ct);
    }

    public interface IMessageBulkSender
    {
        Task<IEnumerable< SentMessageInfo>> Send(Message msg, string msgText, CancellationToken ct);
    }
}
