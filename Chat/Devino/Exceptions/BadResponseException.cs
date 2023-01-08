using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Devino.Exceptions
{
    public class BadResponseException : Exception
    {
        public BadResponseException(string message) : base(message)
        {
            
        }
    }

    public class BadResponse
    {
        [JsonProperty(PropertyName = "Code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "Desc")]
        public string Description { get; set; }
    }
}
