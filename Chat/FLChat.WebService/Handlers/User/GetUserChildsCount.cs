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
    /// <summary>
    /// Get count of users in structure
    /// </summary>
    public class GetUserChildsCount : IObjectedHandlerStrategy<string, UserChildCountResponse>
    {
        public bool IsReusable => true;

        public GetUserChildsCount() {
        }

        public UserChildCountResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, string input) {
            if (input == null) //profile
                return ProcessRequest(entities, currUserInfo.UserId);
            else if (Guid.TryParse(input, out Guid id)) //user id
                return ProcessRequest(entities, id);

            throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.uri_key_error, $"Invalid id: {input}");
        }

        private UserChildCountResponse ProcessRequest(ChatEntities entities, Guid userId) {
            return new UserChildCountResponse() {
                UserId = userId,
                Count = entities.User_GetChilds(userId, null, null).Count()
            };
        }
    }
}
