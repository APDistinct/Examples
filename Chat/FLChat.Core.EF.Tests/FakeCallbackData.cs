using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;

namespace FLChat.Core
{
    public class FakeCallbackData : ICallbackData
    {
        public TransportKind TransportKind { get; set; } = TransportKind.Test;

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string FromMessageId { get; set; }

        public string FromUserId { get; set; }

        public string Data { get; set; } = Guid.NewGuid().ToString();
    }
}
