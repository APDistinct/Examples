using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FLChat.Core
{
    public class MessageStats
    {
        public string TransportId { get; set; }
        public Nullable<int> SentTo { get; set; }
        public Nullable<int> ViberStatus { get; set; }
        public Nullable<int> SmsStatus { get; set; }
        public Nullable<System.DateTime> UpdatedTime { get; set; }
        public bool IsFinished { get; set; }
    }

    /// <summary>
    /// Message status via Devino perform interface
    /// </summary>
    public interface IMessageStatusPerformer
    {
        Task<IEnumerable<MessageStats>> Perform(string[] ids, CancellationToken ct);
    }
}
