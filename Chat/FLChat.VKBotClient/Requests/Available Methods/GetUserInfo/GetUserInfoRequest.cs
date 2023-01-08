using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FLChat.VKBotClient.Requests.Available_Methods.GetUserInfo
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class GetUserInfoRequest:RequestBase<GetUserInfoResponse>
    {
        [JsonProperty(Required = Required.Always)]
        public string UserId { get; }

        [JsonProperty(Required = Required.Always)]
        public string Fields { get; }

        [JsonProperty(Required = Required.Always)]
        public string NameCase { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public GetUserInfoRequest(string userId) : base("users.get")
        {
            UserId = userId;
            Fields = "photo_50,contacts";
            NameCase = "Nom";
            _rnd = new Random();
        }

        Random _rnd;

        public override string Params => $"user_ids={UserId}&fields={Fields}&name_case={NameCase}&random_id={_rnd.Next()}";
    }
}
