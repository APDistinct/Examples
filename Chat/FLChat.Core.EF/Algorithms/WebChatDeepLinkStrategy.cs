using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;
using FLChat.DAL;

namespace FLChat.Core.Algorithms
{
    /// <summary>
    /// Process deep-links from web-chat
    /// </summary>
    public class WebChatDeepLinkStrategy : IDeepLinkStrategy
    {
        public class Context
        {
            public Context(WebChatDeepLink webChat) {
                WebChat = webChat;
                WebChatId = webChat.Id;
                MessageId = webChat.MsgId;
            }

            /// <summary>
            /// Web chat record id
            /// </summary>
            public long WebChatId { get; }

            /// <summary>
            /// Message id
            /// </summary>
            public Guid MessageId { get; }

            /// <summary>
            /// Web chat object
            /// </summary>
            public WebChatDeepLink WebChat { get; }

            public bool AcceptedEarly { get; internal set; } = false;
        }

        public bool AcceptDeepLink(ChatEntities entities,
            IDeepLinkData message,
            out User user,
            out Message answerTo,
            out object context,
            out IDeepLinkStrategy sender) {

            answerTo = null;
            WebChatDeepLink webchat = entities
                .WebChatDeepLink
                .Where(wc => wc.Link == message.DeepLink && wc.ExpireDate >= DateTime.UtcNow)
                .SingleOrDefault();

            if (webchat == null) {
                context = null;
                user = null;
                sender = null;
                return false;
            }
            context = new Context(webchat);
            sender = this;

            answerTo = webchat.MessageToUser.Message;

            //get deep link user
            user = entities.User.Where(u => u.Id == webchat.MessageToUser.ToUserId).Include(u => u.Transports).Single();

            bool accepted = webchat.AcceptedTransportKind.Contains(message.TransportKind);
            ((Context)context).AcceptedEarly = accepted;
            if (accepted) {
                Transport transport = user.Transports.Get(message.TransportKind);                
                if (transport != null && transport.TransportOuterId != message.FromId) {
                    //deep link was accepted yearly and by another messeger
                    user = null;
                    return true;
                }
            }

            return true;
        }

        public void AfterAddTransport(ChatEntities entities, IDeepLinkData message, Transport transport, object context) {
            long wcid = (context as Context).WebChatId;
            WebChatDeepLink webchat = entities
                .WebChatDeepLink
                .Where(wc => wc.Id == wcid)
                .Single();

            bool accepted = webchat.AcceptedTransportKind.Contains(message.TransportKind);
            if (!accepted) {

                //create event for user accepted deep link and attached new transport
                Event ev = new Event() {
                    Kind = EventKind.DeepLinkAccepted,
                    CausedByUserId = transport.User.Id,
                    CausedByUserTransportTypeId = transport.TransportTypeId,
                    ToUsers = new User[] {
                        webchat.MessageToUser.Message.FromTransport.User
                    }
                };
                entities.Event.Add(ev);

                //deep link was already accepted
                webchat.AcceptedTransportType.Add(entities.TransportType.Where(tt => tt.Id == (int)message.TransportKind).Single());

                //set sent invite user as new user's messages addressee
                transport.ChangeAddressee(entities, webchat.MessageToUser.Message.FromTransport.User);
            }
        }
    }
}
