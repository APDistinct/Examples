using FLChat.VKBotClient.Requests.Available_Methods.Sending_Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.VKBotClient.Types
{
    public class ServerInfo
    {
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "server")]
        public string server { get; set; }

        [JsonProperty(PropertyName = "ts")]
        public string Ts { get; set; }
    }

    public class LongPollServerResponse : VkError
    {
        [JsonProperty(PropertyName = "response")]
        public ServerInfo ServerInfo { get; set; }
    }
}
