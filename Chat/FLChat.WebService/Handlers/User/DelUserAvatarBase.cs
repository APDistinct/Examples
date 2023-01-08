using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace FLChat.WebService.Handlers
{
    public class DelUserAvatarBase
    {
        public bool IsReusable => true;

        public void ProcessRequest(ChatEntities entities, Guid id, bool getAll = false)
        {
            DAL.Model.User user = entities.User
                .Where(u => (u.Id == id) && (getAll || u.Enabled))
                .Include(t => t.UserAvatar)
                .SingleOrDefault();
            if (user != null)
            {
                if (user.UserAvatar != null)
                {
                    entities.UserAvatar.Where(t => t.UserId == id).Delete();
                    entities.SaveChanges();
                }
                return;
            }

            throw new ErrorResponseException(
                (int)HttpStatusCode.NotFound,
                new ErrorResponse(ErrorResponse.Kind.user_not_found, $"User with id {id} not found"));
        }
    }
}

