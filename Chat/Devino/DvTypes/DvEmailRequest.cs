using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Devino.DvTypes
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Sender
    {
        [JsonProperty(PropertyName = "Address")]
        public string Address { get; set; }
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Recipient
    {
        [JsonProperty(PropertyName = "MergeFields")]
        public Dictionary<string, string> MergeFields;
        [JsonProperty(PropertyName = "RecipientId")]
        public string RecipientId { get; set; }
        [JsonProperty(PropertyName = "Address")]
        public string Address { get; set; }
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Body
    {
        [JsonProperty(PropertyName = "Html")]
        public string Html { get; set; }
        [JsonProperty(PropertyName = "PlainText")]
        public string PlainText { get; set; }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class DvEmailRequest
    {
        [JsonProperty(PropertyName = "Sender")]
        public Sender Sender;
        [JsonProperty(PropertyName = "Recipients")]
        public IEnumerable<Recipient> Recipients;
        [JsonProperty(PropertyName = "Subject")]
        public string Subject { get; set; }
        [JsonProperty(PropertyName = "Body")]
        public Body Body;
        [JsonProperty(PropertyName = "UserCampaignId")]
        public string UserCampaignId { get; set; }
    }
}
