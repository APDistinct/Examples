using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using FLChat.WebService.Utils;

namespace FLChat.WebService.Handlers.User
{
    /// <summary>
    /// Return Profile of current User 
    /// </summary>
    public class GetUserChilds : IObjectedHandlerStrategy<PartialDataIdRequest, UserChildResponse>
    {
        public static readonly string Key = typeof(PartialDataIdRequest).GetJsonPropertyName(nameof(PartialDataIdRequest.Ids));

        private readonly bool _getAll;

        public bool CalcalulateStructureCapacity { get; set; } = false;

        public GetUserChilds(bool getAll = false)
        {
            _getAll = getAll;
        }

        public bool IsReusable => true;

        public int MaxCount { get; set; } = 100;

        public UserChildResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, string input) =>
            ProcessRequest(entities, currUserInfo, input != null ? new PartialDataIdRequest() { Ids = input } : null);

        public UserChildResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, PartialDataIdRequest input)
        {
            if (input == null)
                input = new PartialDataIdRequest();
            input.MaxCount = MaxCount;

            DAL.Model.User currUser = currUserInfo.GetUser(entities);

            if (input.Ids == null)
                return ProcessRequest(entities, currUserInfo.UserId, currUser, input, _getAll);
            else if (Guid.TryParse(input.Ids, out Guid id))
                return ProcessRequest(entities, id, currUser, input, _getAll);

            throw new ErrorResponseException(
                (int)HttpStatusCode.NotFound,
                new ErrorResponse(ErrorResponse.Kind.user_not_found, $"User with id {input} not found"));
        }

        private UserChildResponse ProcessRequest(ChatEntities entities, Guid userId, DAL.Model.User currUser,
            IPartialData partial, bool getAll = false) {
            //load info for parent user
            DAL.Model.User user = entities.User
                .Where(u => (u.Id == userId) && (getAll || u.Enabled))
                .SingleOrDefault();

            if (user == null)
                throw new ErrorResponseException(HttpStatusCode.NotFound, ErrorResponse.Kind.user_not_found, $"User with id {userId} not found");

            entities.Entry(currUser).Collection(u => u.BroadcastProhibition).Load();

            //request parents
            Guid[] parents = entities.User_GetParents(userId, null).Select(r => r.UserId).ToArray();

            //if for user or any user's parents has broadcast prohibition, then 
            bool broadcastProhibitionParents = currUser.BroadcastProhibition.Select(s => s.Id).Intersect(parents).Any();
            bool broadcastProhibitionUser = currUser.BroadcastProhibition.Select(s => s.Id).Contains(userId);

            int? totalCount = null;
            //base request: query direct childs for parent user
            IQueryable<DAL.Model.User> query = entities
                .User
                .Where(u => (getAll || u.Enabled) && u.OwnerUserId == user.Id);

            //calculate childs count
            if ((partial.Offset ?? 0) == 0)
                totalCount = entities
                    .User
                    .Where(u => (getAll || u.Enabled) && u.OwnerUserId == user.Id)
                    .Count();

            //var q = (from c in )

            //sorting childs and take partial data
            query = query
                .OrderByName()
                .TakePart(partial);

            //query childs has owns childs or not
            var queryChilds = entities
                .User_GetChilds(userId, 2, getAll)
                .Where(r => r.Deep == 2)
                .Select(r => new { UserId = r.OwnerUserId })
                .Distinct();

            //union two requests
            var list = (from u in query
                        join c in queryChilds on u.Id equals c.UserId
                           into uj
                        from su in uj.DefaultIfEmpty()
                            //join bps in entities.BroadcastProhibition_Structure(userId, 1) on u.Id equals bps 
                            //   into bps_g
                            //from bps_r in bps_g.DefaultIfEmpty()
                        select new UserEx {
                            User = u,
                            HasChilds = su.UserId != null,
                            BroadcastProhibitionStructure = broadcastProhibitionUser || broadcastProhibitionParents,
                        })
                         .OrderByName()
                         .ToArray();

            //load ranks
            int[] rankIds = list
                .Select(u => u.User.RankId)
                .Concat(Enumerable.Repeat(user.RankId, 1))
                .Where(r => r.HasValue)
                .Select(r => r.Value)
                .Distinct()
                .ToArray();
            entities.Rank.Where(r => rankIds.Contains(r.Id)).Load();

            Guid[] userIds = list.Select(u => u.User.Id).Concat(Enumerable.Repeat(userId, 1)).ToArray();

            Dictionary<Guid, MessageToUser> lastMessages = null;
            Dictionary<Guid, int> unread = null;
            Dictionary<Guid, List<string>> tags = null;
            if (userIds.Length != 0) {
                lastMessages = entities.GetLastMessages(currUser.Id, userIds);
                unread = entities.CountOfUnreadMessages(currUser.Id, userIds);
                tags = entities.LoadShortInfoSegments(userIds);
            }

            UserChildResponse resp = new UserChildResponse(user, list, currUser, lastMessages, unread, tags, partial) {
                TotalCount = totalCount
            };
            resp.User.HasChilds = list.Length > 0;
            resp.User.BroadcastProhibitionStructure = broadcastProhibitionParents;            

            if (CalcalulateStructureCapacity && currUser.Id == userId)
                resp.TotalChildsCount = entities.User_GetChilds(userId, null, null).Count();

            return resp;
        }
    }
}
