using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FLChat.Viber.Client.Types
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class GetUserDetailsResponse : SendMessageResponse
    {
        [JsonProperty]
        public User User { get; set; }
    }
}