using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using FLChat.DAL;

namespace FLChat.WebService.DataTypes
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class MessageSentHistoryRequest : PartialDataRequest
    {          
        public string Ids { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public MessageKind[] Types { get; set; }

        [JsonProperty(propertyName: "start_from")]
        public string StartFromStr { get => StartFrom?.ToString(); set => StartFrom = value != null ? Guid.Parse(value) : (Guid?)null; }

        /// <summary>
        /// Resulting messages start from that message id (not include)
        /// </summary>
        [JsonIgnore]
        public Guid? StartFrom { get; set; }
    }
}
