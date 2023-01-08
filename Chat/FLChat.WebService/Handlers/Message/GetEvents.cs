using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Z.EntityFramework.Plus;

using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using FLChat.Core;

namespace FLChat.WebService.Handlers.Message
{
    public class GetEvents : IObjectedHandlerStrategy<EventsRequest, EventsResponse>
    {
        private readonly IMessageTextCompilerWithCheck _msgCompiler;

        public GetEvents(IMessageTextCompilerWithCheck msgCompiler = null) {
            _msgCompiler = msgCompiler;
        }

        /// <summary>
        /// Maximum count of events can extracted by request
        /// </summary>
        public int MaxCount { get; set; } = 500;

        public bool IsReusable => true;

        public EventsResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, EventsRequest input) {
            //using (var trans = entities.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted)) {

                if (input.LastEventId.HasValue == false)
                    input.LastEventId = entities.LastDeliveryEventId(currUserInfo.UserId);
                if (input.LastEventId.HasValue == false)
                    input.LastEventId = 0;

                if (input.Count.HasValue)
                    input.Count = Math.Max(1, Math.Min(MaxCount, input.Count.Value));
                else
                    input.Count = MaxCount;

                List<Event> events = entities
                    .Event
                    .Where(e => e.ToUsers.Where(u => u.Id == currUserInfo.UserId).Any() && e.Id > input.LastEventId)
                    .Include(e => e.Message)
                    .Include(e => e.Message.ToUsers)
                    .OrderBy(e => e.Id)
                    .Take(input.Count.Value)
                    .ToList();

                EventsResponse response = new EventsResponse() {
                    MaxCount = MaxCount,
                    LastId = events.Count == 0 ? input.LastEventId.Value : events[events.Count - 1].Id,
                    Events = events.Select(e => new EventInfo(e, currUserInfo.UserId, _msgCompiler))
                };

                if (events.Count > 0) {

                    //set messages IsDelivered flag
                    Guid[] msgGuids = events
                        .Where(e => e.Kind == EventKind.MessageIncome
                            && e.Message.ToUsers.Where(
                                tu => tu.ToUserId == currUserInfo.UserId
                                && tu.ToTransportKind == TransportKind.FLChat
                                && tu.IsDelivered == false).Any())
                        .Select(e => e.Message.Id)
                        .ToArray();
                    if (msgGuids.Length > 0)
                        entities.ExecuteMessageSetDelivered(currUserInfo.UserId, msgGuids, TransportKind.FLChat);
                    //set last delivered event id
                    entities.SetLastDeliveredEvent(currUserInfo.UserId, events[events.Count - 1].Id);
                }

                entities.User.Where(u => u.Id == currUserInfo.UserId).Update(u => new DAL.Model.User() { LastGetEvents = DateTime.UtcNow });

                //entities.Database.CurrentTransaction.Commit();
                return response;
            //}
        }
    }
}
