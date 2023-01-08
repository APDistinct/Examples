using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;
using Newtonsoft.Json;

namespace FLChat.WebService.DataTypes
{
    public class UserParentResponse
    {
        [JsonIgnore]
        private readonly UserChildResponse _resp;

        public UserParentResponse(User user, IEnumerable<User> parents, User currUser, 
            Dictionary<Guid, MessageToUser> lastMessages, 
            Dictionary<Guid, int> unread,
            Dictionary<Guid, List<string>> tags = null,
            HashSet<Guid> broadcastProhibitionStructure = null) 
        {
            _resp = new UserChildResponse(user, parents, currUser, lastMessages, unread, 
                tags: tags, 
                broadcastProhibitionStructure: broadcastProhibitionStructure);            
        }

        [JsonProperty(PropertyName = "user")]
        public UserInfoShort User => _resp.User;


        [JsonProperty(PropertyName = "parents")]
        public IEnumerable<UserInfoShort> UserList => _resp.UserList.Select(u => { u.HasChilds = true; return u; });
    }
}
