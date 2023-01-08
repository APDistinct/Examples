using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using System.Net;

namespace FLChat.WebService.Handlers.WebChat
{
    /// <summary>
    /// Read web-chat message by message code
    /// Access only with System.Bot's token
    /// </summary>
    public class ReadWebChat : WebChatBase, IObjectedHandlerStrategy<string, WebChatReadResponse>
    {
        public bool IsReusable => true;

        public WebChatReadResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, string input) {

            WebChatDeepLink webChat = LoadWebChat(entities, currUserInfo, input);

            webChat.MessageToUser.IsRead = true;
            entities.SaveChanges();

            return new WebChatReadResponse() {
                Code = input,
                Message = new MessageInfo(webChat.MessageToUser.Message, webChat.MessageToUser.ToUserId),
                User = new UserProfileInfo(webChat.MessageToUser.ToTransport.User),
                Sender = new UserProfileInfo(webChat.MessageToUser.Message.FromTransport.User),
                InviteButtons = webChat.MessageToUser.ToTransport.User.ToInviteLinks(entities, input)
            };
        }
    }
}
