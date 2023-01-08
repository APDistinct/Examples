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
    public class UserChildResponse : PartialDataResponse
    {
        public UserChildResponse(User user, IEnumerable<User> childs, User currUser,
            Dictionary<Guid, MessageToUser> lastMessages,
            Dictionary<Guid, int> unread,
            Dictionary<Guid, List<string>> tags = null,
            IPartialData partial = null,
            HashSet<Guid> broadcastProhibitionStructure = null,
            HashSet<Guid> hasChilds = null) : base(partial)
        {
            User = user?.ToUserInfoShort(currUser, lastMessages, unread, tags, broadcastProhibitionStructure, hasChilds);
            UserList = childs.ToUserInfoShort(currUser, lastMessages, unread, tags, 
                broadcastProhibitionStructure, hasChilds);

            Count = childs.Count();
        }

        public UserChildResponse(User user, IEnumerable<UserEx> childs, User currUser,
            Dictionary<Guid, MessageToUser> lastMessages,
            Dictionary<Guid, int> unread,
            Dictionary<Guid, List<string>> tags = null,
            IPartialData partial = null) : base(partial) {
            User = user?.ToUserInfoShort(currUser, lastMessages, unread, tags);
            UserList = childs.Select(x => {
                UserInfoShort ui = x.User.ToUserInfoShort(currUser, lastMessages, unread, tags);
                ui.HasChilds = x.HasChilds;
                ui.BroadcastProhibitionStructure = x.BroadcastProhibitionStructure;
                return ui;
            });

            Count = childs.Count();
        }

        [JsonProperty(PropertyName = "user", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public UserInfoShort User { get; }

        [JsonProperty(PropertyName = "childs")]
        public IEnumerable<UserInfoShort> UserList { get; }

        [JsonProperty(PropertyName = "total_childs_count", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? TotalChildsCount { get; set; }
    }
}
