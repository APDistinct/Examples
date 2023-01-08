using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.VKBotClient.Types
{
    public class VkError
    {
        [JsonProperty(PropertyName = "error")]
        public VkErrorInfo Error { get; set; }
    }

    public class VkErrorInfo
    {
        [JsonProperty(PropertyName = "error_code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "error_msg")]
        public string Msg { get; set; }

        //[JsonProperty(PropertyName = "request_params")]
        //public Dictionary<string, string> RequestParams { get; set; }        
    }
}
