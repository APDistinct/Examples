using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;
using System.Data.Entity;
using FLChat.DAL;
using FLChat.Core.Texts;

namespace FLChat.Core.Algorithms
{
    public class GreetingMessageListener : NewMessageStrategy.IListener
    {
        public bool GreetingMessageOnAcceptedEarly { get; set; } = true;

        private readonly IGreetingMessagesText _texts;
        private readonly IScenarioStarter _scenarioStarter;

        public GreetingMessageListener(IGreetingMessagesText texts = null, IScenarioStarter scenarioStarter = null)
        {
            _texts = texts ?? new Texts.GreetingMessagesText();
            _scenarioStarter = scenarioStarter ?? new ScenarioStarter();
        }

        /// <summary>
        /// New arrived user was created
        /// </summary>
        /// <param name="entities">database</param>
        /// <param name="message">incoming message</param>
        /// <param name="dbmessage">database message record</param>
        /// <param name="transport">new created user transport</param>
        public void NewUserCreated(ChatEntities entities, IOuterMessage message, Transport transport) {
            SendDefaultMessage(entities, transport);
        }

        /// <summary>
        /// Result of accepting deep link
        /// </summary>
        /// <param name="entities">database</param>
        /// <param name="message">incoming message</param>
        /// <param name="dlResult">deep link accept result</param>
        /// <param name="transport">assigned transport</param>
        public void DeepLinkAccepted(ChatEntities entities, IOuterMessage message, DeepLinkResult dlResult, Transport transport) {
            switch (dlResult.Status) {
                case DeepLinkResultStatus.Accepted when dlResult.Context is WebChatDeepLinkStrategy.Context wcc:
                    WebChatMessage(entities, wcc, message);
                    break;

                case DeepLinkResultStatus.AcceptedEarly when GreetingMessageOnAcceptedEarly
                    && dlResult.Context is WebChatDeepLinkStrategy.Context wcc:
                    if (!wcc.AcceptedEarly)
                        WebChatMessage(entities, wcc, message);
                    break;

                case DeepLinkResultStatus.Accepted when dlResult.Context is LiteDeepLinkStrategy.Context llc:
                    LiteLinkMessage(entities, transport, llc);
                    break;
                case DeepLinkResultStatus.Accepted when dlResult.Context is InviteLinkStrategy.Context ilc:
                    InviteLinkMessage(entities, transport, ilc);
                    break;

                case DeepLinkResultStatus.Accepted when dlResult.Context is CommonLinkStrategy.Context clc:
                    CommonLinkMessage(entities, transport, clc);
                    break;

                case DeepLinkResultStatus.Accepted:
                    SendDefaultMessage(entities, transport);
                    break;

                case DeepLinkResultStatus.Rejected when dlResult.Context is LiteDeepLinkStrategy.Context llc:
                    SendMessage(entities, transport, _texts.LiteLinkUnknownUser, true);
                    break;
                
                case DeepLinkResultStatus.Rejected:
                case DeepLinkResultStatus.Unknown:
                    //SendMessage(entities, transport, Settings.Values.GetValue("TEXT_DEEP_LINK_REJECTED", "Код из ссылки отклонён"));
                    //SendDefaultMessage(entities, transport);
                    SendMessage(entities, transport, _texts.LinkRejectedOrUnknown, true);
                    break;
            }
        }

        private void WebChatMessage(ChatEntities entities, WebChatDeepLinkStrategy.Context wcc, IOuterMessage message) {
            long id = wcc.WebChatId;
            MessageToUser m = entities.WebChatDeepLink
                .Where(w => w.Id == id)
                .Select(w => w.MessageToUser)
                .Include(mtu => mtu.Message)
                .Single();
            m.Message.ToUsers.Add(new MessageToUser() {
                ToUserId = m.ToUserId,
                ToTransportKind = message.TransportKind,
                IsWebChatGreeting = true
            });
            entities.SaveChanges();
        }

        private void LiteLinkMessage(ChatEntities entities, Transport transport, LiteDeepLinkStrategy.Context llc) {
            string text = _texts.LiteLinkKnownUser;
            SendMessage(entities, transport, text, true);

            if (llc.RouteTo != null) {
                text = _texts.LiteLinkRouted;
                if (!String.IsNullOrEmpty(text)) {
                    text = text.Replace(_texts.LiteLinkRouted_Addressee(), llc.RouteTo.FullName ?? "");
                    SendMessage(entities, transport, text, true);
                }                    
            } else
                SendMessage(entities, transport, _texts.LiteLinkUnrouted, true);            
        }

        private void InviteLinkMessage(ChatEntities entities, Transport transport, InviteLinkStrategy.Context ilc)
        {
            //  Послать приветствие
            string text = _scenarioStarter.StartScenario(entities, ScenarioType.Invite, ilc.UserId.Value, transport.TransportTypeId, out int? st);
            if(text != null)
                SendMessage(entities, transport, text, true, st);

            
            //MessageToUser mtu = CreateMessageToUser(entities, transport, null, text);
            ////var kb = ViberSender.GetMainKeyboard(_scenarioButtons.GetButtons(ScenarioType.Invite, 0), mtu);
            //var kb = ViberSender.GetMainKeyboard(_scenarioButtons.Adapt(ScenarioType.Invite, 0), null);


            //  Зафиксировать в сценарии с начальным шагом
            //_scenarioStarter.StartScenario(entities, ScenarioType.Invite, )

            //if (ilc.RouteTo != null)
            //{
            //    text = _texts.LiteLinkRouted;
            //    if (!String.IsNullOrEmpty(text))
            //    {
            //        text = text.Replace(_texts.LiteLinkRouted_Addressee(), ilc.RouteTo.FullName ?? "");
            //        SendMessage(entities, transport, text, true);
            //    }
            //}
            //else
            //    SendMessage(entities, transport, _texts.LiteLinkUnrouted, true);
        }

        private void CommonLinkMessage(ChatEntities entities, Transport transport, CommonLinkStrategy.Context clc)
        {
            //  Послать приветствие
            string text = _scenarioStarter.StartScenario(entities, ScenarioType.Common, clc.UserId.Value, transport.TransportTypeId, out int? st);
            if (text != null)
                SendMessage(entities, transport, text, true, st);          
        }

        private void SendMessage(ChatEntities entities, Transport transport, string text, bool needToChange = false, int? scenarioStepId = null) {
            if (String.IsNullOrEmpty(text))
                return;
            Message msg = new Message() {
                Kind = DAL.MessageKind.Personal,
                FromUserId = Global.SystemBotId,
                FromTransportKind = TransportKind.FLChat,
                Text = text,
                NeedToChangeText = needToChange,
                ScenarioStepId = scenarioStepId,
            };
            msg.ToUsers.Add(new MessageToUser() {
                ToUserId = transport.UserId,
                ToTransportKind = transport.Kind,
            });
            entities.Message.Add(msg);
            entities.SaveChanges();
        }

        private void SendDefaultMessage(ChatEntities entities, Transport transport) {
            string text = Settings.Values.GetValue("TEXT_GREETING_MSG", null);
            if (String.IsNullOrEmpty(text))
                return;
            SendMessage(entities, transport, text);
        }

        /// <summary>
        /// Called before create new transport for user who accepted deep link
        /// </summary>
        /// <param name="entities">data entities</param>
        /// <param name="message">Transport message info</param>
        /// <param name="user">accepted deep link user</param>
        public void BeforeAddTransport(ChatEntities entities, IDeepLinkData message, User user) {
        }
    }

}
