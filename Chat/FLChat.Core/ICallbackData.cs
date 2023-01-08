using FLChat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core
{
    public interface ICallbackData : ICallbackBase
    {
        string Id { get; }

        /// <summary>
        /// Callback on message Id
        /// </summary>
        string FromMessageId { get; }

        /// <summary>
        /// Callback from user id
        /// </summary>
        string FromUserId { get; }

        /// <summary>
        /// Callback data
        /// </summary>
        string Data { get; }
    }

    public static class ICallbackDataExtentions
    {
        public const string ADDRESSEE_SWITCH = "switch:";

        public static bool IsChangeMsgAddressee(this ICallbackData callbackData) => callbackData.Data.StartsWith(ADDRESSEE_SWITCH);

        public static Guid? MsgAddressee(this ICallbackData callbackData) {
            if (callbackData.Data.Length == ADDRESSEE_SWITCH.Length)
                return null;
            else
                return Guid.Parse(callbackData.Data.Substring(ADDRESSEE_SWITCH.Length));
        }
    }
}
