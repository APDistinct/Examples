using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devino.Viber
{
    public class ViberSendMessage
    {
        [JsonProperty(PropertyName = "resendSms")]
        public bool ResendSms { get; set; }
        [JsonProperty(PropertyName = "messages")]
        public List<ViberMessage> Messages { get; set; }
    }

    public class ViberMessage
    {
        [JsonProperty(PropertyName = "subject")]
        public string Subject { get; set; }
        [JsonProperty(PropertyName = "priority")]
        public string Priority { get; set; }
        [JsonProperty(PropertyName = "validityPeriodSec")]
        public int ValidityPeriodSec { get; set; }
        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "contentType")]
        public string ContentType { get; set; }
        [JsonProperty(PropertyName = "content")]
        public MessageContent Content { get; set; }
        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }
        [JsonProperty(PropertyName = "smsText")]
        public string SmsText { get; set; }
        [JsonProperty(PropertyName = "smsSrcAddress")]
        public string SmsSrcAddress { get; set; }
        [JsonProperty(PropertyName = "smsValidityPeriodSec")]
        public int SmsValidityPeriodSec { get; set; }
    }

    public class MessageContent
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
        [JsonProperty(PropertyName = "caption")]
        public string Caption { get; set; }
        [JsonProperty(PropertyName = "action")]
        public string Action { get; set; }
        [JsonProperty(PropertyName = "imageUrl")]
        public string ImageUrl { get; set; }
    }
}
