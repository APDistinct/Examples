using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using System.Data.SqlClient;
using System.Net;
using System.Data.Entity.Core.Objects;

namespace FLChat.WebService.Handlers.User
{
    public class SearchUser : IObjectedHandlerStrategy<SearchUserRequest, UserChildResponse>
    {
        public bool IsReusable => true;

        public int MaxCount { get; set; } = 100;

        public UserChildResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, SearchUserRequest input) {
            if (input == null || input.SearchValue == null)
                throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, "Search parameter [search] must be present");
            if (input.SearchValue.Length < 3)
                throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, "Length of search parameter [search] must be greater then 2");

            DAL.Model.User currUser = currUserInfo.GetUser(entities);
            entities.Entry(currUser).Collection(u => u.BroadcastProhibition).Load();

            input.MaxCount = MaxCount;

            var qUsers = (from u in entities.User 
                         join c in entities.User_GetChilds(currUserInfo.UserId, null, null) on u.Id equals c.UserId
                         select u);

            qUsers = qUsers
                .Include(u => u.Rank)
                .FindString(input.SearchValue);

            int? totalCount = null;
            if ((input.Offset ?? 0) == 0)
                totalCount = qUsers.Count();

            qUsers = qUsers
                .OrderByName()
                .TakePart(input);
            DAL.Model.User[] users = qUsers.ToArray();

            Guid[] userIds = users.Select(u => u.Id).ToArray();

            Dictionary<Guid, MessageToUser> lastMessages = null;
            Dictionary<Guid, int> unread = null;
            Dictionary<Guid, List<string>> tags = null;
            HashSet<Guid> broadcastProhibitionStructure = null;
            HashSet<Guid> hasChilds = null;
            if (userIds.Length != 0) {
                lastMessages = entities.GetLastMessages(currUserInfo.UserId, userIds);
                unread = entities.CountOfUnreadMessages(currUserInfo.UserId, userIds);
                tags = entities.LoadShortInfoSegments(userIds);
                broadcastProhibitionStructure = entities.GetBroadcastProhibitionStructure(currUserInfo.UserId, userIds);
                hasChilds = entities.GetHasChilds(userIds);
            }

            return new UserChildResponse(null, users.ToArray(), currUser, lastMessages, unread, 
                tags: tags, 
                partial: input, 
                broadcastProhibitionStructure: broadcastProhibitionStructure, 
                hasChilds: hasChilds) {
                TotalCount = totalCount
            };
        }
    }
}
