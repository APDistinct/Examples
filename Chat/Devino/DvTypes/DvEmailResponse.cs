using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Devino.DvTypes
{
    public class Result
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

    public class DvEmailResponse
    {
        [JsonProperty(PropertyName = "Result")]
        public IEnumerable<Result> Result;

        [JsonProperty(PropertyName = "Code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

    }
}

