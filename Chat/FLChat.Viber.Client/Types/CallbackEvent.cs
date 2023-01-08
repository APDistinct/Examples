using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Viber.Client.Types
{
    public enum CallbackEvent
    {
        [EnumMember(Value = "subscribed")]
        Subscribed,

        [EnumMember(Value = "unsubscribed")]
        Unsubscribed,

        [EnumMember(Value = "conversation_started")]
        ConversationStarted,

        [EnumMember(Value = "delivered")]
        Delivered,

        [EnumMember(Value = "seen")]
        Seen,

        [EnumMember(Value = "failed")]
        Failed,

        [EnumMember(Value = "message")]
        Message,

        [EnumMember(Value = "webhook")]
        Webhook
    }
}
