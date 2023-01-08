using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL;
using Newtonsoft.Json;

namespace FLChat.WebService.Handlers.Auth
{
    public class TokenPayload : IUserAuthInfo
    {
        [JsonProperty(PropertyName = "id")]
        public Guid UserId { get; set; }

        [JsonProperty(PropertyName = "iss")]
        public DateTime Iss { get; set; }

        [JsonProperty(PropertyName = "exp")]
        public int Exp { get; set; }

        [JsonIgnore]
        public bool IsExpired => (DateTime.Now - Iss).TotalSeconds > Exp;
    }
}
