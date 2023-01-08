using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;

namespace FLChat.Core.Algorithms
{
    public class SubscribeStrategy : ISubscribeStrategy<ChatEntities>
    {
        public void Process(ChatEntities entities, ISubscribeData subscribeData) {
            //search transport for incoming message
            Transport fromTransport = entities
                .Transport
                .GetTransportByOuterId(
                    subscribeData.TransportKind,
                    subscribeData.UserId,
                    q => q.Include(t => t.User),
                    onlyEnabled: false);

            //don't create user on subscribe, if we don't know who is it.            
            if (fromTransport == null)
                return;

            if (fromTransport.Enabled == false) {
                fromTransport.Enabled = true;
                entities.SaveChanges();
            }
        }
    }
}
