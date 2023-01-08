using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace SalesForce.Types
{
    [JsonObject]
    public class ErrorResponse
    {
        [JsonProperty(PropertyName = "error", Required = Required.Always)]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "error_description")]
        public string Description { get; set; }
    }
}
