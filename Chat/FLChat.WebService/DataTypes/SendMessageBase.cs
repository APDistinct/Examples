using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FLChat.WebService.DataTypes
{
    public abstract class SendMessageBase
    {
        /// <summary>
        /// Message type
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageKind Type { get; set; }

        /// <summary>
        /// Message addressee. Multiple users selection
        /// </summary>
        [JsonProperty(PropertyName = "selection")]
        public UserSelection Selection { get; set; }
    }
}
