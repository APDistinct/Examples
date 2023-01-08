using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;

namespace FLChat.Core
{
    public class FakeUnsubscribeData : IUnsubscribeData
    {
        public string UserId { get; set; }

        public TransportKind TransportKind { get; set; } = TransportKind.Test;
    }
}
