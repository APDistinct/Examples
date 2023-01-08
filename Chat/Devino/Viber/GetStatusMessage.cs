using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Devino.Viber
{
    public class GetStatusMessage
    {
        [JsonProperty(PropertyName = "messages")]
        public List<string> Messages { get; set; }
    }
}
