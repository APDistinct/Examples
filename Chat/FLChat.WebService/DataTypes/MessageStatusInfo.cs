using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using FLChat.DAL;
using FLChat.DAL.Model;
using Newtonsoft.Json.Converters;

namespace FLChat.WebService.DataTypes
{
    /// <summary>
    /// Message change status information (delivered, read, failed)
    /// </summary>
    public class MessageStatusInfo : MessageInfoBase
    {
        [JsonIgnore]
        public Event Event { get; }

        public MessageStatusInfo(Event ev) : base(ev.Message) {
            Event = ev;
        }

        [JsonProperty(PropertyName = "user_id")]
        public Guid UserId => Event.CausedByUserId;

        [JsonProperty(PropertyName = "transport")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TransportKind TransportKind => Event.CausedByTransportKind.Value;
    }
}
