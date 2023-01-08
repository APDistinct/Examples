using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using System.Net;
using System.Data.Entity;

namespace FLChat.WebService.Handlers.WebChat
{
    public class AnswerWebChat : WebChatBase, IObjectedHandlerStrategy<WebChatAnswerRequest, object>
    {
        public bool IsReusable => true;

        public object ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, WebChatAnswerRequest input) {
            WebChatDeepLink webChat = LoadWebChat(entities, currUserInfo, input.Code);

            DAL.Model.Message msg = new DAL.Model.Message() {
                AnswerTo = webChat.MessageToUser.Message,
                FromTransport = webChat.MessageToUser.ToTransport,
                Kind = MessageKind.Personal,
                Text = input.Text,
                ToUsers = new MessageToUser[] {
                    new MessageToUser() {
                        ToTransport = webChat.MessageToUser.Message.FromTransport,
                        IsSent = (webChat.MessageToUser.Message.FromTransportKind == TransportKind.FLChat)
                    }
                }
            };
            entities.Message.Add(msg);
            entities.SaveChanges();
            return null;
        }
    }
}
