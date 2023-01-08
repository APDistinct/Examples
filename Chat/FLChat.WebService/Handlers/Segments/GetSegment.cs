using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;

namespace FLChat.WebService.Handlers.Segments
{
    /// <summary>
    /// Return list of not deleted segments names
    /// </summary>
    public class GetSegment : IObjectedHandlerStrategy<string, SegmentInfoResponse>
    {
        public bool IsReusable => true;

        public SegmentInfoResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, string input) {
            Segment segment;
            if (Guid.TryParse(input, out Guid sid))
                segment = entities.Segment.Where(x => x.Id == sid).FirstOrDefault();
            else
                segment = entities.Segment.Where(x => x.Name == input).FirstOrDefault();

            if (segment == null)
                throw new ErrorResponseException(
                    (int)HttpStatusCode.NotFound,
                    new ErrorResponse(ErrorResponse.Kind.user_not_found, $"Segment {input} not found"));

            return ProcessRequest(entities, currUserInfo, segment);
        }

        private SegmentInfoResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, Segment segment) {
            DAL.Model.User currUser = currUserInfo.GetUser(entities);
            entities.Entry(currUser).Collection(u => u.BroadcastProhibition).Load();

            var list = segment.GetMembersForUser(entities, currUser.Id);

            Dictionary<Guid, MessageToUser> lastMessages = null;
            Dictionary<Guid, int> unread = null;
            if (list.Count != 0) {
                Guid[] ids = list.Select(u => u.Id).ToArray();
                lastMessages = entities.GetLastMessages(currUser.Id, ids);
                unread = entities.CountOfUnreadMessages(currUser.Id, ids);
            }
            return new SegmentInfoResponse(segment, list.Count, list, currUser, lastMessages, unread);
        }
    }
}