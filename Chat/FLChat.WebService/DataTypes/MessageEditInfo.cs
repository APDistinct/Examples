using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FLChat.WebService.DataTypes
{
    public class MessageEditInfo
    {
        [JsonProperty(PropertyName = "id")]
        public string Ids { get; set; }

        //[JsonIgnore]
        //public Guid? Id => Ids != null ? Guid.Parse(Ids) : (Guid?)null;

        /// <summary>
        /// Message DelayedStart date/time
        /// </summary>
        [JsonProperty(PropertyName = "delayed_start")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime? DelayedStart { get; set; }

        /// <summary>
        /// Message DelayedStart status
        /// </summary>
        [JsonProperty(PropertyName = "cancelled")]
        public bool? Cancelled { get; set; }
    }
}
