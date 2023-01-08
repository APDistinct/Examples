using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using FLChat.Core;

namespace FLChat.WebService.Handlers.Message
{
    /// <summary>
    /// Message's history request
    /// </summary>
    public class MessageHistory : IObjectedHandlerStrategy<MessageHistoryRequest, MessageHistoryResponse>
    {
        private readonly IMessageTextCompilerWithCheck _msgCompiler;

        public MessageHistory(IMessageTextCompilerWithCheck msgCompiler = null) {
            _msgCompiler = msgCompiler;
        }

        public bool IsReusable => true;

        /// <summary>
        /// Maximum count of message per request
        /// </summary>
        public int MaxCount { get; set; } = 100;

        public MessageHistoryResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, MessageHistoryRequest input) {
            if (input.Count.HasValue && Math.Abs(input.Count.Value) == 1)
                input.Count = input.Count * MaxCount;
            int count = Math.Min(Math.Max(2, Math.Abs(input.Count ?? MaxCount)), MaxCount);
            bool forward = input.MessageId.HasValue ? (input.Count ?? 1) >= 0 : false;
            bool orderAsc = input.Order.HasValue ? input.Order.Value == OrderEnum.Ascending : forward;

            long? lastMsgIdx = null;
            if (input.MessageId.HasValue)
                lastMsgIdx = entities.Message
                    .Where(m => m.Id == input.MessageId)
                    .Select(m => m.Idx)
                    .Cast<long?>()
                    .SingleOrDefault();

            //make request
            var query = entities.MessageToUser
                .Include(mtu => mtu.Message)
                .Where(mtu => (mtu.Message.MessageType.ShowInHistory) &&
                    (mtu.IsWebChatGreeting == false) &&
                    (mtu.Message.FromUserId == currUserInfo.UserId 
                    && mtu.ToUserId == input.UserId 
                    && mtu.Message.FromTransport.TransportType.InnerTransport
                    || 
                    mtu.Message.FromUserId == input.UserId 
                    && mtu.ToUserId == currUserInfo.UserId
                    && mtu.ToTransport.TransportType.InnerTransport)
                    
                    && mtu.Message.IsDeleted == false);

            if (lastMsgIdx.HasValue) {
                if (forward)
                    query = query.Where(mtu => mtu.Message.Idx > lastMsgIdx.Value);
                else
                    query = query.Where(mtu => mtu.Message.Idx < lastMsgIdx.Value);
            }
            //if (input.MessageId.HasValue)
            //    query = query.Where(mtu => mtu.MsgId != input.MessageId.Value);

            if (forward)
                query = query.OrderBy(mtu => mtu.Message.Idx);
            else
                query = query.OrderByDescending(mtu => mtu.Message.Idx);

            query = query.Take(count);

            if (orderAsc)
                query = query.OrderBy(mtu => mtu.Message.Idx);
            else
                query = query.OrderByDescending(mtu => mtu.Message.Idx);

            var list = query.ToList();

            MessageHistoryResponse response = new MessageHistoryResponse() {
                Forward = forward,
                LastId = (list.Count > 0 ? list[list.Count - 1].MsgId : input.MessageId),
                MaxCount = MaxCount,
                UserId = input.UserId,
                Order = orderAsc ? OrderEnum.Ascending : OrderEnum.Descending,
                Messages = list.Select(mtu =>  mtu.ToPersonalMessageInfo(currUserInfo.UserId, _msgCompiler?.MakeText(mtu)))
            };

            //get messages for update IsDelivered flag
            Guid[] msgGuids = list
                .Where(m => m.ToUserId == currUserInfo.UserId && m.IsDelivered == false && m.ToTransportKind == TransportKind.FLChat)
                .Select(m => m.MsgId)
                .ToArray();
            if (msgGuids.Length > 0) {
                entities.ExecuteMessageSetDelivered(currUserInfo.UserId, msgGuids, TransportKind.FLChat);
            }

            return response;
        }
    }
}
