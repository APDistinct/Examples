using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FLChat.WebService.DataTypes
{
    public class SendMessageBroadcastResponse : SendMessageResponse
    {
        /// <summary>
        /// Message addressee
        /// </summary>
        [JsonProperty(PropertyName = "users")]
        public IEnumerable<SendMessagePersonalInfo> Users { get; set; }
    }
}
