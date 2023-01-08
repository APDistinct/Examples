using FLChat.VKBotClient.Requests;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace FLChat.VKBotClient.Types
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class LongPollServerRequest : RequestBase<LongPollServerResponse>
    {
        public override string Params => $"group_id={GroupId}";

        [JsonProperty(Required = Required.Always)]
        public int GroupId { get; }

        public LongPollServerRequest(int groupId)
    : base("groups.getLongPollServer", HttpMethod.Get)
        {
            GroupId = groupId;
        }
    }
}
