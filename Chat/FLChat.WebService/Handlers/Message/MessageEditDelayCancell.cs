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
    public class MessageEditDelayCancell
    {
        private readonly IMessageEditTimeChecker _timeChecker;

        public MessageEditDelayCancell(IMessageEditTimeChecker timeChecker = null)
        {
            _timeChecker = timeChecker ?? new MessageEditTimeChecker("MESSAGE_DELAY_TIMEOUT");
        }

        public bool Perform(ChatEntities entities, DAL.Model.Message msg, MessageEditInfo input)
        {
            bool ret = true;
            if (input.Cancelled.Value == true)  //  1.1
            {
                if (msg.DelayedStart != null)   //  1.1.1
                {
                    if (_timeChecker.Delay(msg.DelayedStart.Value))    //  1.1.1.1
                    {
                        msg.DalayedCancelled = DateTime.UtcNow;
                        entities.SaveChanges();
                    }
                    else  //  1.1.1.2
                    {
                        throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, $"Can't cancell message with id {input.Ids} - timeout");
                    }
                }
                else  //  1.1.2
                {
                    // Попытка отменить НЕотложенное
                    throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, $"Can't cancell message with id {input.Ids} - not in delayed state");
                }
            }
            else  //  1.2
            {
                // Попытка вернуть отложенное
                if (msg.DelayedStart != null)
                {
                    // Попытка вернуть обратно отложенное
                    throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, $"Message with id {input.Ids} is in delayed state");
                }
            }
            return ret;
        }
    }
}
