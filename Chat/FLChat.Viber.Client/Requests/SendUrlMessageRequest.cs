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
    /// Send url message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class SendUrlMessageRequest : SendMessageRequest
    {
        public const int MaxUrlLength = 2000;

        public SendUrlMessageRequest(Sender sender, string receiver, string media)
            : base(sender, receiver, MessageType.Url) {
            Media = media;
        }

        /// <summary>
        /// URL	required. 
        /// Max 2,000 characters
        /// </summary>
        [JsonProperty]
        public string Media { get; set; }
    }
}
