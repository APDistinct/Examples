using FLChat.Core;
using FLChat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.DataTypes
{
    public class FakeLinkData : IDeepLinkData
    {
        public FakeLinkData(string code)
        {
            DeepLink = code;
        }

        public string FromId => "";

        public string DeepLink { get; }

        public bool IsTransportEnabled => true;

        public TransportKind TransportKind => TransportKind.Test;
    }
}
