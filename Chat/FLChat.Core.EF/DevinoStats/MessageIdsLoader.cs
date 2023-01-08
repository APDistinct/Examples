using FLChat.DAL;
using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core
{
    public interface IMessageIdsLoader
    {
        string[] Load(ChatEntities entities);
    }

    public class MessageIdsLoader : IMessageIdsLoader
    {
        public string[] Load(ChatEntities entities)
        {
            //var list = MessageStatsReader.GetMessageSentStats(entities, messageId);
            //  Replace - select * from [FLChat].[Msg].[TransportIdStatsNotFinishedView]

            string sql = "select * from [Msg].[TransportIdStatsNotFinishedView]";
                //"SELECT[TransportId] " +
                //"FROM[Msg].[MessageTransportId] mt " +
                //"INNER JOIN[Msg].[WebChatDeepLink] wcdl on(wcdl.MsgId = mt.MsgId and wcdl.ToUserId = mt.ToUserId) " +
                //"WHERE wcdl.IsFinished = 0";

            return entities.Database.SqlQuery<string>(sql).ToArray();

            //    var stats = entities.MessageTransportId.Where(z => z.TransportTypeId == (int)TransportKind.WebChat).Select(x => x.TransportId).ToArray();
            //return stats;
        }
    }
}
