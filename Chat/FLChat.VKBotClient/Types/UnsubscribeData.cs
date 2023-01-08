using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FLChat.VKBotClient.Types
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class UnsubscribeData
    {
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }
    }
}
