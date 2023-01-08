using FLChat.VKBotClient.Response;
using FLChat.VKBotClient.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.VKBotClient.Requests.Available_Methods.Sending_Messages
{
    public class OneMessageInfo
    {
        [JsonProperty(PropertyName = "message_id")]
        public int MessageId { get; set; }
        [JsonProperty(PropertyName = "peer_id")]
        public int PeerId { get; set; }
        [JsonProperty(PropertyName = "error")]
        public ErrorResponse Error { get; set; }
    }

    public class SendMessagesResponse : VkError
    {
        [JsonProperty(PropertyName = "response")]
        public List<OneMessageInfo> Messages { get; set; }        
    }

    public class SendMessageResponse
    {
        [JsonProperty(PropertyName = "response")]
        public string Id { get; set; }        
    }

    public class SendMessageResponseError : VkError
    {
        [JsonProperty(PropertyName = "error")]
        public string Id { get; set; }        
    }
    //public class VkError
    //{
    //    [JsonProperty(PropertyName = "error")]
    //    public VkErrorInfo Error { get; set; }        
    //}

    //public class VkErrorInfo
    //{
    //    [JsonProperty(PropertyName = "error_code")]
    //    public string Code { get; set; }  
        
    //    [JsonProperty(PropertyName = "error_msg")]
    //    public string Msg { get; set; }

    //    //[JsonProperty(PropertyName = "request_params")]
    //    //public Dictionary<string, string> RequestParams { get; set; }        
    //}
}
