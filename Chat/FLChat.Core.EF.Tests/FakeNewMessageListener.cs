using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.Core.Algorithms;
using FLChat.DAL.Model;

namespace FLChat.Core
{
    public class FakeNewMessageListener : NewMessageStrategy.IListener
    {
        public DeepLinkResult DeepLinkResult { get; private set; }
        public Transport DeepLinkTransport { get; private set; }

        public void BeforeAddTransport(ChatEntities entities, IDeepLinkData message, User user) {
            
        }

        public void DeepLinkAccepted(ChatEntities entities, IOuterMessage message, DeepLinkResult dlResult, Transport transport) {
            DeepLinkResult = dlResult;
            DeepLinkTransport = transport;
        }

        public void NewUserCreated(ChatEntities entities, IOuterMessage message, Transport transport) {
            
        }
    }
}
