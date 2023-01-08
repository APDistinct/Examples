using System;
using System.Collections.Generic;
using FLChat.VKBotClient.Types.Attachments;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace FLChat.VKBotClient.Types
{
    /// <summary>
    /// This object represents a message.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class Message
    {

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "date")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Date { get; set; }

        [JsonProperty(PropertyName = "peer_id")]
        public int PeerId { get; set; }

        [JsonProperty(PropertyName = "from_id")]
        public int FromId { get; set; }

        [JsonProperty(PropertyName = "random_id")]
        public int RandomId { get; set; }

        [JsonProperty(PropertyName = "ref")]
        public string Ref { get; set; }

        [JsonProperty(PropertyName = "ref_source")]
        public string RefSource { get; set; }

        [JsonProperty(PropertyName = "important")]
        public bool Important { get; set; }

        [JsonProperty(PropertyName = "payload")]
        public string Payload { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "attachments")]
        public IEnumerable<Attachment> Attachments { get; set; }


        /*
                /// <summary>
                /// Unique message identifier
                /// </summary>
                [JsonProperty(Required = Required.Always)]
                public int MessageId { get; set; }

                /// <summary>
                /// Sender
                /// </summary>
                [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
                public User From { get; set; }

                /// <summary>
                /// Date the message was sent
                /// </summary>
                [JsonProperty(Required = Required.Always)]
                [JsonConverter(typeof(UnixDateTimeConverter))]
                public DateTime Date { get; set; }

                /// <summary>
                /// Conversation the message belongs to
                /// </summary>
                [JsonProperty(Required = Required.Always)]
                public Chat Chat { get; set; }

                /// <summary>
                /// Indicates whether this message is a forwarded message
                /// </summary>
                [Obsolete("Check ForwardFrom and ForwardFromChat properties instead")]
                public bool IsForwarded => ForwardFrom != null;

                /// <summary>
                /// Optional. For forwarded messages, sender of the original message
                /// </summary>
                [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
                public User ForwardFrom { get; set; }

                /// <summary>
                /// Optional. For messages forwarded from a channel, information about the original channel
                /// </summary>
                [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
                public Chat ForwardFromChat { get; set; }

                /// <summary>
                /// Optional. For forwarded channel posts, identifier of the original message in the channel
                /// </summary>
                [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
                public int ForwardFromMessageId { get; set; }

                /// <summary>
                /// Optional. For text messages, the actual UTF-8 text of the message
                /// </summary>
                [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
                public string Text { get; set; }
        */
    }
}
