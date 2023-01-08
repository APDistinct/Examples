using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;

namespace FLChat.WebService.DataTypes
{
    public class UserContactsResponse : PartialDataResponse
    {
        public UserContactsResponse(
            IEnumerable<User> users, 
            User currUser, 
            Dictionary<Guid, MessageToUser> lastMessages, 
            Dictionary<Guid, int> unread,
            Dictionary<Guid, List<string>> tags = null,
            IPartialData partial = null,
            HashSet<Guid> broadcastProhibitionStructure = null,
            HashSet<Guid> hasChilds = null) : base(partial) { 
            UserList = users.ToUserInfoShort(currUser, lastMessages, unread, tags, 
                broadcastProhibitionStructure, hasChilds);  
        }

        [JsonProperty(PropertyName = "users")]
        public IEnumerable<UserInfoShort> UserList { get; }

        [JsonProperty(PropertyName = "members")]
        public IEnumerable<UserInfoShort> ObsoleteMembers => UserList;
    }
}
