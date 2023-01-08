using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace FLChat.WebService.DataTypes
{
    public class SearchUserRequest : PartialDataRequest
    {
        [JsonProperty(PropertyName = "search")]
        public string SearchValue { get; set; }
    }
}
