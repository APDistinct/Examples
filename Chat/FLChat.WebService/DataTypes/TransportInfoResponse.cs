using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;

using FLChat.DAL.Model;

namespace FLChat.WebService.DataTypes
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class TransportInfoResponse
    {
        private User _user;

        public TransportInfoResponse() {

        }

        public TransportInfoResponse(User user) {
            _user = user;
            Id = _user.Id.ToString();
            Transports = _user.Transports.Select(t => new TransportInfo(t)).ToArray();
        }

        public string Id { get; set; }

        public TransportInfo[] Transports { get; set; }        
    }
}
