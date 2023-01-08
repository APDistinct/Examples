using FLChat.DAL;
using FLChat.DAL.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.DataTypes
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class MessageTypeLimit
    {
        //[JsonConverter(typeof(StringEnumConverter))]
        //public MessageKind Type { get; }
        [JsonProperty(Required = Required.AllowNull, NullValueHandling = NullValueHandling.Include)]
        public int? LimitForDay { get; set; }
        [JsonProperty(Required = Required.AllowNull, NullValueHandling = NullValueHandling.Include)]
        public int? LimitForOnce { get; set; }

        public MessageTypeLimit()
        {
        }

        public MessageTypeLimit(MessageType mtype)
        {
            Kind = mtype.Kind;
            LimitForDay = mtype.LimitForDay;
            LimitForOnce = mtype.LimitForOnce;            
        }

        [JsonProperty()]
        public string Type { get; set; }

        [JsonIgnore]
        public MessageKind Kind { set => Type = value.ToString(); get => (MessageKind)Enum.Parse(typeof(MessageKind), Type); }          
    }
}
