using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.Core.Algorithms;
using FLChat.Core.Routers;
using FLChat.DAL.Model;
using FLChat.Core;

namespace FLChat.VkWidget
{
    public class VKWidgetUpdateHandler : IVKWidgetUpdateHandler
    {

        public IReceiveUpdateStrategy<ChatEntities> NewMessageHandler { get; set; } = new NewMessageStrategy(
           RouterFactory.CreateDefaultRouters(new Core.IMessageRouter[] 
           {                
               new DeepLinkToSystemBotRouter()
           }),
           Factory.CreateDeepLinkStrategy(),  listener: new GreetingMessageListener())
        {
            EnableTransportOnIncommingMessage = true
        };

        public object MakeUpdate(ChatEntities entities, VkWidgetCallbackData callbackData)
        {
            NewMessageHandler?.Process(entities, new VKWidgetAdapter(callbackData));
            return null;
        }
         
    }
}
