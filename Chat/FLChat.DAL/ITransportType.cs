using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL
{
    public enum TransportKind
    {
        FLChat = 0,
        Telegram = 1,
        WhatsApp = 2,
        Viber = 3,
        VK = 4,
        OK = 5,
    }

    public interface ITransportType
    {
        TransportKind Kind { get; set; }
    }
}
