using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.Core.Algorithms;
using FLChat.DAL.Model;

namespace FLChat.Core
{
    public class ActionRouter : IMessageRouter
    {
        private Func<ChatEntities, IOuterMessage, Message, Guid?> _action;

        public Guid? Addressee { get; }

        public ActionRouter(Func<ChatEntities, IOuterMessage, Message, Guid?> action) {
            _action = action;
            Addressee = null;
        }

        public ActionRouter(Guid? addressee) : this((ent, m, dbm) => addressee) {
            Addressee = addressee;
        }

        public Guid? RouteMessage(ChatEntities entities, IOuterMessage message, Message dbmsg) {
            return _action(entities, message, dbmsg);
        }
    }
}
