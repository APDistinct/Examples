using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.DataTypes
{
    public class PasswordRequest
    {
        [JsonProperty(PropertyName = "password", Required = Required.Always)]
        public string Password { get; set; }
        [JsonProperty(PropertyName = "old_password")]
        public string OldPassword { get; set; }
    }
}
