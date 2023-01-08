using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace SalesForce.Types
{
    /// <summary>
    /// Response data type of auth by password request
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class AuthResponse
    {
        [JsonProperty(Required = Required.Always)]
        public string AccessToken { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string InstanceUrl { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string TokenType { get; set; }

        [JsonProperty(Required = Required.Always)]
        //[JsonConverter(typeof(UnixDateTimeConverter))]
        public string IssuedAt { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Signature { get; set; }
    }
}
