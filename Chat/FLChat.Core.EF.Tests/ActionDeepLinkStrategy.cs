using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.Core
{
    public class ActionDeepLinkStrategy : IDeepLinkStrategy
    {
        public bool Result { get; set; } = false;
        public User User { get; set; } = null;
        public object Context { get; } = new object();
        public int CountOfCalls { get; private set; } = 0;

        public bool AcceptDeepLink(ChatEntities entities, IDeepLinkData message, out User user, out Message answerTo, out object context, out IDeepLinkStrategy sender) {
            CountOfCalls += 1;
            if (Result) {
                user = User;
                answerTo = null;
                context = Context;
                sender = this;
                return true;
            } else {
                user = null;
                answerTo = null;
                context = null;
                sender = null;
                return false;
            }
        }

        public void AfterAddTransport(ChatEntities entities, IDeepLinkData message, Transport transport, object context) {
        }
    }
}
