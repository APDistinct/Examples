using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.Utils;

namespace FLChat.WebService.Handlers
{
    public class GetUserAvatarBase
    {        
        public bool IsReusable => true;

        public static int ProcessRequest(ChatEntities entities, Guid id, byte[] requestData, string requestContentType,
            out byte[] responseData, out string responseContentType, out string fileName, bool getAll = false)
        {
            DAL.Model.User user = entities.User
                .Where(u => (u.Id == id) && (getAll || u.Enabled))
                .Include(t => t.UserAvatar)
                .Include(t => t.UserAvatar.MediaType)
                .SingleOrDefault();
            if (user != null)
            {
                responseData = null;
                responseContentType = "";
                if (user.UserAvatar == null)
                {
                    fileName = null;
                    return (int)HttpStatusCode.NoContent;
                }
                responseData = user.UserAvatar.Data;
                responseContentType = user.UserAvatar.MediaType.Name;
                fileName = String.Concat(user.FullName ?? user.Id.ToString(), ".", user.UserAvatar.MediaType.Name.MediaTypeExtention() ?? "");
                return (int)HttpStatusCode.OK;
            }

            throw new ErrorResponseException(
                (int)HttpStatusCode.NotFound,
                new ErrorResponse(ErrorResponse.Kind.user_not_found, $"User with id {id} not found"));
        }
    }
}

