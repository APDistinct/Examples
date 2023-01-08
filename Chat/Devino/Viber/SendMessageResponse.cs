using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devino.Viber
{
    public class SendMessageResponse
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
        [JsonProperty(PropertyName = "messages")]
        public List<MessageResponse> Messages { get; set; }
    }
    
    public class MessageResponse
    {
        [JsonProperty(PropertyName = "providerId")]
        public string ProviderId { get; set; }
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }
    }
}
