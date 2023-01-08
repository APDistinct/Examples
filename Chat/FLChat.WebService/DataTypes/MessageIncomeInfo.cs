using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using FLChat.DAL.Model;
using FLChat.Core.MsgCompilers;

namespace FLChat.WebService.DataTypes
{
    /// <summary>
    /// Incoming message info
    /// Extends message information with field IsRead
    /// </summary>
    public class MessageIncomeInfo : MessageInfo
    {
        public MessageIncomeInfo(MessageToUser msgto, Guid currentUserId, string text = null) 
            : base(msgto.Message, currentUserId, text)
        {
            ToUser = msgto;            
        }

        [JsonIgnore]
        public MessageToUser ToUser { get; }

        [JsonProperty(PropertyName = "is_read")]
        public bool IsRead => ToUser.IsRead;
    }
}
