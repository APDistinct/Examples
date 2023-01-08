using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Model
{
    public partial class MessageCountOverToday
    {
    }

    public class LimitAndSentInfo
    {        
        public MessageType MessageType { get; set; }
        public MessageCountOverToday SentCount { get; set; }
    }

    public static class MessageCountForTodayExtentions
    {
        public static LimitAndSentInfo GetLimitInfo(this ChatEntities entities, Guid userId, MessageKind mk) {
            var mcq = entities.MessageCountOverToday.Where(r => r.FromUserId == userId);
            var q = (from mt in entities.MessageType
                     join sent in mcq on mt.Id equals sent.MessageTypeId into sent_r
                     from sent_d in sent_r.DefaultIfEmpty()
                     where mt.Id == (int)mk
                     select new LimitAndSentInfo { MessageType = mt, SentCount = sent_d });
            return q.Single();
        }
    }
}
