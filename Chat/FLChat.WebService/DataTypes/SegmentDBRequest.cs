using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.DataTypes
{
    public class SegmentDBRequest : PartialDataRequest
    {
        [JsonProperty(PropertyName = "search")]
        public string SearchValue { get; set; }

        [JsonProperty(PropertyName = "segment")]
        public string Segment { get; set; }
    }
}
