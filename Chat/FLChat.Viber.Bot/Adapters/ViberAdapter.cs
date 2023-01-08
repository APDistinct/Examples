using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL;
using FLChat.Viber.Client.Types;

namespace FLChat.Viber.Bot.Adapters
{
    public class ViberAdapter
    {
        public ViberAdapter(CallbackData callback) {
            Callback = callback;
        }

        public CallbackData Callback { get; }

        public TransportKind TransportKind => TransportKind.Viber;
    }
}
