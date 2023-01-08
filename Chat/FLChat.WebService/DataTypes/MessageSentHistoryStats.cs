using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using FLChat.DAL.Model;
using Newtonsoft.Json.Converters;

namespace FLChat.WebService.DataTypes
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class MessageSentHistoryStats
    {
        private MessageStatsGroupedView _item;
        private readonly bool _cancelled;

        /// <summary>
        /// State of sending message process
        /// </summary>
        public enum SendingState
        {
            Quequed,
            InProgress,
            Complete,
            Cancelled
        }

        public MessageSentHistoryStats(MessageStatsGroupedView item, bool cancelled = false)
        {
            _item = item ?? throw new ArgumentNullException(nameof(item));
            _cancelled = cancelled;
        }

        /// <summary>
        /// Total counts of recipients
        /// </summary>
        public int RecipientCount => _item.RecipientCount ?? 0;

        /// <summary>
        /// Total count of web chat messages
        /// </summary>
        public int WebChatCount => _item.WebChatCount ?? 0;

        /// <summary>
        /// Total count of failed messages
        /// </summary>
        public int FailedCount => _item.FailedCount ?? 0;

        /// <summary>
        /// Total count of sent messages
        /// </summary>
        public int SentCount => _item.SentCount ?? 0;

        /// <summary>
        /// total count of quequed messages
        /// </summary>
        public int QuequedCount => _item.QuequedCount ?? 0;

        /// <summary>
        /// count of user for whom we cant send message to web-chat
        /// </summary>
        [JsonProperty(PropertyName = "cant_send_count")]
        public int CantSendToWebChatCount => _item.CantSendToWebChatCount ?? 0;

        /// <summary>
        /// TOtal count of web chat accepted messages
        /// </summary>
        public int WebChatAcceptedCount => _item.WebChatAcceptedCount ?? 0;

        /// <summary>
        /// total count of opened web chat messages by url
        /// </summary>
        public int SmsUrlOpenedCount => _item.SmsUrlOpenedCount ?? 0;

        /// <summary>
        /// Sending message process state
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter), typeof(SnakeCaseNamingStrategy))]
        public SendingState State => QuequedCount == 0 ? SendingState.Complete 
            : (_cancelled ? SendingState.Cancelled 
            : (SentCount == 0 && FailedCount == 0 ? SendingState.Quequed : SendingState.InProgress));
    }
}
