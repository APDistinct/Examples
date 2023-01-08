using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Devino.Exceptions;
using FLChat.Devino.DvTypes;
using Newtonsoft.Json;

namespace FLChat.Devino.Request
{
    public class SendEmailRequest : RequestBase<SendEmailResponse>
    {
        public DvEmailRequest DvEmail { get; set; }

        public SendEmailRequest() : base("messages", HttpMethod.Post)
        {
            Params.Add(new Param("format", "json"));
        }

        public override HttpContent ToHttpContent()
        {
            var jsonString = JsonConvert.SerializeObject(DvEmail);
            return new StringContent(jsonString, Encoding.UTF8, "application/json");
        }

        public override SendEmailResponse DeserializeResponse(string responseJson)
        {
            var result = base.DeserializeResponse(responseJson);
            if (result.Code.ToLower() != "ok")
                throw new BadResponseException($"{result.Code} {result.Description}");

            return result;
        }
    }
}
