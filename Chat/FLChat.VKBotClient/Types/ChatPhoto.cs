using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FLChat.VKBotClient.Types
{
    /// <summary>
    /// Collection of fileIds of profile pictures of a chat.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class ChatPhoto
    {
        /// <summary>
        /// File id of the big version of this <see cref="ChatPhoto"/>
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string BigFileId { get; set; }

        /// <summary>
        /// File id of the small version of this <see cref="ChatPhoto"/>
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string SmallFileId { get; set; }
    }
}
