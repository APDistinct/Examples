using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.MediaType;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FLChat.Core.Media;

namespace FLChat.WebService.Handlers.File
{
    public class GetImage : GetFile
    {
        public GetImage(IMediaTypeChecker fileChecker = null) : base(fileChecker)
        {

        }

        public override int ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, NameValueCollection parameters,
            byte[] requestData, string requestContentType, 
            out byte[] responseData, out string responseContentType, out string fileName)
        {
            if (Guid.TryParse(this.GetKey(parameters), out Guid fileId))
            {
                var ret = ProcessRequest(entities, fileId, requestData, requestContentType, 
                    out responseData, out responseContentType, out fileName);
                if (responseData.GetFileImageType() != null)
                {
                    return ret;
                }
                throw new ErrorResponseException(
                    (int)HttpStatusCode.UnsupportedMediaType,
                    new ErrorResponse(ErrorResponse.Kind.not_support, $"File with id {fileId} is not an Image"));
            }
            throw new ErrorResponseException(
               (int)HttpStatusCode.NotFound,
               new ErrorResponse(ErrorResponse.Kind.not_found, $"User with id {this.GetKey(parameters)} not found"));
        }
    }
}
