using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using System.Net;
using FLChat.WebService.Utils;

namespace FLChat.WebService.Handlers.Message
{
    public class MessageSentHistory : IObjectedHandlerStrategy<MessageSentHistoryRequest, MessageSentHistoryResponse> {
        public bool IsReusable => true;

        public int MaxCount { get; set; } = 100;

        public static readonly string Key = typeof(MessageSentHistoryRequest)
            .GetJsonPropertyName(nameof(MessageSentHistoryRequest.Ids));

        public enum ModeEnum
        {
            CurrentUser,

            SelectedUser,

            All
        }

        public ModeEnum Mode { get; set; } = ModeEnum.CurrentUser;

        /// <summary>
        /// If true, then property User in MessageSentHistoryItem will assigned with sender information
        /// </summary>
        public bool IncludeUserInfo { get; set; } = false;

        public MessageSentHistoryResponse ProcessRequest(
            ChatEntities entities, 
            IUserAuthInfo currUserInfo,
            MessageSentHistoryRequest input) 
        {
            switch (Mode) {
                case ModeEnum.CurrentUser:
                    return Perform(entities, currUserInfo.UserId, input);
                case ModeEnum.SelectedUser:
                    if (Guid.TryParse(input.Ids, out Guid guid))
                        return Perform(entities, guid, input);
                    else
                        throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, 
                            $"User id <{input.Ids}> is invalid");
                case ModeEnum.All:
                    return Perform(entities, null, input);
                default:
                    throw new ErrorResponseException(HttpStatusCode.InternalServerError, ErrorResponse.Kind.not_support,
                        "Unknown mode value");
            }
        }

        private class Record
        {
            public DAL.Model.Message Msg { get; set; }
            public MessageStatsGroupedView Stats { get; set; }
            public FileInfo File { get; set; }
        }

        private MessageSentHistoryResponse Perform(ChatEntities entities, Guid? userId,
            MessageSentHistoryRequest input) {
            //adjust input values
            if (input == null)
                input = new MessageSentHistoryRequest();
            input.MaxCount = MaxCount;

            //add default message kinds
            if (input.Types == null || input.Types.Length == 0)
                input.Types = new MessageKind[] { MessageKind.Broadcast, MessageKind.Mailing };

            IQueryable<MessageStatsGroupedView> query = entities
                .MessageStatsGroupedView
                .Where(i => input.Types.Cast<int>().Contains(i.MessageTypeId));
            if (userId.HasValue)
                query = query.Where(i => i.FromUserId == userId.Value);

            //make query
            //var query = (
            //    from stats in entities.MessageStatsGroupedView
            //    join msg in entities.Message on stats.MsgId equals msg.Id
            //    join fi in entities.FileInfo on msg.FileId equals fi.Id
            //      into fjoin from fgroup in fjoin.DefaultIfEmpty()
            //    where stats.FromUserId == currUserInfo.UserId
            //       && input.Types.Cast<int>().Contains(stats.MessageTypeId)
            //    orderby stats.MsgIdx descending
            //    select new Record() { Msg = msg, Stats = stats, File = fgroup }
            //    );

            //calculate total counts
            int? total = null;
            if ((input.Offset ?? 0) == 0 && input.StartFrom.HasValue == false)
                total = query.Count();

            if (input.StartFrom.HasValue) {
                long? idx = entities.Message.Where(m => m.Id == input.StartFrom.Value).Select(m => m.Idx).SingleOrDefault();
                if (idx == null)
                    throw new ErrorResponseException(HttpStatusCode.NotFound, ErrorResponse.Kind.input_data_error, "Message with id in field <start_from> has not found");
                query = query.Where(r => r.MsgIdx < idx.Value).OrderByDescending(i => i.MsgIdx).TakePart(input, false);
            } else {
                //get partiotion
                query = query.OrderByDescending(i => i.MsgIdx).TakePart(input);
            }
            //and perform query
            MessageStatsGroupedView[] records = query.ToArray();
            Guid[] msgIdx = records.Select(r => r.MsgId).ToArray();

            Dictionary<Guid, DAL.Model.Message> messages = entities
                .Message
                .Where(m => msgIdx.Contains(m.Id))
                .Include(m => m.FileInfo)
                .Include(m => m.FileInfo.MediaType)
                .ToArray()
                .ToDictionary(m => m.Id);


            Dictionary<Guid, DAL.Model.User> users = null;
            if (IncludeUserInfo) {
                Guid[] usrIds = records.Select(r => r.FromUserId).ToArray();
                users = entities
                    .User
                    .Where(u => usrIds.Contains(u.Id))
                    .Include(u => u.Rank)
                    .ToArray()
                    .ToDictionary(u => u.Id);
            }

            //get all media types
            //int[] mediaIds = records.Where(i => i.File != null).Select(i => i.File.MediaTypeId).Distinct().ToArray();
            //load media types
            //if (mediaIds.Length > 0)
            //    entities.MediaType.Where(mt => mediaIds.Contains(mt.Id)).Load();

            //make answer
            return new MessageSentHistoryResponse(input) {
                Count = records.Length,
                Messages = records.Select(i => new MessageSentHistoryItem(i, messages[i.MsgId]) {
                    User = IncludeUserInfo ? new UserInfoAdmin(users[i.FromUserId]) : null
                }),
                TotalCount = total,
                LastMessageId = records.Length > 0 ? records[records.Length - 1].MsgId : (Guid?)null,
                StartedFrom = input.StartFrom
            };
        }
    }
}
