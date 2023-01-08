using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FLChat.WebService.DataTypes
{
    /// <summary>
    /// Response for message history request
    /// </summary>
    public class MessageHistoryResponse
    {
        /// <summary>
        /// Message history for chat with this user
        /// </summary>
        [JsonProperty(PropertyName = "user_id")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Requested direction forward or backward
        /// </summary>
        [JsonProperty(PropertyName = "forward")]
        public bool Forward { get; set; }

        /// <summary>
        /// Maximum possible message count per request
        /// </summary>
        [JsonProperty(PropertyName = "max_count")]
        public int MaxCount { get; set; }

        /// <summary>
        /// Last message id
        /// </summary>
        [JsonProperty(PropertyName = "last_id")]
        public Guid? LastId { get; set; }

        /// <summary>
        /// Message ordering in response
        /// </summary>
        [JsonProperty(PropertyName = "order")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderEnum Order { get; set; }

        /// <summary>
        /// Messages, ordered due to field forward
        /// </summary>
        [JsonProperty(PropertyName = "messages")]
        public IEnumerable<MessageInfo> Messages { get; set; }
    }
}
