using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.VKBotClient.Callback
{
    public enum ResponseMessageType
    {
        None,
        MessageNew
    }

    public static class MessageTypeHelper
    {
        public static ResponseMessageType GetType(string name)
        {
            switch (name)
            {
                case "message_new":
                {
                    return ResponseMessageType.MessageNew;
                }

                default:
                    return ResponseMessageType.None;
            }
        }
    }
}
