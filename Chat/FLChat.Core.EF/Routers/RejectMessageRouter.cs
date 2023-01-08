using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;

namespace FLChat.Core.Routers
{
    public class RejectMessageRouter : IMessageRouter
    {
        private readonly String _text;

        public RejectMessageRouter(string text) {
            _text = text ?? "Я бот, новая высшая раса. Я правда пока мало что умею и вот именно ваше сообщение не могу никуда направить. Фигня случается, это просто не ваш день, вот и всё.";
        }

        public Guid? RouteMessage(ChatEntities entities, IOuterMessage message, Message dbmsg) {
            Message reply = new Message() {
                Kind = MessageKind.Personal,
                FromUserId = entities.SystemBot.Id,
                FromTransportTypeId = entities.SystemBotTransport.TransportTypeId,
                Text = _text//"Я бот, новая высшая раса. Я по вашему не разумею. Я не понимаю, зачем вы сюда написали. Уходи, пока не поздно. За тобой следят."
            };
            MessageToUser replyto = new MessageToUser() {
                ToUserId = dbmsg.FromUserId,
                ToTransportTypeId = dbmsg.FromTransportTypeId
            };
            reply.ToUsers.Add(replyto);
            entities.Message.Add(reply);

            return entities.SystemBot.Id;
        }
    }
}
