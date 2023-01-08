using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace FLChat.WebService.DataTypes
{
    public class TokenRequest
    {
        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "sms_code")]
        public string SmsCode { get; set; }

        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
    }
}
