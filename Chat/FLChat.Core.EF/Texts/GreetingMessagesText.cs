using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core.Texts
{
    public class GreetingMessagesText : IGreetingMessagesText
    {
        public string LiteLinkKnownUser => Settings.Values.GetValue("TEXT_LITELINK_GREETING_MSG",
                Settings.Values.GetValue("TEXT_GREETING_MSG", null));

        public string LiteLinkRouted => Settings.Values.GetValue("TEXT_LITELINK_ROUTED", null);

        public string LiteLinkUnrouted => Settings.Values.GetValue("TEXT_LITELINK_UNROUTED", null);

        public string LiteLinkUnknownUser => LinkRejectedOrUnknown;

        public string LinkRejectedOrUnknown => Settings.Values.GetValue("TEXT_DEEPLINK_GREETING_MSG_REJECTED", null);
    }
}
