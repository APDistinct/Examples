using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.Core;
using FLChat.DAL;
using FLChat.Viber.Bot.Exceptions;
using FLChat.Viber.Client.Types;

namespace FLChat.Viber.Bot.Adapters
{
    /// <summary>
    /// Adapt viber callback data of message event type
    /// </summary>
    public class ViberMessageAdapter : ViberAdapter, IOuterMessage
    {
        public ViberMessageAdapter(CallbackData message) : base(message) {
            if (message.Event != CallbackEvent.Message)
                throw new ViberAdapterException($"{nameof(ViberMessageAdapter)} works only with Message events");

            if (ViberInputFileAdapter.IsContainsFile(message))
                File = new ViberInputFileAdapter(message);
        }

        public string MessageId => Callback.MessageToken.ToString();

        public string FromId => Callback.Sender.Id;

        public string FromName => Callback.Sender.Name;        

        public string Text => Callback.Message.Text;

        public string PhoneNumber => Callback.Message.Contact?.PhoneNumber; //null;

        public string AvatarUrl => Callback.Sender.Avatar;

        public string DeepLink => null;

        public string ReplyToMessageId => null;

        public bool IsTransportEnabled => Callback.Subscribed ?? throw new NullReferenceException($"Viber message's firld 'subscribed' has null value");

        public IInputFile File { get; }
    }
}
