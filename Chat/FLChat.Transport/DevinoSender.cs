using FLChat.Core;
using FLChat.DAL.Model;
using FLChat.Devino;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Devino.Logger;
using Devino;

namespace FLChat.Transport
{
    abstract public class DevinoSender : IMessageSender
    {
        //protected int _transport;
        protected IDevinoProvider Sender { get; }
        protected abstract DAL.TransportKind transportKind { get; }

        protected DevinoSender(IDevinoProvider sender = null, DevinoSettings settings = null)
        {
            if(sender == null)
            {
                if(settings != null)
                {
                    Sender = new DevinoProvider(DevinoLogger.GetLogger(transportKind), settings);
                }
                else
                {
                    Sender = new DevinoProvider(DevinoLogger.GetLogger(transportKind));
                }
            }
            else
            {
                Sender = sender;
            }
        }

        abstract public string GetAddressee(MessageToUser mtu);
        abstract public Task<SentMessageInfo> Send(MessageToUser msg, string addressees, string text);
        
        public Task<SentMessageInfo> Send(MessageToUser msg, string msgText, CancellationToken ct)
        {
            string adr = GetAddressee(msg);
            if(adr == null) 
                 throw new ArgumentNullException($"User {msg.ToTransport.User.UserId} has no valid addressee");
            return Send(msg, adr, msgText);
        }
    }
}
