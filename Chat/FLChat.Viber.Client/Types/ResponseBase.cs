using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.Viber.Client.Exceptions;

namespace FLChat.Viber.Client.Types
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class ResponseBase
    {
        [JsonProperty(Required = Required.Always)]
        public int Status { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string StatusMessage { get; set; }

        [JsonIgnore]
        public bool IsOk => Status == 0;

        public void EnsureIsOk() {
            if (!IsOk)
                throw new ApiRequestException($"Error {Status.ToString()}: {StatusMessage}");
        }
    }
}
