using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.ReportDB
{
    public interface IIdsReader
    {
        IEnumerable<string> GetIds(Guid messageId, ChatEntities entities);
    }

    public class IdsReader : IIdsReader
    {
        public IEnumerable<string> GetIds(Guid messageId, ChatEntities entities)
        {
            //var list = MessageStatsReader.GetMessageSentStats(entities, messageId);
            var stats = entities.MessageTransportId.Where(z => z.TransportTypeId == 100 && z.MsgId == messageId).Select(x => x.TransportId).ToArray();
            return stats;
        }
    }
}
