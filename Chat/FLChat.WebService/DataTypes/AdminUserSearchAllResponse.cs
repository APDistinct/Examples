using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.DataTypes
{    
    public class AdminUserSearchAllResponse : PartialDataResponse
    {
        public AdminUserSearchAllResponse(IEnumerable<User> users, IPartialData partial = null) : base(partial)
        {
            //User = user != null ? user.ToUserInfoShort(currUserId, lastMessages, unread) : null;
            UserList = users.Select(x => new UserInfoAdmin(x)).ToList();

            Count = users.Count();
        }

        //[JsonProperty(PropertyName = "user", DefaultValueHandling = DefaultValueHandling.Ignore)]
        //public UserInfoShort User { get; }

        [JsonProperty(PropertyName = "users")]
        public IEnumerable<UserInfoAdmin> UserList { get; }

        [JsonProperty(PropertyName = "total_users_count", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? TotalUsersCount { get; set; }
    }
}
