using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using System.Net;

namespace FLChat.WebService.Handlers.User
{
    public class UserSelectionCount : IObjectedHandlerStrategy<UserSelection, UserCountResponse>
    {
        public bool IsReusable => true;

        public UserCountResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, UserSelection input) {
            int count = entities.UserSelectionCount(currUserInfo.UserId, input.Convert());
            return new UserCountResponse(count);
        }
    }
}
