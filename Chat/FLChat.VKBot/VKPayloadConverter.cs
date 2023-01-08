using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.VKBot
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class VKPayloadConverter
    {
        public VKPayloadConverter(string button)
        {
            Button = button;
        }

        [JsonProperty]
        public string Button { get; set; }

        public string GetJson() => JsonConvert.SerializeObject(this);

    }
}
