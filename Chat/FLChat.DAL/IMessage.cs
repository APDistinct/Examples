using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL
{
    public enum MessageKind
    {
        Personal = 0,
        Group = 1,
        Broadcast = 2,
        Mailing = 4,
        //Segment = 3
    }

    public interface IMessage : IMessageSpecific
    {
        MessageKind Kind { get; set; }
        TransportKind FromTransportKind { get; set; }
    }

    public static class MessageKindExtentions
    {
        public static string DefaultTransportViewName(this MessageKind kind) =>
            kind == MessageKind.Mailing ? "[Usr].[UserMailingTransportView]" : "[Usr].[UserDefaultTransportView]";
    }
}
