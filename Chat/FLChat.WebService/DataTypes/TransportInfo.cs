using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;

using FLChat.DAL;
using FLChat.DAL.Model;

namespace FLChat.WebService.DataTypes
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class TransportInfo
    {
        public TransportInfo() {
        }

        public TransportInfo(DAL.Model.Transport transport) {
            Kind = transport.Kind;
            Enabled = transport.Enabled;
            TransportId = transport.TransportOuterId;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public TransportKind Kind { get; set; }

        public bool Enabled { get; set; }

        public string TransportId { get; set; }
    }
}
