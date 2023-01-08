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
    public class MessageSentAddresseeInfo
    {
        private MessageStatsRowsView _item;

        public MessageSentAddresseeInfo(MessageStatsRowsView item) {
            _item = item;
        }

        public Guid User => _item.ToUserId.Value;

        [JsonConverter(typeof(StringEnumConverter))]
        public TransportKind Transport => (TransportKind)_item.ToTransportTypeId;

        public bool IsWebChat => _item.IsWebChat != 0;

        public bool IsFailed => _item.IsFailed != 0;

        public bool IsSent => _item.IsSent != 0;

        public bool IsQuequed => _item.IsQuequed != 0;

        [JsonProperty(PropertyName = "cant_send")]
        public bool? CantSendToWebChat => IsWebChat ? _item.CantSendToWebChat != 0 : (bool?)null;

        public bool? IsWebChatAccepted => IsWebChat ? _item.IsWebChatAccepted != 0 : (bool?)null;

        public bool? IsWebFormRequested => IsWebChat ? _item.IsSmsUrlOpened != 0 : (bool?)null;

    }
}
