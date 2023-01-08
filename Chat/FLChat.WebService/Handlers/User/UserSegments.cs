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
    public class UserSegments : IObjectedHandlerStrategy<string, SegmentListResponse>
    {
        public bool IsReusable => true;

        public SegmentListResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, string input) {
            if (String.IsNullOrEmpty(input) || !Guid.TryParse(input, out Guid id))
                throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error,
                    $"User id <{input}> is incorrect");

            DAL.Model.User user = entities.User.Where(u => u.Id == id).SingleOrDefault();
            if (user == null)
                throw new ErrorResponseException(HttpStatusCode.NotFound, ErrorResponse.Kind.user_not_found,
                    $"User with id <{input}> has not found");

            var list = user.Segments.ToArray().Select(s => new SegmentInfo(s));
            return new SegmentListResponse() { Segments = list };
        }
    }
}
