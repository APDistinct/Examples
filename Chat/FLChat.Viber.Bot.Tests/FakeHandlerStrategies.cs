using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;
using FLChat.Core.Algorithms;
using FLChat.Core;
using FLChat.Viber.Client.Types;
using FLChat.Viber.Bot.Algorithms;
using FLChat.Viber.Client.Requests;

namespace FLChat.Viber.Bot.Tests
{
    public abstract class BaseAction<T>
    {
        private readonly Action<T> _action;

        public BaseAction(Action<T> action) {
            _action = action;
        }

        public void Process(ChatEntities db, T message) {
            if (_action != null)
                _action.Invoke(message);
            else
                throw new NotImplementedException();
        }
    }

    public class NewMessageAction : BaseAction<IOuterMessage>, IReceiveUpdateStrategy<ChatEntities>
    {
        public NewMessageAction(Action<IOuterMessage> action = null) : base(action) {}

        public void Process(ChatEntities db, IOuterMessage message, out DeepLinkResult deepLinkResult) {
            deepLinkResult = null;
            Process(db, message);
        }
    }

    public class MessageStatusChangedAction : BaseAction<IOuterMessageStatus>, IMessageStatusChangedStrategy<ChatEntities>
    {
        public MessageStatusChangedAction(Action<IOuterMessageStatus> action = null) : base(action) {}
    }

    public class ConversationStartedAction : IConversationStartedStrategy
    {
        private readonly Func<CallbackData, SendMessageRequest> _action;

        public ConversationStartedAction(Func<CallbackData, SendMessageRequest> action = null) {
            _action = action;
        }

        public SendMessageRequest Process(ChatEntities db, CallbackData message, DeepLinkResult deepLinkResult) {
            if (_action != null)
                return _action.Invoke(message);
            else
                throw new NotImplementedException();
        }
    }

    public class SubscribeAction : BaseAction<ISubscribeData>, ISubscribeStrategy<ChatEntities>
    {
        public SubscribeAction(Action<ISubscribeData> action = null) : base(action) {
        }
    }

    public class UnsubscribeAction : BaseAction<IUnsubscribeData>, IUnsubscribeStrategy<ChatEntities>
    {
        public UnsubscribeAction(Action<IUnsubscribeData> action = null) : base(action) {
        }
    }
}
