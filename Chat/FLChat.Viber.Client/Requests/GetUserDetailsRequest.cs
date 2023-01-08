using FLChat.Viber.Client.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FLChat.Viber.Client.Requests
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class GetUserDetailsRequest : RequestBase<GetUserDetailsResponse>
    {
        public GetUserDetailsRequest(string userId) : base("get_user_details") {
            Id = userId;
        }

        [JsonProperty(Required = Required.Always)]
        public string Id { get; set; }

    }
}