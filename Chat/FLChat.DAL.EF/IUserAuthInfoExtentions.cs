using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;

namespace FLChat.DAL
{
    public static class IUserAuthInfoExtentions
    {
        public static User GetUser(this IUserAuthInfo userAuthInfo, ChatEntities entities) {
            return entities
                .User
                .Where(u => u.Id == userAuthInfo.UserId)
                .Single();
        }
    }
}
