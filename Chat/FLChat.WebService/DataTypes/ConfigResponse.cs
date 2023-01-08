using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.DataTypes
{
    public class ConfigResponse
    {
        [JsonProperty(PropertyName = "config")]
        public string Config { get; set; }
    }
}
