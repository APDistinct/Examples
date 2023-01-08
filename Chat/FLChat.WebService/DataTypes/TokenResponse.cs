using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FLChat.WebService.DataTypes
{
    public class TokenResponse
    {
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
    }
}
