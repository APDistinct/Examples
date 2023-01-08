using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using FLChat.WebService.Utils;

namespace FLChat.WebService.Handlers.Message
{
    public class MessageTypeLimitSet : IObjectedHandlerStrategy<MessageTypeLimit, MessageTypeLimit>
    {
        public static readonly string Key = typeof(MessageTypeLimit).GetJsonPropertyName(nameof(MessageTypeLimit.Type));
        public bool IsReusable => true;       

        public MessageTypeLimit ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, MessageTypeLimit input)
        {
            if(input.Kind == MessageKind.Personal)
                throw new ErrorResponseException(
                       (int)HttpStatusCode.BadRequest,
                       new ErrorResponse(ErrorResponse.Kind.not_support, $"Type {input.Type.ToString() } not permitted"));
            var type = entities.MessageType.Where(x => x.Id == (int)input.Kind).FirstOrDefault();
            if(type == null)
                throw new ErrorResponseException(
                (int)HttpStatusCode.NotFound,
                new ErrorResponse(ErrorResponse.Kind.not_found, $"Type {input.Type.ToString() } not found"));
            if (input.LimitForDay.HasValue && input.LimitForDay <= 0)
                throw new ErrorResponseException(
                       (int)HttpStatusCode.BadRequest,
                       new ErrorResponse(ErrorResponse.Kind.not_support, $"LimitForDay must be grater then 0 {input.LimitForDay } not permitted"));
            if (input.LimitForOnce.HasValue && input.LimitForOnce <= 0)
                throw new ErrorResponseException(
                       (int)HttpStatusCode.BadRequest,
                       new ErrorResponse(ErrorResponse.Kind.not_support, $"LimitForOnce must be grater then 0 {input.LimitForOnce} not permitted"));            
            type.LimitForDay = input.LimitForDay;
            type.LimitForOnce = input.LimitForOnce;
            entities.SaveChanges();
            return input;
        }
    }
}
