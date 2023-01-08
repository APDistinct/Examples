using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;

namespace FLChat.Core.Algorithms
{
    public class UnsubscribeStrategy : IUnsubscribeStrategy<ChatEntities>
    {
        public void Process(ChatEntities entities, IUnsubscribeData unsubscribeData) {
            //search transport for incoming message
            Transport fromTransport = entities
                .Transport
                .GetTransportByOuterId(
                    unsubscribeData.TransportKind,
                    unsubscribeData.UserId,
                    q => q.Include(t => t.User),
                    onlyEnabled: true);

            //don't create user on subscribe, if we don't know who is it.            
            if (fromTransport == null)
                return;

            fromTransport.Enabled = false;
            entities.SaveChanges();
        }
    }
}
