using FLChat.Core;
using FLChat.DAL;
using FLChat.VkWidget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.VkWidget
{
    public class VKWidgetAdapter : IOuterMessage
    {
        public VKWidgetAdapter(VkWidgetCallbackData message)
        {
            Message = message;
        }
        public VkWidgetCallbackData Message { get; }

        public string MessageId => Message.Id?.ToString() ?? Guid.NewGuid().ToString();

        public string FromId => Message.UserId.ToString();

        public string FromName => null;

        public string Text => $"user_id: {Message.UserId}; link: {Message.DeepLink}";

        public string PhoneNumber => null;
        public string AvatarUrl { get; }

        public string DeepLink => Message.DeepLink;

        public string ReplyToMessageId => null;

        public TransportKind TransportKind => TransportKind.VK;

        public bool IsTransportEnabled => true;

        public IInputFile File => null;
    }
}
