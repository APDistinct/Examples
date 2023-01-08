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
using FLChat.WebService.DataTypes.Converters;

namespace FLChat.WebService.DataTypes
{
    /// <summary>
    /// Base information about message content
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class MessageContentInfo : MessageInfoBase
    {
        public MessageContentInfo(Message msg, FileInfo fi = null) : base(msg) {
            if (fi != null) {
                if (fi.Id != msg.FileId)
                    throw new ArgumentException("FileId in FileInfo and FileId in Message are not same");
                File = new FileInfoShort(fi);
            } else if (Message.FileId != null)
                File = new FileInfoShort(Message.FileInfo);
        }

        [JsonProperty(PropertyName = "tm")]
        [JsonConverter(typeof(FixedIsoDateTimeConverter))]
        public DateTime PostTm => Message.PostTm;

        [JsonProperty(PropertyName = "tm_started")]
        [JsonConverter(typeof(FixedIsoDateTimeConverter))]
        public DateTime? DelayedStart => Message.DelayedStart;

        [JsonConverter(typeof(StringEnumConverter))]
        public MessageKind Kind => Message.Kind;

        public string Text => Message.Text;

        public FileInfoShort File { get; private set; } //=> Message.FileId;
    }
}
