using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Model
{
    public partial class Event : IEvent
    {
        public EventKind Kind {
            get => (EventKind)EventTypeId;
            set => EventTypeId = (int)value;
        }

        public TransportKind? CausedByTransportKind {
            get => (TransportKind?)CausedByUserTransportTypeId;
            set => CausedByUserTransportTypeId = (int?)value; }
    }
}
