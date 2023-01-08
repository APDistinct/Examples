using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using FLChat.Viber.Client.Json;
using Newtonsoft.Json.Converters;

namespace FLChat.Viber.Client.Types
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class CallbackData
    {
        /// <summary>
        /// Call back type - which event triggered the callback
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public CallbackEvent Event { get; set; }

        /// <summary>
        /// Time of the event that triggered the callback
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Unique ID of the message
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public long MessageToken { get; set; }

        /// <summary>
        /// Information about sender. Relevant for message type callback
        /// </summary>
        [JsonProperty]
        public User Sender { get; set; }

        /// <summary>
        /// Message. Relevant for message type callback
        /// </summary>
        [JsonProperty]
        public Message Message { get; set; }

        /// <summary>
        /// Information about user. Relevant for subscribe type callback
        /// </summary>
        [JsonProperty]
        public User User { get; set; }

        /// <summary>
        /// User's identificatior. Relevant for unsubscribe, delivered, seen and failed type callback
        /// </summary>
        [JsonProperty]
        public string UserId { get; set; }

        /// <summary>
        /// The specific type of conversation_started event. Relevant to conversation started type callback
        /// </summary>
        [JsonProperty]
        public ConversationStartedType? Type { get; set; }

        /// <summary>
        /// Any additional parameters added to the deep link used to access the conversation passed as a string.
        /// Relevant to conversation started type callback
        /// </summary>
        [JsonProperty]
        public string Context { get; set; }

        /// <summary>
        /// indicated whether a user is already subscribed
        /// </summary>
        [JsonProperty]
        public bool? Subscribed { get; set; }

        [JsonProperty(PropertyName = "desc")]
        public string Description { get; set; }

        /// <summary>
        /// Relevant to webhook event
        /// </summary>
        [JsonProperty]
        public string ChatHostname { get; set; }
    }
}
