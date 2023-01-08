using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FLChat.WebService.DataTypes
{
    public class MessageHistoryRequest
    {
        /// <summary>
        /// Messages from chat with that user
        /// </summary>
        [JsonProperty(PropertyName = "user_id")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Message history start's id 
        /// If null, then history starts from last message
        /// </summary>
        [JsonProperty(PropertyName = "message_id")]
        public Guid? MessageId { get; set; }

        /// <summary>
        /// Count of messages. Can be null
        /// </summary>
        [JsonProperty(PropertyName = "count")]
        public int? Count { get; set; }

        /// <summary>
        /// Message ordering in response
        /// </summary>
        [JsonProperty(PropertyName = "order")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderEnum? Order { get; set; }
    }
}
