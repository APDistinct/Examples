using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.Core;
using FLChat.Viber.Client.Types;
using FLChat.Viber.Bot.Exceptions;

namespace FLChat.Viber.Bot.Adapters
{
    public class ViberUnsubscribeAdapter : ViberAdapter, IUnsubscribeData
    {
        public ViberUnsubscribeAdapter(CallbackData callback) : base(callback) {
            if (callback.Event != CallbackEvent.Unsubscribed)
                throw new ViberAdapterException("Unsubscribe adapter works only with unsubscribe event type");
        }

        public string UserId => Callback.UserId;
    }
}
