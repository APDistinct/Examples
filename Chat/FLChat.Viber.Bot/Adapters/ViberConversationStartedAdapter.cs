using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.Core;
using FLChat.DAL;
using FLChat.Viber.Client.Types;
using FLChat.Viber.Bot.Exceptions;

namespace FLChat.Viber.Bot.Adapters
{
    /// <summary>
    /// Adapt viber conversation started event to IOuterMessage
    /// </summary>
    public class ViberConversationStartedAdapter : ViberAdapter, IOuterMessage
    {
        public ViberConversationStartedAdapter(CallbackData message) : base(message) {
            if (message.Event != CallbackEvent.ConversationStarted || message.Context == null)
                throw new ViberAdapterException($"{nameof(ViberMessageAdapter)} works only with ConversationStarted event where context is not empty");
        }       

        public string FromId => Callback.User.Id;

        public string FromName => Callback.User.Name;

        public string Text => String.Concat("DeepLink ", Callback.Context);

        public string PhoneNumber => null;

        public string DeepLink => Callback.Context;

        public string AvatarUrl => Callback.User.Avatar;

        public string ReplyToMessageId => null;

        public string MessageId => Callback.MessageToken.ToString();

        public bool IsTransportEnabled => Callback.Subscribed ?? throw new NullReferenceException($"Viber message's firld 'subscribed' has null value");

        public IInputFile File => null;
    }
}
