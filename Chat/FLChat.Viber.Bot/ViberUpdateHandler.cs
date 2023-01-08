using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.Core.Algorithms;
using FLChat.Core.Routers;
using FLChat.DAL.Model;
using FLChat.Viber.Client.Types;
using FLChat.Viber.Bot.Adapters;
using FLChat.Viber.Bot.Algorithms;
using FLChat.Viber.Bot.Routers;
using FLChat.Core;

namespace FLChat.Viber.Bot
{
    public class ViberUpdateHandler : IViberUpdateHandler
    {
        /// <summary>
        /// Called for incoming message and conversation started with context events
        /// </summary>        
        public IReceiveUpdateStrategy<ChatEntities> NewMessageHandler { get; set; } = new NewMessageStrategy(
            MakeRouters(),
            //RouterFactory.CreateDefaultRouters(
            //    new Core.IMessageRouter[] {
            //    new InviteLinkRouter(),
            //    new CommonLinkRouter(),
            //    new ViberBotCommandsRouter(),
            //    new DeepLinkToSystemBotRouter(),
            //    new ButtonUrlMessageToSystemBot()    }            ),
            Factory.CreateDeepLinkStrategy(),
            avatarProvider: new ViberAvatarProvider(),
            uploadFile: new FileLoaderByUrl(),
            listener: new DeepLinkStrategy()) {
            EnableTransportOnIncommingMessage = true,
            ProcessDeepLinkForActiveUser = true,
        };

        /// <summary>
        /// called for Failed, Delivered and Seen events
        /// </summary>
        public IMessageStatusChangedStrategy<ChatEntities> ChangeMessageStatusHandler { get; set; }
            = new MessageStatusChangedStrategy();

        /// <summary>
        /// Called for ConversationStarted event without DeepLink
        /// </summary>
        public IConversationStartedStrategy ConversationStartedHandler { get; set; }
            = new ConversationStartedStrategy(new ViberGreetingMessageTexts());

        /// <summary>
        /// Call for subscribe event
        /// </summary>
        public ISubscribeStrategy<ChatEntities> SubscribeHandler { get; set; } 
            = new SubscribeStrategy();

        /// <summary>
        /// Call for unsubscribe event
        /// </summary>
        public IUnsubscribeStrategy<ChatEntities> UnsubscribeHandler { get; set; }
            = new UnsubscribeStrategy();

        /// <summary>
        /// Treat callback updates
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        public object MakeUpdate(ChatEntities entities, CallbackData callbackData) {
            switch (callbackData.Event) {

                case CallbackEvent.Message:
                    NewMessageHandler.Process(entities, new ViberMessageAdapter(callbackData));
                    break;

                case CallbackEvent.Delivered:
                case CallbackEvent.Failed:
                case CallbackEvent.Seen:
                    ChangeMessageStatusHandler.Process(entities, new ViberMessageStatusAdapter(callbackData));
                    break;

                case CallbackEvent.ConversationStarted when callbackData.Context != null:
                    NewMessageHandler.Process(entities, new ViberConversationStartedAdapter(callbackData), out Core.DeepLinkResult dlResult);
                    return ConversationStartedHandler.Process(entities, callbackData, dlResult);

                case CallbackEvent.ConversationStarted when callbackData.Context == null:
                    return ConversationStartedHandler.Process(entities, callbackData, null);

                case CallbackEvent.Subscribed:
                    SubscribeHandler.Process(entities, new ViberSubscribeAdapter(callbackData));
                    break;

                case CallbackEvent.Unsubscribed:
                    UnsubscribeHandler.Process(entities, new ViberUnsubscribeAdapter(callbackData));
                    break;
            }

            return null;
        }

        private static Core.IMessageRouter MakeRouters()
        //private static Core.IMessageRouter[] MakeRouters()
        {
            List<Core.IMessageRouter> messageRouters = new List<IMessageRouter>();
            if (Settings.IsInviteLinkWork)
            {
                messageRouters.Add(new InviteLinkRouter());
            }
            if (Settings.IsCommonLinkWork)
            {
                messageRouters.Add(new CommonLinkRouter());
            }
            messageRouters.Add(new ViberBotCommandsRouter());
            messageRouters.Add(new DeepLinkToSystemBotRouter());
            messageRouters.Add(new ButtonUrlMessageToSystemBot());
            return RouterFactory.CreateDefaultRouters(messageRouters.ToArray());
        }


    }
}
