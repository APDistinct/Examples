using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.VKBotClient.Callback
{
    public class CallbackCommonResponse : CallbackResponse
    {
        [JsonProperty(PropertyName = "object")]
        public JRaw Object { get; set; }
    }
}
