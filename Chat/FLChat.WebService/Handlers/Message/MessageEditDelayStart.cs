using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.Handlers.Message
{
    public class MessageEditDelayStart
    {
        private readonly IMessageEditTimeChecker _timeChecker;

        public MessageEditDelayStart(IMessageEditTimeChecker timeChecker = null)
        {
            _timeChecker = timeChecker ?? new MessageEditTimeChecker("MESSAGE_DELAY_TIMEOUT");
        }

        public bool Perform(ChatEntities entities, DAL.Model.Message msg, MessageEditInfo input)
        {
            bool ret = true;
            if (msg.DalayedCancelled != null)  // 
            {
                throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, $"Can't change DelayedStart for message with id {input.Ids} - Cancelled");
            }

            if (msg.DelayedStart != null)  // 2.1.1
            {
                if (_timeChecker.Delay(msg.DelayedStart.Value))  // 2.1.1.1
                {
                    msg.DelayedStart = input.DelayedStart;
                    entities.SaveChanges();
                }
                else // 2.1.1.2
                    throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, $"Can't change DelayedStart for message with id {input.Ids} - timeout");
            }
            else  // 2.1.2
            {
                throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, $"Can't change DelayedStart for message with id {input.Ids} - not in delayed state");
            }

            return ret;
        }
    }
}
