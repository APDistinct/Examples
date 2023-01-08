using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FLChat.VkWidget
{
    
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class VkWidgetCallbackData
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? Id { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "user_id")]
        public int UserId { get; set; }

        [JsonProperty(Required = Required.Always, PropertyName = "deep_link")]
        public string DeepLink { get; set; }
    }
}
