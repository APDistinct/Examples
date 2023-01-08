using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.Core;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using System.Net;
using FLChat.Core.Algorithms;

namespace FLChat.WebService.Handlers.WebChat
{
    /// <summary>
    /// Read web-chat message by message code
    /// Access only with System.Bot's token
    /// </summary>
    public class ReadWebChatExt : WebChatBase, IObjectedHandlerStrategy<string, WebChatReadResponse>
    {
        private string messSender = "Sender";  

        public bool IsReusable => true;

        private readonly IDeepLinkStrategy _strategy;

        public ReadWebChatExt(IDeepLinkStrategy strategy)
        {
            _strategy = strategy;
        }

        private class FakeData : IDeepLinkData
        {
            public FakeData(string code)
            {
                DeepLink = code;
            }

            public string FromId => "";

            public string DeepLink { get; }

            public bool IsTransportEnabled => true;

            public TransportKind TransportKind => TransportKind.Test;
        }

        public WebChatReadResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, string input)
        {
            messSender = entities.SystemBot.FullName;
            if (!_strategy.AcceptDeepLink(entities, new FakeData(input),
                out DAL.Model.User user, out DAL.Model.Message answ, out object context, out IDeepLinkStrategy sender))
            {
                throw new ErrorResponseException(HttpStatusCode.NotFound, ErrorResponse.Kind.not_found, "Code not found");
            }

            if (context is WebChatDeepLinkStrategy.Context)
            {
                WebChatDeepLink webChat = LoadWebChat(entities, currUserInfo, input);

                webChat.MessageToUser.IsRead = true;
                entities.SaveChanges();

                return new WebChatReadResponse()
                {
                    Code = input,
                    Message = new MessageInfo(webChat.MessageToUser.Message, webChat.MessageToUser.ToUserId),
                    User = new UserProfileInfo(webChat.MessageToUser.ToTransport.User),
                    Sender = new UserProfileInfo(webChat.MessageToUser.Message.FromTransport.User),
                    InviteButtons = webChat.MessageToUser.ToTransport.User.ToInviteLinks(entities, input)
                };
            }
            else
            {
                if (context is LiteDeepLinkStrategy.Context)
                {
                    return new WebChatReadResponse()
                    {
                        Code = input,
                        Message = new MessageInfo(new DAL.Model.Message()
                        {
                            Text = Settings.Values.GetValue("LITE_LINK_MESSAGE", "Добро пожаловать в Faberlic Chat")
                        }, Guid.Empty),
                        User = user != null ? new UserProfileInfo(user) : null,
                        Sender = new UserProfileInfo(new DAL.Model.User() { FullName = messSender }),
                        InviteButtons = user.ToInviteLinks(entities, input)
                    };
                }
                else
                {
                    return new WebChatReadResponse()
                    {
                        Code = input,
                        Message = new MessageInfo(new DAL.Model.Message()
                        {
                            Text = Settings.Values.GetValue("INVITE_LINK_MESSAGE", "Добро пожаловать в Faberlic Chat")
                        }, Guid.Empty),
                        User = user != null ? new UserProfileInfo(user) : null,
                        Sender = new UserProfileInfo(new DAL.Model.User() { FullName = messSender }),
                        InviteButtons = user.ToInviteLinks(entities, input)
                    };
                }
            }
        }
    }
}
