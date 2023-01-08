using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using System.Net;

namespace FLChat.WebService.Handlers.User
{
    public class GetUserParents : IObjectedHandlerStrategy<string, UserParentResponse>
    {
        public bool IsReusable => true;

        public UserParentResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, string input) {
            if (input == null)
                return GetParents(entities, currUserInfo.UserId, currUserInfo);
            else if (Guid.TryParse(input, out Guid id))
                return GetParents(entities, id, currUserInfo);

            throw new ErrorResponseException(
                (int)HttpStatusCode.NotFound,
                new ErrorResponse(ErrorResponse.Kind.user_not_found, $"User with id <{input}> not found"));
        }

        private static UserParentResponse GetParents(ChatEntities entities, Guid userId, IUserAuthInfo currUserInfo) {
            DAL.Model.User user = entities.User
                .Where(u => u.Id == userId && u.Enabled)
                .SingleOrDefault();
            if (user == null)
                throw new ErrorResponseException(HttpStatusCode.NotFound, ErrorResponse.Kind.user_not_found, 
                    $"User with id {userId} has't found");

            DAL.Model.User currUser = currUserInfo.GetUser(entities);
            entities.Entry(currUser).Collection(u => u.BroadcastProhibition).Load();

            DAL.Model.User[] users = (from p in entities.User_GetParents(userId, null)
                         join u in entities.User on p.UserId equals u.Id
                         orderby p.Deep descending
                         select u).ToArray();

            //DAL.Model.User[] users = entities
            //    .User_GetParents(userId, null)
            //    .OrderByDescending(u => u.Deep)
            //    .Join(entities.User, p => p.UserId, u => u.Id, (p, u) => u)
            //    .ToArray();

            Guid[] userIds = users.Select(u => u.Id).Concat(Enumerable.Repeat(userId, 1)).ToArray();

            Dictionary<Guid, MessageToUser> lastMessages = null;
            Dictionary<Guid, int> unread = null;
            Dictionary<Guid, List<string>> tags = null;
            HashSet<Guid> broadcastProhibitionStructure = null;
            if (userIds.Length != 0) {
                lastMessages = entities.GetLastMessages(currUser.Id, userIds);
                unread = entities.CountOfUnreadMessages(currUser.Id, userIds);
                tags = entities.LoadShortInfoSegments(userIds);
                broadcastProhibitionStructure = entities.GetBroadcastProhibitionStructure(currUserInfo.UserId, userIds);
            }

            return new UserParentResponse(user, users, currUser, lastMessages, unread, tags, broadcastProhibitionStructure);
        }
    }
}
