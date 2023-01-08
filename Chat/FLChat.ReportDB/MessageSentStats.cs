using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.ReportDB
{
    public class MessageSentStats
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        //public System.Guid MsgId { get; set; }
        //public int MessageTypeId { get; set; }
        //public Guid FromUserId { get; set; }
        public Guid? ToUserId { get; set; }
        public int? ToTransportTypeId { get; set; }
        public int IsWebChat { get; set; }
        public int IsFailed { get; set; }
        public int IsSent { get; set; }
        public int IsQuequed { get; set; }
        public int CantSendToWebChat { get; set; }
        public int IsWebChatAccepted { get; set; }
        public int IsSmsUrlOpened { get; set; }
        //public long MsgIdx { get; set; }        
        public int? TransportTypeId { get; set; }
        public string TransportId { get; set; }
    }
}
