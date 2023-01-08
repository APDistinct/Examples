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
    public class SendMessagePersonalInfo
    {        
        /// <summary>
        /// Message Addressee 
        /// </summary>
        [JsonProperty(PropertyName = "user_id")]
        public Guid? ToUser { get; set; }

        /// <summary>
        /// Addressee transport
        /// </summary>
        [JsonProperty(PropertyName = "to_transport")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TransportKind? ToTransport { get; set; }

        /// <summary>
        /// Message status
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageStatus Status { get; set; }
    }    
}
