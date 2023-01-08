using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core
{
    public class FakeOuterMessage : IOuterMessage
    {
        public string MessageId { get; set; } = Guid.NewGuid().ToString();

        public string FromId { get; set; } 

        public string FromName { get; set; }

        public DAL.TransportKind TransportKind { get; set; } = DAL.TransportKind.Test;

        public string Text { get; set; } = "fake outer message text";

        public string PhoneNumber { get; set; }
        
        public string AvatarUrl { get; }

        public string DeepLink { get; set; }

        public string ReplyToMessageId { get; set; }

        public bool IsTransportEnabled { get; set; } = true;

        public IInputFile File { get; set; }
    }
}
