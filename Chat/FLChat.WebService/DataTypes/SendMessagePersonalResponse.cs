using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FLChat.WebService.DataTypes
{
    public class SendMessagePersonalResponse : SendMessageResponse {
        [JsonProperty(PropertyName = "status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageStatus Status => User.Status;

        [JsonProperty(PropertyName = "user")]
        public SendMessagePersonalInfo User { get; set; }
    }
}
