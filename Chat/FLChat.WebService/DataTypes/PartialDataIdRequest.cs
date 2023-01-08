using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.DataTypes
{
    public class PartialDataIdRequest : PartialDataRequest
    {      
        [JsonProperty(PropertyName = "id")]
        public string Ids { get; set; }

        [JsonIgnore]
        public Guid? Id => Ids != null ? Guid.Parse(Ids) : (Guid?)null;
    }
}
