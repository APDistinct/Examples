using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using FLChat.Core;
using FLChat.Core.Algorithms;
using FLChat.Core.Routers;
using FLChat.TelegramBot.Algorithms;
using FLChat.TelegramBot.Adapters;
using System.Configuration;

namespace FLChat.TelegramBot
{
    public class TelegramUpdateHandler : ITelegramUpdateHandler
    {
        public IReceiveUpdateStrategy<ChatEntities> NewMessageHandler { get; set; }
            = new NewMessageStrategy(MakeRouters(),
                //RouterFactory.CreateDefaultRouters(
                //new Core.IMessageRouter[] {
                //    new StartRouter(),
                //    new InviteLinkRouter(),
                //    new DeepLinkToSystemBotRouter(),
                //    new TgBotCommandsRouter(),
                //    new CommonLinkRouter(),
                //new SelectAddresseeRouter(),
                //new AskPhoneRouter()    }),
                Factory.CreateDeepLinkStrategy(),
                listener: new GreetingMessageListener(),
                uploadFile: CreateFileLoader(),
                avatarProvider: new TelegramAvatarProvider(CreaTelegramClient()));

        public ICallbackQueryStrategy<ChatEntities> CallbackQueryHandler { get; set; } =
            new CallbackStrategy(new CallbackSelectAddressee());
        

        public void MakeUpdate(ChatEntities entities, Update update) {
            switch (update.Type) {
                case UpdateType.Message:
                    NewMessageHandler?.Process(entities, new TelegramMessageAdapter(update.Message));
                    break;

                case UpdateType.CallbackQuery:
                    CallbackQueryHandler?.Process(entities, new TelegramCallbackDataAdapter(update.CallbackQuery));
                    break;
            }
        }

        private static IFileLoader CreateFileLoader() {
            string token = ConfigurationManager.AppSettings["tg_token"] ?? throw new ConfigurationErrorsException("Configuration value for telegram token must be present");
            string proxy = ConfigurationManager.AppSettings["tg_proxy_addr"];
            if (!int.TryParse(ConfigurationManager.AppSettings["tg_proxy_port"] ?? "0", out int port))
                throw new ConfigurationErrorsException("Configuration value for proxy port is invalid");
            string usr = ConfigurationManager.AppSettings["tg_proxy_user"];
            string psw = ConfigurationManager.AppSettings["tg_proxy_password"];
            TelegramFileLoader loader = new TelegramFileLoader(token, proxy, port, usr, psw);            
            return loader;
        }
        
        private static TelegramClient CreaTelegramClient() {
            string token = ConfigurationManager.AppSettings["tg_token"] ?? throw new ConfigurationErrorsException("Configuration value for telegram token must be present");
            string proxy = ConfigurationManager.AppSettings["tg_proxy_addr"];
            if (!int.TryParse(ConfigurationManager.AppSettings["tg_proxy_port"] ?? "0", out int port))
                throw new ConfigurationErrorsException("Configuration value for proxy port is invalid");
            string usr = ConfigurationManager.AppSettings["tg_proxy_user"];
            string psw = ConfigurationManager.AppSettings["tg_proxy_password"];
            TelegramClient loader = new TelegramClient(token, proxy, port, usr, psw);            
            return loader;
        }

        private static Core.IMessageRouter MakeRouters()
        //private static Core.IMessageRouter[] MakeRouters()
        {
            List<Core.IMessageRouter> messageRouters = new List<IMessageRouter>();
            messageRouters.Add(new StartRouter());
            if (Settings.IsInviteLinkWork)
            {
                messageRouters.Add(new InviteLinkRouter());
            }            
            messageRouters.Add(new TgBotCommandsRouter());
            if (Settings.IsCommonLinkWork)
            {
                messageRouters.Add(new CommonLinkRouter());
            }
            messageRouters.Add(new DeepLinkToSystemBotRouter());
            return RouterFactory.CreateDefaultRouters(messageRouters.ToArray());
        }
    }
}
