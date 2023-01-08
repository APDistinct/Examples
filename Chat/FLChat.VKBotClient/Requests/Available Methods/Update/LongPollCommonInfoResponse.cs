using FLChat.VKBotClient.Callback;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.VKBotClient.Requests.Available_Methods.Update
{
    public class LongPollCommonInfoResponse
    {
        [JsonProperty(PropertyName = "failed")]
        public int? Error { get; set; }

        [JsonProperty(PropertyName = "ts")]
        public string Ts { get; set; }

        [JsonProperty(PropertyName = "updates")]
        public  IEnumerable<CallbackCommonResponse> Updates { get; set; }
    }
}
