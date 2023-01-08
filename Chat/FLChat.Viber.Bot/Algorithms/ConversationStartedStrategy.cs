using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;
using FLChat.Viber.Client.Types;
using FLChat.Viber.Client.Requests;
using FLChat.DAL;
using FLChat.Core;
using FLChat.Core.Texts;
using FLChat.Core.Algorithms;
using FLChat.Core.Buttons;
using FLChat.Viber.Client;

namespace FLChat.Viber.Bot.Algorithms
{
    public class ConversationStartedStrategy : IConversationStartedStrategy
    {
        static readonly IMessageTextCompiler _textCompiler = ViberFactory.CreateCompiler();
        private IGreetingMessagesText _texts;
        private readonly IScenarioStarter _scenarioStarter;
        private readonly IScenarioButtons _scenarioButtons;
        private readonly int MinApiVersion = ViberClient.DefaultMinApiVersion;
        private string messSender = "Sender";  //  Сделать настройку из БД, может конструктор

        public ConversationStartedStrategy(IGreetingMessagesText texts = null, IScenarioStarter scenarioStarter = null, IScenarioButtons scenarioButtons = null)
        {
            _texts = texts ?? new GreetingMessagesText();
            _scenarioStarter = scenarioStarter ?? new ScenarioStarter();
            _scenarioButtons = scenarioButtons ?? new ScenarioButtons();
        }

        public SendMessageRequest Process(ChatEntities entities, CallbackData callbackData, DeepLinkResult dlResult)
        {
            messSender = entities.SystemBot.FullName;
            //if deep link was processed
            if (dlResult != null) {
                switch (dlResult.Status) {
                    case DeepLinkResultStatus.Accepted when dlResult.Context is WebChatDeepLinkStrategy.Context wcc:
                        //send web-chat message
                        return MakeWebChatMessage(entities, wcc);

                    case DeepLinkResultStatus.Accepted when dlResult.Context is LiteDeepLinkStrategy.Context llc:
                        return LiteLinkAccepted(entities, SeekTransport(entities, callbackData), llc);

                    case DeepLinkResultStatus.Accepted when dlResult.Context is InviteLinkStrategy.Context ilc:
                        return InviteLinkAccepted(entities, SeekTransport(entities, callbackData), ilc);

                    case DeepLinkResultStatus.Accepted when dlResult.Context is CommonLinkStrategy.Context clc:
                        return CommonLinkAccepted(entities, SeekTransport(entities, callbackData), clc);

                    case DeepLinkResultStatus.AcceptedEarly 
                    when dlResult.Context is LiteDeepLinkStrategy.Context llc
                        && (callbackData?.Subscribed ?? false) == false:
                        return LiteLinkAccepted(entities, SeekTransport(entities, callbackData), llc);

                    case DeepLinkResultStatus.AcceptedEarly when dlResult.Context is WebChatDeepLinkStrategy.Context wcc:
                        if (callbackData.Subscribed == false)
                            return MakeWebChatMessage(entities, wcc);
                        else
                            return null;

                    case DeepLinkResultStatus.Accepted:
                        return MakeDefMessage(entities, SeekTransport(entities, callbackData));

                    case DeepLinkResultStatus.Rejected when dlResult.Context is LiteDeepLinkStrategy.Context llc:
                        return MakeMessage(entities, SeekTransport(entities, callbackData), _texts.LiteLinkUnknownUser);

                    case DeepLinkResultStatus.Rejected:
                    case DeepLinkResultStatus.Unknown:
                        return MakeMessage(entities, SeekTransport(entities, callbackData), _texts.LinkRejectedOrUnknown);
                }
            } else {
                if ((callbackData.Subscribed ?? false) == false) {                    
                    return MakeDefMessage(entities, SeekTransport(entities, callbackData), includeKeyboard: callbackData.Context != null);
                }
            }
            return null;
        }

        private Transport SeekTransport(ChatEntities entities, CallbackData callbackData) {
            return entities
               .Transport
               .Where(t => t.TransportTypeId == (int)TransportKind.Viber && t.TransportOuterId == callbackData.User.Id)
               .Include(t => t.User)
               .FirstOrDefault();
        }

        private SendMessageRequest MakeWebChatMessage(ChatEntities entities, WebChatDeepLinkStrategy.Context wcc) {
            MessageToUser msg = entities.WebChatDeepLink
                .Where(wc => wc.Id == wcc.WebChatId)
                .Select(wc => wc.MessageToUser)
                .Include(mtu => mtu.Message)
                .Include(mtu => mtu.Message.FromTransport)
                .Include(mtu => mtu.Message.FromTransport.User)
                .Include(mtu => mtu.ToTransport)
                .Include(mtu => mtu.ToTransport.User)
                .FirstOrDefault();
            string senderName = msg.Message.FromTransport.User.FullName.CutFullName(Sender.NAME_MAX_LENGTH);
            string text = _textCompiler.MakeText(msg);            
            if (msg.Message.FileId == null) {
                //message with text only
                return MakeMessage(msg, text, sender: senderName);
            } else {
                Keyboard kb = ViberSender.GetMainKeyboard(new TransportButtonsSourse(), msg);
                //message with file
                if (msg.Message.FileInfo.IsViberPicture()) {                    
                    //send with picture. Long text will cut
                    //TODO: send other message with full text
                    if (text != null && text.Length > SendPictureMessageRequest.MaxTextLength)
                        text = String.Concat(text.Substring(0, SendPictureMessageRequest.MaxTextLength - 3), "...");
                    return new SendPictureMessageRequest(
                        new Sender(senderName),
                        null,
                        text,
                        msg.Message.FileInfo.Url) { Keyboard = kb };
                } else {
                    //another file
                    if (String.IsNullOrEmpty(text) == false) {
                        //message with text. Will send only text
                        //TODO: send another message with file
                        return MakeMessage(msg, text, sender: senderName);
                    } else {
                        if (msg.Message.FileInfo.FileLength < SendFileMessageRequest.MaxFileSize) {
                            return new SendFileMessageRequest(
                                new Sender(senderName),
                                null,
                                msg.Message.FileInfo.Url,
                                msg.Message.FileInfo.FileLength,
                                msg.Message.FileInfo.FileName) { Keyboard = kb };
                        } else {
                            //send file as url
                            return new SendUrlMessageRequest(new Sender(senderName), null, msg.Message.FileInfo.Url) {
                                Keyboard = kb };
                        }
                    }
                }
            }
        }

