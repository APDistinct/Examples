using FLChat.VKBotClient.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.VKBotClient.Requests.Available_Methods.Update
{
    public class LongPollCommonInfoRequest : RequestBase<LongPollCommonInfoResponse>
    {
        public override string Params => "";

        public LongPollCommonInfoRequest()
    : base("groups.getLongPollServer", HttpMethod.Get)
        {
        }
    }
}
