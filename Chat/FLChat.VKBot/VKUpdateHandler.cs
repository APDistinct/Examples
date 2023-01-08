using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

using FLChat.Core.Algorithms;
using FLChat.Core.Routers;
using FLChat.VKBot.Adapters;
using FLChat.VKBotClient.Types;
using FLChat.VKBot.Routers;
using FLChat.VKBotClient.Types.Enums;
using Newtonsoft.Json.Linq;
using Message = FLChat.VKBotClient.Types.Message;
using FLChat.Core;
using FLChat.Logger;

//using FLChat.TelegramBot.Algotithms;


namespace FLChat.VKBot
{
    public class VKUpdateHandler : IVKUpdateHandler
    {

        public IReceiveUpdateStrategy<ChatEntities> NewMessageHandler { get; set; }

        public ISubscribeStrategy<ChatEntities> SubscribeHandler { get; set; }
            = new SubscribeStrategy();

        public IUnsubscribeStrategy<ChatEntities> UnsubscribeHandler { get; set; }
            = new UnsubscribeStrategy();

        private FileLogger _logger;

        public VKUpdateHandler() : this(null)
        {            
        }

        public VKUpdateHandler(IMessageRouter router)
        {
            string fname = "C:\\FLChat\\Log.txt";
            _logger = new FileLogger(fname);
            _logger.Log($"Start VKUpdateHandler");
            var tokenstring = ConfigurationManager.AppSettings["vk_token"];
            bool pri = !string.IsNullOrWhiteSpace(tokenstring);
            _logger.Log($"tokenstring " + tokenstring);

            List<IMessageRouter> invrouters = new List<IMessageRouter>();
            if (Settings.IsInviteLinkWork)
            {
                invrouters.Add(new InviteLinkRouter());
            }
            if (Settings.IsCommonLinkWork)
            {
                invrouters.Add(new CommonLinkRouter());
            }            

            //var invrouters = new Core.IMessageRouter[] { };
            //if(Settings.IsInviteLinkWork)
            //{
            //    _logger.Log($"Append InviteLinkRouter {Settings.IsInviteLinkWork.ToString()}");
            //    invrouters.Concat(Enumerable.Repeat(new InviteLinkRouter(), 1));
            //}
            //if (Settings.IsCommonLinkWork)
            //{
            //    _logger.Log($"Append CommonLinkRouter {Settings.IsInviteLinkWork.ToString()}");
            //    invrouters.Concat(Enumerable.Repeat(new CommonLinkRouter(), 1));
            //}

            _logger.Log($"Routers : {invrouters.Count().ToString()}");
            var routers = new Core.IMessageRouter[]
                {
                    new VKBotCommandsRouter(),
                    new VKInviteRouter(new ChainRouter(invrouters.ToArray()), pri ? new VKClient(tokenstring) : null),
                    //new DeepLinkToSystemBotRouter()
                };
            _logger.Log($"routers : {routers.Count().ToString()}");
            if (router != null)
            {
                routers.Concat(Enumerable.Repeat(router, 1));
            }
            NewMessageHandler = new NewMessageStrategy(
                RouterFactory.CreateDefaultRouters(routers), 
                Factory.CreateDeepLinkStrategy(),
                uploadFile: new FileLoaderByUrl(), 
                listener: new GreetingMessageListener(), 
                avatarProvider: new VKAvatarProvider(pri ? new VKClient(tokenstring) : null))
            {
                EnableTransportOnIncommingMessage = true,                
            };
        }

        public object MakeUpdate(ChatEntities entities, CallbackData callbackData)
        {
            switch (callbackData.Event)
            {
                case CallbackEvent.Confirmation:
                    return ConfigurationManager.AppSettings["VK_confirmation"] ?? "c30c7e45";

                case CallbackEvent.Message:
                    var message = ((JObject) callbackData.Object).ToObject<Message>();
                    NewMessageHandler?.Process(entities, new VKMessageAdapter(message));
                    break;

                case CallbackEvent.Subscribed:
                    var subscribeData = ((JObject) callbackData.Object).ToObject<SubscribeData>();
                    SubscribeHandler?.Process(entities, new VkSubscribeAdapter(subscribeData));
                    break;

                case CallbackEvent.Unsubscribed:
                    var unsubscribeData = ((JObject) callbackData.Object).ToObject<UnsubscribeData>();
                    UnsubscribeHandler?.Process(entities, new VkUnsubscribeAdapter(unsubscribeData));
                    break;
            }

            return null;
        }
    }
}
