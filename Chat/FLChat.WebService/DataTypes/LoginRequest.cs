using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace FLChat.WebService.DataTypes
{
    public class LoginRequest
    {
        [JsonProperty(PropertyName = "phone", Required = Required.Always)]
        public string Phone { get; set; }
    }
}
