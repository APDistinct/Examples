using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using FLChat.DAL;
using Newtonsoft.Json.Converters;

namespace FLChat.WebService.DataTypes
{
    public class UserSendInfo
    {
        /// <summary>
        /// Message addressee
        /// </summary>
        [JsonProperty(PropertyName = "user_id")]
        public Guid? ToUser { get; set; }

        /// <summary>
        /// Addressee transport
        /// </summary>
        [JsonProperty(PropertyName = "transport")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TransportKind? ToTransport { get; set; }

    }
    /// <summary>
    /// Data type for send message request
    /// </summary>
    public class SendMessageRequest : SendMessageBase
    {        
        /// <summary>
        /// Message addressee
        /// </summary>
        [JsonProperty(PropertyName = "to_user")]
        public Guid? ToUser { get; set; }

        /// <summary>
        /// Message addressee
        /// </summary>
        [JsonProperty(PropertyName = "to_segment")]
        public string ToSegment { get; set; }

        /// <summary>
        /// Addressee transport
        /// </summary>
        [JsonProperty(PropertyName = "to_transport")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TransportKind? ToTransport { get; set; }

        /// <summary>
        /// Message addressee
        /// </summary>
        [JsonProperty(PropertyName = "to_users")]
        public IEnumerable<UserSendInfo> ToUsers { get; set; }

        /// <summary>
        /// Message addressee
        /// </summary>
        [JsonProperty(PropertyName = "to_segments")]
        public IEnumerable<string> ToSegments { get; set; }

        /// <summary>
        /// Message addressee
        /// </summary>
        [JsonProperty(PropertyName = "to_phones")]
        public IEnumerable<string> ToPhones { get; set; }

        /// <summary>
        /// Message sends to matched phone list
        /// </summary>
        [JsonProperty(PropertyName = "to_phone_list")]
        public bool ToPhoneList { get; set; }

        /// <summary>
        /// Message text
        /// </summary>
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        /// <summary>
        /// Message file data
        /// </summary>
        [JsonProperty(PropertyName = "file")]
        public FileInfoData File { get; set; }

        /// <summary>
        /// Message file id
        /// </summary>
        [JsonProperty(PropertyName = "file_id")]
        public Guid? FileId { get; set; }

        /// <summary>
        /// Message DelayedStart date/time
        /// </summary>
        [JsonProperty(PropertyName = "delayed_start")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime? DelayedStart { get; set; }
    }
}
