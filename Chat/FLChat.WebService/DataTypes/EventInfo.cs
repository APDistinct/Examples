using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using FLChat.DAL;
using FLChat.DAL.Model;
using Newtonsoft.Json.Converters;
using System.Net;
using FLChat.Core;

namespace FLChat.WebService.DataTypes
{
    public class EventInfo
    {
        [JsonIgnore]
        private readonly Event _event;

        public EventInfo(Event e, Guid currentUserId, IMessageTextCompiler compiler = null) {
            _event = e;
            if (_event.Kind == EventKind.MessageIncome) {
                MessageToUser msgTo = _event
                    .Message
                    .ToUsers
                    //.Where(mt => mt.ToUserId == currentUserId && mt.ToTransportTypeId == (int)TransportKind.FLChat)
                    .Where(mt => mt.ToUserId == currentUserId)
                    //.OrderByDescending(mt => mt.ToTransport.TransportType.InnerTransport)
                    //.ThenByDescending(mt => mt.ToTransport.TransportType.Prior)
                    .FirstOrDefault();
                if (msgTo == null)
                    throw new ErrorResponseException(HttpStatusCode.InternalServerError, ErrorResponse.Kind.not_found, "Message addressee for incoming message type event has not found");
                Message = new MessageIncomeInfo(msgTo, currentUserId, compiler?.MakeText(msgTo));
            } else
                Message = null;
            MessageStatus = _event.Kind.IsMessageLifeCicleEvent() ? new MessageStatusInfo(_event) : null;
        }

        [JsonProperty(PropertyName = "id")]
        public long Id => _event.Id;

        [JsonProperty(PropertyName = "kind")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EventKind Kind => _event.Kind;

        [JsonProperty(
            PropertyName = "msg",
            NullValueHandling = NullValueHandling.Ignore
            )]
        public MessageInfo Message { get; }

        [JsonProperty(
            PropertyName = "msg_status",
            NullValueHandling = NullValueHandling.Ignore
            )]
        public MessageStatusInfo MessageStatus { get; }

        [JsonProperty(PropertyName = "caused_by_user")]
        public Guid CausedByUserId => _event.CausedByUserId;

        [JsonProperty(PropertyName = "caused_by_transport")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TransportKind? CausedByUserTransport => _event.CausedByTransportKind;
    }
}
