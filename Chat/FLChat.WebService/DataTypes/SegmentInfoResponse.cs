using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.WebService.DataTypes
{
    public class SegmentInfoResponse : SegmentInfo
    {
        public SegmentInfoResponse(Segment segment, int membersCount, 
            IEnumerable<User> users, User currUser, 
            Dictionary<Guid, MessageToUser> lastMessages, 
            Dictionary<Guid, int> unread) 
            : base(segment, membersCount) {
            UserList = users.ToUserInfoShort(currUser, lastMessages, unread);
        }

        [JsonProperty(PropertyName = "members")]
        public IEnumerable<UserInfoShort> UserList { get; }
    }
}
