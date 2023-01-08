using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core.Texts
{
    public class FakeGreetingMessages : Texts.IGreetingMessagesText
    {
        public string LiteLinkKnownUser { get; set; } = "LiteLinkKnownUser";

        public string LiteLinkRouted { get; set; } = "LiteLinkRouted";

        public string LiteLinkUnrouted { get; set; } = "LiteLinkUnrouted";

        public string LiteLinkUnknownUser { get; set; } = "LiteLinkUnknownUser";

        public string LinkRejectedOrUnknown { get; set; } = "LinkRejectedOrUnknown";
    }
}
