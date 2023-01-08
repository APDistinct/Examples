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
using FLChat.WebService.DataTypes;
using FLChat.WebService.MediaType;

namespace FLChat.WebService.Handlers
{
    public class SetUserAvatar : SetUserAvatarBase, IByteArrayHandlerStrategy
    {
        private readonly bool _getAll;

        public SetUserAvatar(IMediaTypeChecker avatarChecker = null, bool getAll = false) : base(avatarChecker)
        {
            _getAll = getAll;
        }        

        public int ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, NameValueCollection parameters, 
            byte[] requestData, string requestContentType, 
            out byte[] responseData, out string responseContentType, out string fileName)
        {
            if (Guid.TryParse(this.GetKey(parameters), out Guid id))
            {
                fileName = null;
                return ProcessRequest(entities, id, requestData, requestContentType, 
                    out responseData, out responseContentType, SizeLimit ,_getAll);
            }

            throw new ErrorResponseException(
                (int)HttpStatusCode.NotFound,
                new ErrorResponse(ErrorResponse.Kind.user_not_found, $"User with id {this.GetKey(parameters)} not found"));
        }        
    }
}
