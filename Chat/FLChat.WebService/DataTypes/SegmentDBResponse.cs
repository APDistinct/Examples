using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.WebService.DataTypes
{
    public class SegmentDBResponse : PartialDataResponse
    {
        //  Подумать о присутствии currUserId - будет ли в нём необходимость?
        public SegmentDBResponse(Segment segment, int membersCount, IEnumerable<User> users, IPartialData data)
            : base(data)
        {
            UserList = users.Select(x => new UserInfoAdmin(x)).ToList();
            Segment = new SegmentInfo(segment, membersCount);
        }

        [JsonProperty(PropertyName = "segment")]
        public SegmentInfo Segment { get; set; }

        [JsonProperty(PropertyName = "members")]
        public IEnumerable<UserInfoAdmin> UserList { get; }
    }
}
