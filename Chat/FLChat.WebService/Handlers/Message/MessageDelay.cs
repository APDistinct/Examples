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
    public class MessageDelay : IObjectedHandlerStrategy<MessageEditInfo, MessageEditInfo>
    {
        public bool IsReusable => true;
        public static readonly string Key = typeof(MessageEditInfo).GetJsonPropertyName(nameof(MessageEditInfo.Ids));

        /// <summary>
        /// Can access only to himself sent messages
        /// </summary>
        public bool OnlySelfMessages { get; set; } = true;

        private readonly IMessageDelayTimeChecker _timeChecker;

        public MessageDelay(IMessageDelayTimeChecker timeChecker = null)
        {
            _timeChecker = timeChecker ?? new MessageDelayTimeChecker();
        }

        public MessageEditInfo ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, MessageEditInfo input)
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
                if (input.Cancelled.Value == true)  //  1.1
                {
                    if (msg.DelayedStart != null)   //  1.1.1
                    {
                        if (_timeChecker.DelayCheck(msg.DelayedStart.Value))    //  1.1.1.1
                        {
                            msg.DalayedCancelled = DateTime.UtcNow;
                            entities.SaveChanges();
                        }
                        else  //  1.1.1.2
                        {
                            if (input.DelayedStart == null)  //  1.1.1.2.1
                            {
                                throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, $"Can't cancell message with id {msgId.ToString()} - timeout");
                            }
                        }
                    }
                    else  //  1.1.2
                    {
                        if (input.DelayedStart == null)  //  1.1.2.1
                        {
                            // Попытка отменить НЕотложенное
                            throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, $"Can't cancell message with id {msgId.ToString()} - not in delayed state");
                        }
                    }
                }
                else  //  1.2
                {
                    if (msg.DelayedStart != null && input.DelayedStart == null)
                    {
                        // Попытка вернуть обратно отложенное
                        throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, $"Message with id {msgId.ToString()} is in delayed state");
                    }
                }
            }
            else  // 2
            {
                //  Cancelled отсутствует в посылке
                if (input.DelayedStart != null)  //  2.1
                {
                    if (msg.DelayedStart != null)  // 2.1.1
                    {
                        if (_timeChecker.DelayCheck(msg.DelayedStart.Value))  // 2.1.1.1
                        {
                            msg.DelayedStart = input.DelayedStart;
                            entities.SaveChanges();
                        }
                        else // 2.1.1.2
                            throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, $"Can't change DelayedStart for message with id {msgId.ToString()} - timeout");
                    }
                    else  // 2.1.2
                    {
                        throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, $"Can't change DelayedStart for message with id {msgId.ToString()} - not in delayed state");
                    }
                }
            }
            input.DelayedStart = msg.DelayedStart;
            input.Cancelled = msg.DalayedCancelled != null;

            return input;
        }        
    }
}
