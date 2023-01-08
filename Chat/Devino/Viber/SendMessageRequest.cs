using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Devino.Exceptions;
using FLChat.Devino.Request;
using Newtonsoft.Json;

namespace Devino.Viber
{
    public class SendMessageRequest : RequestBase<SendMessageResponse>
    {
        public ViberSendMessage Message { get; set; }
        public SendMessageRequest() : base("send", HttpMethod.Post)
        {
            Params.Add(new Param("format", "json"));
        }

        public override HttpContent ToHttpContent()
        {
            var jsonString = JsonConvert.SerializeObject(Message);
            return new StringContent(jsonString, Encoding.UTF8, "application/json");
        }

        public override SendMessageResponse DeserializeResponse(string responseJson)
        {
            var result = base.DeserializeResponse(responseJson);
            if (result.Status.ToLower() != "ok")
                throw new BadResponseException($"{result.Status} {string.Join("\n", result.Messages?.Select(a => $"{a.ProviderId} {a.Code})") ?? new List<string>())}");

            return result;
        }
    }
}
