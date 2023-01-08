using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using FLChat.Viber.Client.Types;

namespace FLChat.Viber.Client.Requests
{
    /// <summary>
    /// Send picture message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class SendPictureMessageRequest : SendMessageRequest
    {
        public const int MaxTextLength = 120;
        public const int MaxImageSize = 1024 * 1024;

        public SendPictureMessageRequest(Sender sender, string receiver, string text, string media) 
            : base(sender, receiver, MessageType.Picture) {
            Media = media;
            Text = text ?? "";
        }

        [JsonProperty(Required = Required.Always)]
        public string Text { get; set; }

        /// <summary>
        /// URL of the image (JPEG)
        /// required. Max size 1 MB. Only JPEG format is supported. 
        /// Other image formats as well as animated GIFs can be sent as URL messages or file messages
        /// </summary>
        [JsonProperty]
        public string Media { get; set; }

        /// <summary>
        /// URL of a reduced size image (JPEG)
        /// optional. Max size 100 kb. Recommended: 400x400. Only JPEG format is supported
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Thumbnail { get; set; }
    }
}
