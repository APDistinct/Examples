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
    public class ViberSubscribeAdapter : ViberAdapter, ISubscribeData
    {
        public ViberSubscribeAdapter(CallbackData callback) : base(callback) {
            if (callback.Event != CallbackEvent.Subscribed)
                throw new ViberAdapterException("Subscribe adapter works only with subscribe event type");
        }

        public string UserId => Callback.User.Id;

        public string UserName => Callback.User.Name;
    }
}
