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
    public class GetStatusRequest : RequestBase<GetStatusResponse>
    {
        public GetStatusMessage Message { get; set; }
        public GetStatusRequest() : base("status", HttpMethod.Post)
        {
            Params.Add(new Param("format", "json"));
        }

        public override HttpContent ToHttpContent()
        {
            var jsonString = JsonConvert.SerializeObject(Message);
            return new StringContent(jsonString, Encoding.UTF8, "application/json");
        }

        public override GetStatusResponse DeserializeResponse(string responseJson)
        {
            var result = base.DeserializeResponse(responseJson);
            //  Вопрос - что выдавать? Там может быть много. Сделать отдельно?
            if (result.Status.ToLower() != "ok")
                throw new BadResponseException($"{result.Status} {string.Join("\n", result.Messages?.Select(a => $"{a.ProviderId} {a.Code})") ?? new List<string>())}");

            return result;
        }
    }
}
