using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;

namespace FLChat.Core
{
    public class FakeDeepLinkData : IDeepLinkData
    {
        public string FromId { get; set; }

        public string DeepLink { get; set; }

        public bool IsTransportEnabled { get; set; }

        public TransportKind TransportKind => TransportKind.Test;
    }
}
