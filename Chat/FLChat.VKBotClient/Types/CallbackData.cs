using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.VKBotClient.Types.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace FLChat.VKBotClient.Types
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class CallbackData
    {
        [JsonProperty(PropertyName = "type", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public CallbackEvent Event { get; set; }

        [JsonProperty(PropertyName = "group_id", Required = Required.Always)]
        public string GroupId { get; set; } 
        
        [JsonProperty(PropertyName = "object")]
        public object Object { get; set; }
    }
}
