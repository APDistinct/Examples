using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FLChat.Devino.Request;
using Newtonsoft.Json;

namespace FLChat.Devino.SMS.Request
{
    public class SessionIdRequest : RequestBase<SessionIdResponse>
    {
        public SessionIdRequest() : base("User/SessionId", HttpMethod.Get)
        {
        }

        public override SessionIdResponse DeserializeResponse(string responseJson)
        {
            var result = JsonConvert.DeserializeObject<string>(responseJson);
            return new SessionIdResponse {SessionId = result };
        }
    }
}
