using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;
using FLChat.DAL;
using System.Net;
using System.Data.Entity;

namespace FLChat.WebService.Handlers.WebChat
{
    public abstract class WebChatBase
    {
        protected WebChatDeepLink LoadWebChat(ChatEntities entities, IUserAuthInfo currUserInfo, string code) {
            if (currUserInfo.UserId != Guid.Empty)
                throw new ErrorResponseException(HttpStatusCode.Unauthorized, ErrorResponse.Kind.access_denied, "Can't perform this request with this token");

            WebChatDeepLink webChat = entities
                .WebChatDeepLink.Where(wc => 
                    wc.Link == code 
                    && wc.ExpireDate >= DateTime.UtcNow 
                    && wc.MessageToUser.Message.IsDeleted == false)
                .Include(wc => wc.MessageToUser)
                .Include(wc => wc.MessageToUser.Message)
                .Include(wc => wc.MessageToUser.ToTransport.User)
                .Include(wc => wc.MessageToUser.Message.FromTransport)
                .Include(wc => wc.MessageToUser.Message.FromTransport.User)
                .SingleOrDefault();
            if (webChat == null)
                throw new ErrorResponseException(HttpStatusCode.NotFound, ErrorResponse.Kind.not_found, "Message with this code has not found");

            if (webChat.MessageToUser.ToTransport.User.Enabled == false)
                throw new ErrorResponseException(HttpStatusCode.NotFound, ErrorResponse.Kind.user_not_found, "User for this message was disabled");

            return webChat;
        }
    }
}
