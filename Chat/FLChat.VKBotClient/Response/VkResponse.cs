using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.VKBotClient.Response
{
    public class VkResponse<T> 
    {
        [JsonProperty(PropertyName = "error")]
        public ErrorResponse Error { get; set; }
        [JsonProperty(PropertyName = "response")]
        public T Response { get; set; }
    }
}
