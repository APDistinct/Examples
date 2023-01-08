using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.VKBotClient.Response
{
    public class SendMessageResponse
    {
        //public int Random { get; set; }
        [JsonProperty(PropertyName = "response")]
        public int MessageId { get; set; }
        //public DateTime Date { get; set; }
    }
}
