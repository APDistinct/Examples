using Devino;
using Devino.Viber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Devino
{
    public interface IDevinoProvider
    {
        Task<SentAllMessageInfo> SmsSend(string destinationAddresses, string data, string subject = null, DateTime? sendDate = null, int validity = 0);
        Task<SentAllMessageInfo> EmailSend(string destinationAddresses, string data, string subject = null, DateTime? sendDate = null, int validity = 0);
        Task<SentAllMessageInfo> ViberSend(ViberMessageInfo vmInfo);
    }
}
