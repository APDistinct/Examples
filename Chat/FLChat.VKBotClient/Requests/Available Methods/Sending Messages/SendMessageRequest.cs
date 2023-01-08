using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FLChat.VKBotClient.Response;
using FLChat.VKBotClient.Types;
using FLChat.VKBotClient.Types.Attachments;
using FLChat.VKBotClient.Types.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FLChat.VKBotClient.Requests.Available_Methods.Sending_Messages
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class SendMessageRequest : RequestBase<SendMessagesResponse>
    {
        private readonly string _attachments;

        [JsonProperty(Required = Required.Always)]
        public string UserId { get; }

        /// <summary>
        /// Message of the message to be sent
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Message { get; }
        public string KbString { get; }

        public SendMessageRequest(string userId, string message, string kbString, string attachments)
            : base("messages.send", HttpMethod.Post)
        {
            _attachments = attachments;
            UserId = userId;
            Message = message;
            KbString = kbString;
        }

        public override HttpContent ToHttpContent()
        {

            //HttpClient httpClient = new HttpClient();
            MultipartFormDataContent content = new MultipartFormDataContent();

            content.Add(new StringContent(Message), "message");            
            if (KbString != null)
            {
                content.Add(new StringContent(KbString), "keyboard");
            }

            if (!string.IsNullOrEmpty( _attachments))
            {
                content.Add(new StringContent(_attachments), "attachment");
            }

            //var requestParameters = new List<KeyValuePair<string, string>>()
            //{                
            //    new KeyValuePair<string,string>("message", Message),
            //};
            //if(KbString != null)
            //{
            //    requestParameters.Add(new KeyValuePair<string, string>("keyboard", KbString));
            //}
            //var content = new MultipartFormDataContent {new FormUrlEncodedContent(requestParameters)};
            return content;
        }

        Random _rnd = new Random();

        //public override string Params => $"user_id={UserId}&message={Message}&random_id={_rnd.Next()}";

        public override string Params => $"user_ids={UserId}&random_id={_rnd.Next()}";
    }
}
