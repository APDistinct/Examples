using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FLChat.WebService.DataTypes
{
    /// <summary>
    /// Information about outcome message with single addressee
    /// </summary>
    public class MessageOutcomeToOneInfo : MessageInfo
    {
        [JsonIgnore]
        public MessageToUser ToUser { get; }

        public MessageOutcomeToOneInfo(MessageToUser msg, string text = null) : base(msg.Message, msg.Message.FromUserId, text) {
            ToUser = msg;
        }

        [JsonProperty(PropertyName = "to_user")]
        public Guid ToUserId => ToUser.ToUserId;

        [JsonProperty(PropertyName = "to_transport")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TransportKind ToTransport => ToUser.ToTransportKind;

        [JsonProperty(PropertyName = "status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageStatus Status => ToUser.GetMessageStatus();
    }
}
