using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FLChat.WebService.DataTypes
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class MessageSentHistoryResponse : PartialDataResponse
    {
        public MessageSentHistoryResponse(IPartialData data) : base(data) {
        }

        public IEnumerable<MessageSentHistoryItem> Messages { get; set; }

        /// <summary>
        /// Last message id in messages
        /// </summary>
        public Guid? LastMessageId { get; set; }

        /// <summary>
        /// Resulting messages start from that message id (not include)
        /// </summary>
        public Guid? StartedFrom { get; set; }
    }
}
