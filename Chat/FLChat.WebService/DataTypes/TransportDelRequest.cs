using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;

using FLChat.DAL.Model;
using FLChat.DAL;
using FLChat.WebService.Utils;

namespace FLChat.WebService.DataTypes
{
    //[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class TransportDelRequest
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "transport")]
        //[JsonConverter(typeof(StringEnumConverter))]
        public string TransportString { get; set; }

        [JsonIgnore]
        public TransportKind? Transport
        { get => TransportString.GetValue<TransportKind>();
            set => TransportString = value.ToString(); }
        


    }
}
