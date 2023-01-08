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
    public class VkSubscribeAdapter : ISubscribeData
    {
        public SubscribeData SubscribeData { get; }

        public VkSubscribeAdapter(SubscribeData subscribeData)
        {
            SubscribeData = subscribeData;
        }

        public TransportKind TransportKind => TransportKind.VK;
        public string UserId => SubscribeData.UserId;
        public string UserName { get; }
    }
}
