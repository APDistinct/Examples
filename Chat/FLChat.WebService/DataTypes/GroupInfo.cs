using FLChat.DAL.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.DataTypes
{
    public class GroupInfo : GroupInfoShort
    {
        public GroupInfo(Group group) : base(group)
        { }

        [JsonProperty(PropertyName = "members")]
        public IEnumerable<Guid> Members => _group.MemberList;
        [JsonProperty(PropertyName = "admins")]
        public IEnumerable<Guid> Admins => _group.AdminList;
    }
}
