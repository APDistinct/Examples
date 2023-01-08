using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.DataTypes
{
    public class RankGetResponse
    {
        [JsonProperty(PropertyName = "ranks")]
        public IEnumerable<string> Ranks { get; set; }
    }
}
