using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace FLChat.WebService.DataTypes
{
    public class UserCountResponse
    {
        public UserCountResponse(int count) {
            Count = count;
        }

        [JsonProperty(PropertyName = "count")]
        public int Count { get; }
    }
}
