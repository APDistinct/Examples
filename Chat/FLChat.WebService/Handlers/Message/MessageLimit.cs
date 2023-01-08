using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;

namespace FLChat.WebService.Handlers.Message
{
    public class MessageLimit : IObjectedHandlerStrategy<SendMessageLimitRequest, LimitInfo>
    {
        public bool IsReusable => true;

        public LimitInfo ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, SendMessageLimitRequest input) {
            LimitAndSentInfo li = entities.GetLimitInfo(currUserInfo.UserId, input.Type);
            return new LimitInfo(
                li.MessageType, 
                li.SentCount?.Count ?? 0, 
                GetSelectionCount(entities, currUserInfo, input));
        }

        private int? GetSelectionCount(ChatEntities entities, IUserAuthInfo currUserInfo, SendMessageLimitRequest input) {
            if (input.Selection == null)
                return null;
            return entities.MessageCountForUserSelection(
                currUserInfo.UserId,
                input.Selection.Convert(),
                input.Type);
        }
    }
}
