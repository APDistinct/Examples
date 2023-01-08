using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace FLChat.WebService.DataTypes
{
    public class EventsResponse
    {
        [JsonProperty(PropertyName = "events")]
        public IEnumerable<EventInfo> Events { get; set; }

        [JsonProperty(PropertyName = "last_id")]
        public long LastId { get; set; }

        [JsonProperty(PropertyName = "max_count")]
        public int MaxCount { get; set; }
    }
}
