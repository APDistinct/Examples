using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.DataTypes
{   
    public class SegmentListResponse
    {
        [JsonProperty(PropertyName = "segments")]
        public IEnumerable<SegmentInfo> Segments { get; set; }
    }
}
