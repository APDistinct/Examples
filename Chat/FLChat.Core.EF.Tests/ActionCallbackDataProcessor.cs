using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.Core
{
    class ActionCallbackDataProcessor : ICallbackDataProcessor
    {
        private Action<ChatEntities, Transport, ICallbackData> _action;

        public ActionCallbackDataProcessor(Action<ChatEntities, Transport, ICallbackData> action) {
            _action = action;
        }

        public void Process(ChatEntities entities, Transport transport, ICallbackData callbackData) {
            _action(entities, transport, callbackData);
        }
    }
}
