using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FLChat.VKBotClient.Requests.Available_Methods.GetUserInfo
{
    public class GetUserInfoResponse
    {
        [JsonProperty(PropertyName = "response")]
        public List<UserInfoResponse> User { get; set; }
    }

    public class UserInfoResponse
    {
        [JsonProperty(PropertyName = "photo_50")]
        public string AvatarUrl { get; set; }

        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "mobile_phone")]
        public string MobilePhone { get; set; }

        [JsonProperty(PropertyName = "home_phone")]
        public string HomePhone { get; set; }
    }
}
