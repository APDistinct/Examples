using FLChat.DAL;
using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.Handlers
{
    public class DelUserAvatar : DelUserAvatarBase, IObjectedHandlerStrategy<string, object>
    {
        private readonly bool _getAll;

        public DelUserAvatar(bool getAll = false)
        {
            _getAll = getAll;
        }

        public object ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, string input) {
            if (Guid.TryParse(input, out Guid id)) {
                ProcessRequest(entities, id, _getAll);
                return null;
            }

            throw new ErrorResponseException(
                (int)HttpStatusCode.NotFound,
                new ErrorResponse(ErrorResponse.Kind.user_not_found, $"User with id {input} not found"));
        }
    }
}
