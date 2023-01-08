using FLChat.DAL.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.DataTypes
{
    /// <summary>
    /// Another tables which used are: [Dir].[Rank]
    /// </summary>
    public class UserInfoAdmin : UserInfoBase
    {
        public UserInfoAdmin(User user) : base(user)
        {
        }

        [JsonProperty(PropertyName = "is_deleted", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsDeleted => !_user.Enabled;
    }
}
