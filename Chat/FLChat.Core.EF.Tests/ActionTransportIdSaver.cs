using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.Core
{
    public class ActionTransportIdSaver : ITransportIdSaver
    {
        private Action<ChatEntities, string, Message> _action;
        private Action<ChatEntities, string, MessageToUser> _actionTo;

        public ActionTransportIdSaver(
            Action<ChatEntities, string, Message> action = null,
            Action<ChatEntities, string, MessageToUser> actionTo = null) {
            _action = action;
            _actionTo = actionTo;
        }

        public void SaveFrom(ChatEntities entities, string id, Message dbmsg) {
            _action?.Invoke(entities, id, dbmsg);
        }

        public void SaveTo(ChatEntities entities, string id, MessageToUser msg) {
            _actionTo?.Invoke(entities, id, msg);
        }

        public void SaveTo(ChatEntities entities, string[] ids, MessageToUser msg) {
            throw new NotImplementedException();
        }
    }
}
