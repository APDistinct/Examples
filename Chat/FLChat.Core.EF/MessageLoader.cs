using FLChat.DAL;
using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core
{
    public class MessageLoader : IMessageLoader
    {
        private readonly TransportKind _transport;

        public MessageLoader(TransportKind transport)
        {
            _transport = transport;
        }

        public MessageToUser[] Load(ChatEntities entities)
        {
            return entities.MessageToUser
                        .Where(mt =>
                               mt.ToTransportTypeId == (int)_transport
                            && mt.IsSent == false
                            && mt.IsFailed == false
                            && mt.Message.IsDeleted == false
                            && mt.Message.DalayedCancelled == null
                            && (mt.Message.DelayedStart == null || mt.Message.DelayedStart != null && mt.Message.DelayedStart <= DateTime.UtcNow ))
                        .Include(mt => mt.Message)
                        .Include(mt => mt.ToTransport)
                        .Include(mt => mt.ToTransport.User)
                        .Include(mt => mt.Message.FromTransport)
                        .Include(mt => mt.Message.FromTransport.User)
                        .ToArray();
        }
    }
}
