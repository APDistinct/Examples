using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.Core;
using FLChat.DAL;
using FLChat.VKBotClient.Types;

namespace FLChat.VKBot.Adapters
{
    class VkUnsubscribeAdapter : IUnsubscribeData
    {
        public UnsubscribeData UnsubscribeData { get; }

        public VkUnsubscribeAdapter(UnsubscribeData unsubscribeData)
        {
            UnsubscribeData = unsubscribeData;
        }

        public TransportKind TransportKind => TransportKind.VK;
        public string UserId => UnsubscribeData.UserId;
        public string UserName { get; }
    }
}
