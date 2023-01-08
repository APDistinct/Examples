using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FLChat.VKBotClient.Callback
{
    public class MessageInfo
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
    }
}
