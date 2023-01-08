using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;
using Newtonsoft.Json;

namespace FLChat.WebService.DataTypes
{
    public class UserInfoSimple : UserInfoBase
    {
        public UserInfoSimple(User user) : base(user)
        {
        }
    }
}
