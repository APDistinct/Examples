using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.DataTypes
{
    public class SegmentManageRequest
    {
        [JsonProperty(PropertyName = "add")]
        public IEnumerable<Guid> Add { get; set; }
        [JsonProperty(PropertyName = "remove")]
        public IEnumerable<Guid> Remove { get; set; }
    }
}
