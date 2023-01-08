using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FLChat.VKBotClient.Callback
{
    public class CallbackResponse
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "group_id")]
        public int GroupId { get; set; }
    }
    public class CallbackResponse<T> : CallbackResponse where T : class
    {
        [JsonProperty(PropertyName = "object")]
        public T Object { get; set; }
    }
}
