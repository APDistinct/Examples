using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL
{
    /// <summary>
    /// Represents values in table [Cfg].[EventType]
    /// </summary>
    public enum EventKind
    {
        Test = 0,

        MessageSent = 1,
        MessageDelivered = 2,
        MessageRead = 3,
        MessageDeleted = 4,
        MessageFailed = 5,

        MessageIncome = 10,

        AvatarChanged = 100,
        DeepLinkAccepted = 101
    }

    public interface IEvent
    {
        EventKind Kind { get; set; }

        TransportKind? CausedByTransportKind { get; set; }
    }

    public static class EventKindExtentions
    {
        /// <summary>
        /// Returns true if <paramref name="e"/> is event of message life cicle's type (sent, delivered, read, deleted, failed)
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool IsMessageLifeCicleEvent(this EventKind e) {
            return e >= EventKind.MessageSent && e <= EventKind.MessageRead || e == EventKind.MessageFailed;
        }
    }
}
