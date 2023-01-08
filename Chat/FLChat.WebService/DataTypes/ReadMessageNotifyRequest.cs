using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FLChat.WebService.DataTypes
{
    public class ReadMessageNotifyRequest
    {
        [JsonProperty(PropertyName = "messages", Required = Required.Always)]
        public Guid[] Messages { get; set; }
    }
}
