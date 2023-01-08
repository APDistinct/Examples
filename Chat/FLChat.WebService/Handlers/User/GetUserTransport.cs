using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using Newtonsoft.Json.Linq;

using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;

namespace FLChat.WebService.Handlers.User
{
    public class GetUserTransport : IObjectedHandlerStrategy<string, TransportInfoResponse>
    {
        public bool IsReusable => true;

        public TransportInfoResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, string input) {
            if (! Guid.TryParse(input, out Guid userId))
                throw new ErrorResponseException(HttpStatusCode.NotFound, ErrorResponse.Kind.user_not_found, $"user with id [{input}] has not found");

            DAL.Model.User user = entities.User.Where(u => u.Id == userId && u.Enabled).Include(u => u.Transports).FirstOrDefault();
            if (user == null)
                throw new ErrorResponseException(HttpStatusCode.NotFound, ErrorResponse.Kind.user_not_found, $"user with id [{input}] has not found");

            return new TransportInfoResponse(user);
        }
    }
}
