using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;

using Z.EntityFramework.Plus;

namespace FLChat.WebService.Handlers.Message
{
    public class ReadMessageNotify : IObjectedHandlerStrategy<ReadMessageNotifyRequest, object>
    {
        public bool IsReusable => true;

        public object ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, ReadMessageNotifyRequest input) {
            if (input.Messages.Length == 1) {
                Guid msgId = input.Messages[0];
                entities.MessageToUser
                    .Where(m => m.MsgId == msgId && m.ToUserId == currUserInfo.UserId)
                    .Update(m => new MessageToUser() { IsRead = true });
            } else {
                entities.ExecuteMessageSetRead(currUserInfo.UserId, input.Messages);
            }
            return null;
        }
    }
}
