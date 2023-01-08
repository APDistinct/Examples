using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using FLChat.DAL.Model;

namespace FLChat.WebService.DataTypes
{
    /// <summary>
    /// base class for message information
    /// </summary>
    public class MessageInfoBase
    {
        [JsonIgnore]
        public Message Message { get; }

        public MessageInfoBase(Message msg) {
            Message = msg;
        }

        [JsonProperty(PropertyName = "id")]
        public Guid Id => Message.Id;
    }
}