        private SendMessageRequest LiteLinkAccepted(ChatEntities entities, Transport transport,
            LiteDeepLinkStrategy.Context llc)
        {
            string text = _texts.LiteLinkKnownUser + "\r\n";
            if (llc.RouteTo != null)
                text = text + _texts.LiteLinkRouted.Replace(_texts.LiteLinkRouted_Addressee(), llc.RouteTo.FullName ?? "");
            else
                text = text + _texts.LiteLinkUnrouted;
            text = _textCompiler.MakeText(text, llc.User);
            return MakeMessage(entities, transport, text);
        }

        private SendMessageRequest InviteLinkAccepted(ChatEntities entities, Transport transport, InviteLinkStrategy.Context ilc)
        {
            string text = _scenarioStarter.StartScenario(entities, ScenarioType.Invite, ilc.UserId.Value, transport.TransportTypeId, out int? scS);
            MessageToUser mtu = CreateMessageToUser(entities, transport, null, text);
            //var kb = ViberSender.GetMainKeyboard(_scenarioButtons.GetButtons(ScenarioType.Invite, 0), mtu);
            var kb = ViberSender.GetMainKeyboard(_scenarioButtons.Adapt(ScenarioType.Invite, 0), null);

            return MakeMessage(entities, transport, text, kb /*ViberSender.GetSharePhoneKeyboard()*/);
        }

        private SendMessageRequest CommonLinkAccepted(ChatEntities entities, Transport transport, CommonLinkStrategy.Context clc)
        {
            string text = _scenarioStarter.StartScenario(entities, ScenarioType.Common, clc.UserId.Value, transport.TransportTypeId, out int? scS);
            MessageToUser mtu = CreateMessageToUser(entities, transport, null, text);
            //var kb = ViberSender.GetMainKeyboard(_scenarioButtons.GetButtons(ScenarioType.Invite, 0), mtu);
            var kb = ViberSender.GetMainKeyboard(_scenarioButtons.Adapt(ScenarioType.Invite, 0), null);

            return MakeMessage(entities, transport, text, kb /*ViberSender.GetSharePhoneKeyboard()*/);
        }

        private SendMessageRequest MakeMessage(ChatEntities entities, Transport transport, 
            string text, string sender = null, bool includeKeyboard = true)
        {
            return MakeMessage(entities, transport, text, null, sender, includeKeyboard);
                //new SendTextMessageRequest(new Sender(sender.CutFullName(Sender.NAME_MAX_LENGTH)), null, text) { Keyboard = kb };
        }

        private SendMessageRequest MakeMessage(ChatEntities entities, Transport transport,
            string text, Keyboard kb, string sender = null, bool includeKeyboard = true)
        {
            sender = sender ?? messSender;
            //Keyboard kb = null;
            if (kb == null && includeKeyboard)
            {
                MessageToUser mtu = CreateMessageToUser(entities, transport, null, text);
                kb = ViberSender.GetMainKeyboard(new TransportButtonsSourse(), mtu);
            }
            return new SendTextMessageRequest(new Sender(sender.CutFullName(Sender.NAME_MAX_LENGTH)), null, text) { Keyboard = kb, MinApiVersion = MinApiVersion, };
        }

        private SendMessageRequest MakeMessage(MessageToUser mtu, string text, string sender = null, bool includeKeyboard = true) {
            sender = sender ?? messSender;
            Keyboard kb = null;
            if (includeKeyboard)
                kb = ViberSender.GetMainKeyboard(new TransportButtonsSourse(), mtu);
            return new SendTextMessageRequest(new Sender(sender.CutFullName(Sender.NAME_MAX_LENGTH)), null, text) { Keyboard = kb, MinApiVersion = MinApiVersion, };

        }

        private SendMessageRequest MakeDefMessage(ChatEntities entities, Transport transport,
            string postfix = null, string sender = null, bool includeKeyboard = true) {
            sender = sender ?? messSender;
            Keyboard kb = null;
            string text = Settings.Values.GetValue("VIBER_WELCOME_MESSAGE", "Welcome to Faberlic");
            if (postfix != null)
                text = String.Concat(text, Environment.NewLine, postfix);
            if (includeKeyboard) {
                MessageToUser mtu = CreateMessageToUser(entities, transport, null, text);
                kb = ViberSender.GetMainKeyboard(new TransportButtonsSourse(), mtu);
            }
            return new SendTextMessageRequest(new Sender(sender.CutFullName(Sender.NAME_MAX_LENGTH)), null, text) { Keyboard = kb, MinApiVersion = MinApiVersion, };
        }

        private MessageToUser CreateMessageToUser(ChatEntities entities, Transport to, Transport from, string text) {
            if (to == null)
                return null;
            return new MessageToUser() {
                ToTransport = to,
                IsSent = true,
                Message = new DAL.Model.Message() {
                    Text = text,
                    FromTransport = from ?? entities.SystemBotTransport,
                    Specific = "VIBER_GREETING_MESSAGE"
                }
            };
        }
    }
}
