using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.Core;
using FLChat.Core.Algorithms;
using FLChat.DAL.Model;
using FLChat.DAL;
using FLChat.Viber.Bot.Adapters;

namespace FLChat.Viber.Bot.Algorithms
{
    /// <summary>
    /// Deep link strategy for viber do:
    ///     1. Clear id in all disabled transports with same id
    ///     2. Create new transport in disabled state if Viber's user has't subscribed on viber channel
    /// </summary>
    public class DeepLinkStrategy : NewMessageStrategy.IListener
    {
        public void BeforeAddTransport(ChatEntities entities, IDeepLinkData message, User user) {
            Transport[] forDelete = entities
                .Transport
                .Where(t => t.Enabled == false && t.TransportTypeId == (int)TransportKind.Viber && t.TransportOuterId == message.FromId)
                .ToArray();
            foreach (Transport transport in forDelete)
                transport.TransportOuterId = String.Empty;
            if (forDelete.Length > 0)
                entities.SaveChanges();
        }

        public void DeepLinkAccepted(ChatEntities entities, IOuterMessage message, DeepLinkResult dlResult, Transport transport) {
            //throw new NotImplementedException();
        }

        public void NewUserCreated(ChatEntities entities, IOuterMessage message, Transport transport) {
            //throw new NotImplementedException();
        }
    }
}
