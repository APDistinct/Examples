using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.Handlers.Message
{
    public class MessageTypeLimitGet : IObjectedHandlerStrategy<object, MessageTypeLimitResponse>
    {
        public bool IsReusable => true;

        public MessageTypeLimitResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, object input)
        {
            //var dbLiist = entities.MessageType.ToList().Select(x => new MessageTypeLimit(x)).ToList();
            var dbLiist = entities.MessageType.ToList();
            var inner = Enum.GetValues(typeof(MessageKind)).Cast<MessageKind>().ToList();
            var li = dbLiist.Join(inner, l => l.Kind, i => i, (l, i) => new MessageTypeLimit(l)).ToList();
            //var inner = Enum.GetValues(typeof(MessageKind)).Cast<MessageKind>().ToList().Select(x => (int)x).ToList();
            //var li = dbLiist.Join(inner, l => l.Id, i => i, (l, i) => new MessageTypeLimit(l)).ToList();

            //{ Type = x.Kind.ToString(), LimitForDay = x.LimitForDay, LimitForOnce = x.LimitForOnce}
            //List<MessageTypeLimit> li = new List<MessageTypeLimit>();

            return new MessageTypeLimitResponse() { MessageType = li};
        }
    }
}
