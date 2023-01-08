using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public class MessageEdit : IObjectedHandlerStrategy<MessageEditInfo, object>
    {
        public bool IsReusable => true;
        public static readonly string Key = typeof(MessageEditInfo).GetJsonPropertyName(nameof(MessageEditInfo.Ids));

        /// <summary>
        /// Can access only to himself sent messages
        /// </summary>
        public bool OnlySelfMessages { get; set; } = true;

        private readonly IMessageEditTimeChecker _timeChecker;

        public MessageEdit(IMessageEditTimeChecker timeChecker = null)
        {
            _timeChecker = timeChecker ?? new MessageEditTimeChecker();
        }

        public object ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, MessageEditInfo input)
        {
            if (!Guid.TryParse(input.Ids, out Guid msgId))
            {
                throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, $"Message id {input.Ids} is incorrect");
            }
            //load message
            DAL.Model.Message msg = entities
                .Message
                .Where(m => m.Id == msgId)
                //.Include(m => m.FileInfo)
                //.Include(m => m.FileInfo.MediaType)
                .SingleOrDefault();

            //if not found
            if (msg == null)
                throw new ErrorResponseException(HttpStatusCode.NotFound, ErrorResponse.Kind.not_found, $"Message with id {msgId.ToString()} has not found");

            //check access to messages sent by another user
            if (OnlySelfMessages && currUserInfo.UserId != msg.FromUserId)
                throw new ErrorResponseException(HttpStatusCode.NotFound, ErrorResponse.Kind.not_found, $"Message with id {msgId.ToString()} has not found");

            //  Cancelled присутствует в посылке
            if (input.Cancelled.HasValue)  // 1.
            {
                return (new MessageEditDelayCancell()).Perform(entities, msg, input);
            }

            //  DelayedStart присутствует в посылке
            if (input.DelayedStart != null)  // 2
            {
                return (new MessageEditDelayStart()).Perform(entities, msg, input);
            }

            return null;
        }
    }
}
