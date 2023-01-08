using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;

namespace FLChat.Core
{
    public class FakeOuterMessageStatus : IOuterMessageStatus
    {
        public bool IsFailed { get; set; } = false;

        public bool IsDelivered { get; set; } = false;

        public bool IsRead { get; set; } = false;

        public string UserId { get; set; }

        public string MessageId { get; set; }

        public TransportKind TransportKind { get; set; } = TransportKind.Test;

        public string FailureReason { get; set; }
    }
}
