using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.Viber.Client.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FLChat.Viber.Client.Requests
{
    /// <summary>
    /// Send text messages
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class SendTextMessageRequest : SendMessageRequest
    {
        public SendTextMessageRequest(Sender sender, string receiver, string text) : base(sender, receiver, MessageType.Text) {
            Text = text;
        }

        [JsonProperty(Required = Required.Always)]
        public string Text { get; set; }
    }
}
