using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace FLChat.WebService.DataTypes
{
    public class EventsRequest
    {
        [JsonProperty(PropertyName = "last_event_id")]
        public long? LastEventId { get; set; }

        [JsonProperty(PropertyName = "count")]
        public int? Count { get; set; }
    }
}
