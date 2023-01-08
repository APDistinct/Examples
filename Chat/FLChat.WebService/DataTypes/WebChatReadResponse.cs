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
    /// <summary>
    /// Response for request WebChat message
    /// </summary>
    public class WebChatReadResponse : DeepLinkResponse
    {
        //[JsonProperty(PropertyName = "user")]
        //public UserProfileInfo User { get; set; }

        [JsonProperty(PropertyName = "from_user")]
        public UserProfileInfo Sender { get; set; }

        [JsonProperty(PropertyName = "message")]
        public MessageInfo Message { get; set; }
    }
}
