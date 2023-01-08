using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SalesForce.Types
{
    /// <summary>
    /// Type for response execute SOQL Query request
    /// </summary>
    /// <typeparam name="TRecord"></typeparam>
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class QueryResponse<TRecord> where TRecord : class
    {
        [JsonProperty(Required = Required.Always)]
        public bool Done { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int TotalSize { get; set; }

        [JsonProperty(Required = Required.Default)]
        public string NextRecordsUrl { get; set; }

        [JsonProperty(Required = Required.Always)]
        public TRecord[] Records { get; set; }
    }
}
