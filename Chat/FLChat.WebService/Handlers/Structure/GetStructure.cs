using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using FLChat.DAL.DataTypes;
using FLChat.WebService.Utils;
using System.Net;

namespace FLChat.WebService.Handlers
{
    public class GetStructure : IObjectedHandlerStrategy<PartialDataIdRequest, StructureResponse>
    {
        public static readonly string Key = typeof(PartialDataIdRequest).GetJsonPropertyName(nameof(PartialDataIdRequest.Ids));

        public bool IsReusable => true;

        public int MaxCount { get; set; } = 100;

        public StructureResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, string inputId) =>
            ProcessRequest(entities, currUserInfo, new PartialDataIdRequest() { Ids = inputId });

        public StructureResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, PartialDataIdRequest input)
        {
            if (input == null)
                input = new PartialDataIdRequest();
            input.MaxCount = MaxCount;

            DAL.Model.User currUser = currUserInfo.GetUser(entities);
            entities.Entry(currUser).Collection(u => u.BroadcastProhibition).Load();

            StructureNodeFullInfo node = entities.ExecuteStructureNode_GetInfo(
                input.Ids, 
                currUserInfo.UserId, 
                input.Offset ?? 0, 
                input.Count());
            if (node == null)
                throw new ErrorResponseException(HttpStatusCode.NotFound, ErrorResponse.Kind.not_found, $"Segment with id <{input.Ids}> has not found");

            Dictionary<Guid, MessageToUser> messages = null;
            Dictionary<Guid, int> unread = null;
            Dictionary<Guid, List<string>> tags = null;
            HashSet<Guid> broadcastProhibitionStructure = null;
            HashSet<Guid> hasChilds = null;
            if (node.Users.Count > 0) {
                Guid[] ids = node.Users.Select(u => u.Id).ToArray();
                messages = entities.GetLastMessages(currUserInfo.UserId, ids);
                unread = entities.CountOfUnreadMessages(currUserInfo.UserId, ids);
                tags = entities.LoadShortInfoSegments(ids);
                broadcastProhibitionStructure = entities.GetBroadcastProhibitionStructure(currUserInfo.UserId, ids);
                hasChilds = entities.GetHasChilds(ids);
            }
            StructureResponse sir = new StructureResponse(node, currUser, messages, unread, tags, input, 
                broadcastProhibitionStructure, hasChilds) {
                Count = node.Users.Count,
                TotalCount = node.TotalCount
            };
            return sir;
        }
    }
}
