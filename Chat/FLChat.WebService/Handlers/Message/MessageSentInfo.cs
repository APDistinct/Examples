using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using System.Net;

namespace FLChat.WebService.Handlers.Message
{
    /// <summary>
    /// Get statistic about single message
    /// </summary>
    public class MessageSentInfo : IObjectedHandlerStrategy<string, MessageSentInfoResponse>
    {
        public bool IsReusable => true;

        /// <summary>
        /// Can access only to himself sent messages
        /// </summary>
        public bool OnlySelfMessages { get; set; } = true;

        public MessageSentInfoResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, string input) {
            if (Guid.TryParse(input, out Guid msgId))
                return Process(entities, currUserInfo, msgId);            

            throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, $"Message id {input} is incorrect");
        }

        private MessageSentInfoResponse Process(ChatEntities entities, IUserAuthInfo currUserInfo, Guid msgId) {
            //load message
            DAL.Model.Message msg = entities
                .Message
                .Where(m => m.Id == msgId)
                .Include(m => m.FileInfo)
                .Include(m => m.FileInfo.MediaType)
                .SingleOrDefault();

            //if not found
            if (msg == null)
                throw new ErrorResponseException(HttpStatusCode.NotFound, ErrorResponse.Kind.not_found, $"Message with id {msgId.ToString()} has not found");

            //check access to messages sent by another user
            if (OnlySelfMessages && currUserInfo.UserId != msg.FromUserId)
                throw new ErrorResponseException(HttpStatusCode.NotFound, ErrorResponse.Kind.not_found, $"Message with id {msgId.ToString()} has not found");

            //load stats
            MessageStatsRowsView []rows = entities
                .MessageStatsRowsView
                .Where(i => i.MsgId == msgId)
                .ToArray();

            MessageStatsGroupedView grouped = entities
                .MessageStatsGroupedView
                .Where(i => i.MsgId == msgId)
                .Single();

            //return response
            return new MessageSentInfoResponse(grouped, rows, msg);
        }
    }
}
