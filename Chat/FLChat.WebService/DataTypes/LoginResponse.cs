using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FLChat.WebService.DataTypes
{
    public class LoginResponse
    {
        public enum StatusEnum
        {
            Sent,
            Waiting
        };

        [JsonProperty(PropertyName = "status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public StatusEnum Status { get; set; }

        [JsonProperty(PropertyName = "waiting_time")]
        public int? WaitingTime { get; set; }
    }
}
