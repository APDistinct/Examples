using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.Core.Texts;
using FLChat.DAL.Model;

namespace FLChat.Viber.Bot.Algorithms
{
    class ViberGreetingMessageTexts : GreetingMessagesTextWrapper
    {
        public override string LiteLinkKnownUser => Settings.Values.GetValue("TEXT_LITELINK_GREETING_MSG_VIBER", null);
    }
}
