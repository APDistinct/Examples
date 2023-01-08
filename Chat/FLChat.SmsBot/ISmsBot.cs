using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.SmsBott
{
    public interface ISmsBot
    {
        User SmsBotUser { get; }
        Transport SmsBotTransport { get; }
        string GetAddressee(MessageToUser mtu);
        Message SendSmsMessage(string addressee, string text, bool change = false);
    }
}
