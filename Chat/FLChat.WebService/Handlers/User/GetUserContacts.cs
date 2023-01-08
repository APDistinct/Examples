using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using FLChat.WebService.Utils;

namespace FLChat.WebService.Handlers
{
    /// <summary>
    /// Return user's profile
    /// </summary>
    public class GetUserContacts : IObjectedHandlerStrategy<PartialDataIdRequest, UserContactsResponse>
    {
        public static readonly string Key = typeof(PartialDataIdRequest).GetJsonPropertyName(nameof(PartialDataIdRequest.Ids));

        private readonly bool _getAll;

        public GetUserContacts(bool getAll = false)
        {
            _getAll = getAll;
        }

        public int MaxCount { get; set; } = 100;

        public bool IsReusable => true;

        public UserContactsResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, string input) =>
            ProcessRequest(entities, currUserInfo, input != null ? new PartialDataIdRequest() { Ids = input } : null);

        public UserContactsResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, PartialDataIdRequest input)
        {
            if (input == null)
                input = new PartialDataIdRequest();
            input.MaxCount = MaxCount;

            if (input.Ids == null)
                return ProcessRequest(entities, currUserInfo, currUserInfo.UserId, input, _getAll);
            else //if (Guid.TryParse(input, out Guid id))
                throw new NotSupportedException("Can't request contacts for another user");
                //return ProcessRequest(entities, id, _getAll);

            throw new ErrorResponseException(
                (int)HttpStatusCode.NotFound,
                new ErrorResponse(ErrorResponse.Kind.user_not_found, $"User with id {input} not found"));
        }

        public static UserContactsResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, Guid userId, 
            IPartialData partial, bool getAll = false) {
            DAL.Model.User user = entities.User
                .Where(u => (u.Id == userId) && (getAll || u.Enabled))
                .SingleOrDefault();
            if (user == null)
                throw new ErrorResponseException(
                    (int)HttpStatusCode.NotFound,
                    new ErrorResponse(ErrorResponse.Kind.user_not_found, $"User with id {userId} not found"));

            DAL.Model.User currUser = currUserInfo.GetUser(entities);
            entities.Entry(currUser).Collection(u => u.BroadcastProhibition).Load();

            //get list of last messages. Use direct SQL because EF has odd bag. If get rows from this view without select, 
            // then all rows are equal to first row
            LastMessageView[] lastMessages = entities.GetLastMessageViewForContact(userId, partial.Offset ?? 0, partial.Count().Value);

            int? totalCount = null;
            if ((partial.Offset ?? 0) == 0)
                totalCount = entities.GetLastMessageViewCountForContact(userId);

            if (lastMessages.Length == 0)
                return new UserContactsResponse(new DAL.Model.User[] { }, currUser, null, null);

            //load users from last messages
            Guid[] userIds = lastMessages.Select(m => m.UserOppId).ToArray();
            Dictionary<Guid, DAL.Model.User> users = entities
                .User
                .Where(u => u.Enabled && userIds.Contains(u.Id))
                //.Include(u => u.Transports)
                .Include(u => u.Rank)
                .ToDictionary(u => u.Id);

            //load MessageToUsers and Message
            long[] mtuIdx = lastMessages.Select(m => m.MsgToUserIdx).ToArray();
            Dictionary<Guid, MessageToUser> msgs = entities
                .MessageToUser
                .Where(m => mtuIdx.Contains(m.Idx))
                .Include(m => m.Message)
                .ToDictionary(m => m.ToUserId == userId ? m.Message.FromUserId : m.ToUserId);

            Dictionary<Guid, int> unread = lastMessages.ToDictionary(i => i.UserOppId, i => i.UnreadCnt ?? 0); //entities.CountOfUnreadMessages(currUserInfo.UserId, users.Select(u => u.Key));
            Dictionary<Guid, List<string>> tags = entities.LoadShortInfoSegments(userIds);

            HashSet<Guid> broadcastProhibitionStructure = entities.GetBroadcastProhibitionStructure(currUserInfo.UserId, userIds);
            HashSet<Guid> hasChilds = entities.GetHasChilds(userIds);

            //make response
            return new UserContactsResponse(
                lastMessages.Select(lm => users[lm.UserOppId]),
                currUser,
                msgs,
                unread,
                tags,
                partial,
                broadcastProhibitionStructure,
                hasChilds) {
                Count = lastMessages.Length,
                TotalCount = totalCount
            };
        }
    }
}
