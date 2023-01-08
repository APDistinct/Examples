using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Viber.Client.Types
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class Message
    {
        /// <summary>
        /// Message type
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public MessageType Type { get; set; }

        /// <summary>
        /// The message text  
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// URL of the message media - can be image, video, file and url.Image/Video/File URLs will have a TTL of 1 hour
        /// </summary>
        public string Media { get; set; }

        /// <summary>
        /// Locati on coordinates
        /// </summary>
        public Location Location { get; set; }

        public Contact Contact { get; set; }

        /// <summary>
        /// Tracking data sent with the last message to the user
        /// </summary>
        public string TrackingData { get; set; }

        /// <summary>
        /// File name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// File size in bytes. Relevant for file type messages
        /// </summary>
        public int? FileSize { get; set; }

        /// <summary>
        /// Video length in seconds. Relevant for video type messages
        /// </summary>
        public int? Duration { get; set; }

        /// <summary>
        /// Relevant for sticker type messages
        /// </summary>
        public string StickerId { get; set; }
    }
}
