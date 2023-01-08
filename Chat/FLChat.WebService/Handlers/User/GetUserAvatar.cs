using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;

namespace FLChat.WebService.Handlers
{
    public class GetUserAvatar : GetUserAvatarBase, IByteArrayHandlerStrategy
    {
        private readonly bool _getAll;

        public GetUserAvatar(bool getAll = false)
        {
            _getAll = getAll;
        }
        
        public int ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, NameValueCollection parameters, 
            byte[] requestData, string requestContentType, 
            out byte[] responseData, out string responseContentType, out string fileName)
        {
            if (Guid.TryParse(this.GetKey(parameters), out Guid id))
                return ProcessRequest(entities, id, requestData, requestContentType, 
                    out responseData, out responseContentType, out fileName,
                    _getAll);

            throw new ErrorResponseException(
                (int)HttpStatusCode.NotFound,
                new ErrorResponse(ErrorResponse.Kind.user_not_found, $"User with id {this.GetKey(parameters)} not found"));
        }
    }
}
