using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using FLChat.WebService.Utils;

namespace FLChat.WebService.Handlers.User
{
    public class DelUserTransport : IObjectedHandlerStrategy<TransportDelRequest, object>
    {
        public static readonly string Key = typeof(TransportDelRequest).GetJsonPropertyName(nameof(TransportDelRequest.Id));
        public bool IsReusable => true;

        private readonly int TransportKindLimit = 100;  // 

        public object ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, TransportDelRequest input)
        {
            if (!Guid.TryParse(input.Id, out Guid userId))
                throw new ErrorResponseException(HttpStatusCode.NotFound, ErrorResponse.Kind.user_not_found, $"Bad user id [{input.Id}] ");

            DAL.Model.User user = entities.User.Where(u => u.Id == userId && u.Enabled).Include(u => u.Transports).FirstOrDefault();
            if (user == null)
                throw new ErrorResponseException(HttpStatusCode.NotFound, ErrorResponse.Kind.user_not_found, $"user with id [{input.Id}] has not found");

            if (input.Transport == null)
                throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.access_denied, $"Bad type of transport {input.TransportString}");

            if ((int)input.Transport >= TransportKindLimit)
                throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.access_denied, $"Can't delete {input.Transport}({(int)input.Transport}) transport (only for less {TransportKindLimit})");

            DAL.Model.Transport t = user.Transports.Get(input.Transport.Value);
            if (t != null)
            {
                if (t.Kind == TransportKind.Viber || t.Kind == TransportKind.VK || t.Kind == TransportKind.Telegram)
                {
                    t.TransportOuterId = ""/*null*/;
                    //entities.SaveChanges();
                }
                t.Enabled = false;
            }

            entities.SaveChanges();
            return null;
        }
    }
}
