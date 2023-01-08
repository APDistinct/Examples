using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.ReportDB
{
    public class DevinoMessageStatus
    {
        public string TransportId { get; set; }
        public Nullable<int> SentTo { get; set; }
        public Nullable<int> ViberStatus { get; set; }
        public Nullable<int> SmsStatus { get; set; }
        public Nullable<System.DateTime> UpdatedTime { get; set; }
        //public bool WebFormRequested { get; set; }
        public bool IsFinished { get; set; }
        public bool Update { get; set; }
    }
}
