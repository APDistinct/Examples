using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Reports
{
    public class MessageStatsReader
    {
        public static MessageSentStats[] GetMessageSentStats(ChatEntities entities, Guid messageId)
        {
            const string sql = "with [Frwd] as (select m.[Id], m.[ForwardMsgId], mtu.[ToUserId], mtu.[ToTransportTypeId] from[Msg].[Message] m " +
                " inner join[Msg].[MessageToUser] mtu on m.[Id] = mtu.[MsgId] where m.[ForwardMsgId] is not null) " +
                " select u.[FullName], u.[Phone], " +
                " sv.ToUserId, sv.ToTransportTypeId, sv.IsWebChat, sv.IsFailed, sv.IsSent, sv.IsQuequed, " +
                " sv.CantSendToWebChat, sv.IsWebChatAccepted, sv.IsSmsUrlOpened, " +
                " tid.[TransportTypeId], tid.[TransportId] " +
                " from [Msg].[MessageStatsRowsView] sv " +
                " inner join[Usr].[User] u on sv.[ToUserId] = u.[Id] " +
                " left join [Frwd] frwd on sv.[MsgId] = frwd.[ForwardMsgId] and sv.[ToUserId] = frwd.[ToUserId] " +
                " left join [Msg].[MessageTransportId] tid on tid.[MsgId] = coalesce(frwd.[Id], sv.[MsgId]) and tid.[ToUserId] = sv.[ToUserId] " +
                " where sv.[MsgId] = @msgid ";
            return entities.Database.SqlQuery<MessageSentStats>(sql, new SqlParameter("@msgid", messageId)).ToArray();
        }
    }
}
