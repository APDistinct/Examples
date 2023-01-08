using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FLChat.DAL;
using FLChat.DAL.Model;
using Newtonsoft.Json.Converters;
using FLChat.WebService.DataTypes.Converters;

namespace FLChat.WebService.DataTypes
{
    public class MessageInfo : MessageInfoBase
    {
        public MessageInfo(Message msg, Guid currentUserId, string _text = null) : base(msg)
        {
            Incoming = currentUserId != msg.FromUserId;
            File = msg.FileId != null ? new FileInfoShort(msg.FileInfo) : null;
            Text = _text ?? Message.Text;            
        }

        [JsonProperty(PropertyName = "tm")]
        [JsonConverter(typeof(FixedIsoDateTimeConverter))]
        public DateTime PostTm => Message.PostTm;

        //[JsonProperty(PropertyName = "kind")]
        //[JsonConverter(typeof(StringEnumConverter))]
        //public MessageKind Kind => Message.Kind;

        [JsonProperty(PropertyName = "from")]
        public Guid FromUserId => Message.FromUserId;

        [JsonProperty(PropertyName = "transport")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TransportKind FromTransport => Message.FromTransportKind;

        [JsonProperty(PropertyName = "incoming")]
        public bool Incoming { get; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; }/*=> Message.Text;*/

        [JsonProperty(PropertyName = "file")]
        public FileInfoShort File { set; get; } //=> Message.FileId;
    }

    public static class MessageToUserExtentions
    {
        /// <summary>
        /// Create object of type derived from 
        /// </summary>
        /// <param name="mtu"></param>
        /// <param name="currUserId"></param>
        /// <returns></returns>
        public static MessageInfo ToPersonalMessageInfo(this MessageToUser mtu, Guid currUserId, string text = null) {
            return mtu.Message.FromUserId == currUserId
                ? (MessageInfo)new MessageOutcomeToOneInfo(mtu, text)
                : new MessageIncomeInfo(mtu, currUserId, text);
        }
    }
}
