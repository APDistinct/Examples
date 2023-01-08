using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.VKBotClient.Types.Enums
{
    public enum CallbackEvent
    {
        [EnumMember(Value = "confirmation")]
        Confirmation,   

        [EnumMember(Value = "message_new")]
        Message,

        [EnumMember(Value = "message_allow")]
        Subscribed,

        [EnumMember(Value = "message_deny")]
        Unsubscribed,
    }
}
