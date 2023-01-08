using FLChat.DAL;
using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.Handlers
{
    public class DelProfileAvatar : DelUserAvatarBase, IObjectedHandlerStrategy<object, object>
    {
        public object ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, object input)
        {
            ProcessRequest(entities, currUserInfo.UserId, false);
            return null;
        }
    }
}
