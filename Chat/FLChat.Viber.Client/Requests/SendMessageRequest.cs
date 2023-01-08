using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using FLChat.Viber.Client.Types;
using Newtonsoft.Json.Converters;

namespace FLChat.Viber.Client.Requests
{
    /// <summary>
    /// Send messages base class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public abstract class SendMessageRequest : RequestBase<SendMessageResponse>
    {
        public SendMessageRequest(Sender sender, string receiver, MessageType type) : base("send_message") {
            Type = type;
            Sender = sender;
            Receiver = receiver;
        }

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Receiver { get; set; }

        [JsonProperty(Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageType Type { get; }

        [JsonProperty(Required = Required.Always)]
        public Sender Sender { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string TrackingData { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? MinApiVersion { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Keyboard Keyboard { get; set; }
    }
}
