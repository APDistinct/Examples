using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace FLChat.WebService.DataTypes
{
    /// <summary>
    /// Response data for SendMessage request
    /// </summary>
    public class SendMessageResponse
    {        
        [JsonProperty(PropertyName = "message_id")]
        public Guid? MessageId { get; set; }

        [JsonProperty(PropertyName = "file_id")]
        public Guid? FileId { get; set; }        
    }
}
