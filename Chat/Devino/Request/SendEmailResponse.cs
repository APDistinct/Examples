using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FLChat.Devino.Request
{
    public class SendEmailResponse
    {
        [JsonProperty(PropertyName = "Result")]
        public List<SendEmailResult> Result;

        [JsonProperty(PropertyName = "Code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

    }

    public class SendEmailResult
    {
        [JsonProperty(PropertyName = "Index")]
        public int Index { get; set; }

        [JsonProperty(PropertyName = "Address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "MessageId")]
        public string MessageId { get; set; }

        [JsonProperty(PropertyName = "Code")]
        public string Code { get; set; }
    }
}