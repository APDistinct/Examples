using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.Core;
using FLChat.DAL;
using FLChat.Viber.Client.Types;
using FLChat.Viber.Bot.Exceptions;

namespace FLChat.Viber.Bot.Adapters
{
    public class ViberMessageStatusAdapter : ViberAdapter, IOuterMessageStatus
    {
        public ViberMessageStatusAdapter(CallbackData message) : base(message) {
            if (message.Event != CallbackEvent.Failed && message.Event != CallbackEvent.Delivered && message.Event != CallbackEvent.Seen)
                throw new ViberAdapterException($"{nameof(ViberAdapterException)} works only with change message statuses events");
        }

        public bool IsFailed => Callback.Event == CallbackEvent.Failed;

        public string FailureReason => Callback.Description;

        public bool IsDelivered => Callback.Event == CallbackEvent.Delivered;

        public bool IsRead => Callback.Event == CallbackEvent.Seen;

        public string UserId => Callback.UserId;

        public string MessageId => Callback.MessageToken.ToString();
    }
}
