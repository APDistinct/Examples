using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Model
{
    public partial class LastMessageView
    {
    }

    public static class LastMessageViewExtentions
    {
        /// <summary>
        /// Load last messages for multiple chats with specific users
        /// </summary>
        /// <param name="entities">database entities</param>
        /// <param name="userId">one of opponents in chat</param>
        /// <param name="oppUsers">users for whom load last messages in chat with <paramref name="userId"/>user</param>
        /// <returns></returns>
        public static Dictionary<Guid, MessageToUser> GetLastMessages(this ChatEntities entities, Guid userId, IEnumerable<Guid> oppUsers) {
            //load last messages id
            long[] MessageToUserIdx = entities
                .LastMessageView
                .Where(lmv => lmv.UserId == userId && oppUsers.Contains(lmv.UserOppId))
                .Select(lmv => lmv.MsgToUserIdx)
                .ToArray();

            MessageToUser[] messages = entities
                .MessageToUser
                .Where(mtu => MessageToUserIdx.Contains(mtu.Idx))
                .Include(mtu => mtu.Message)
                .ToArray();

            Dictionary<Guid, MessageToUser> map = new Dictionary<Guid, MessageToUser>(messages.Count());
            foreach (MessageToUser msg in messages) {
                if (msg.ToUserId == userId)
                    map[msg.Message.FromUserId] = msg;
                else
                    map[msg.ToUserId] = msg;
            }

            return map;
        }

        public static LastMessageView[] GetLastMessageViewForContact(this ChatEntities entities, Guid userId) {
            const string sql = "select * from [Msg].[LastMessageView] "
                + " where[UserId] = @userId "
                + " and[UserId] != [UserOppId] "
                + " order by[MsgToUserIdx] desc";
            return entities.Database.SqlQuery<LastMessageView>(sql, new SqlParameter("@userId", userId)).ToArray();
        }

        public static int GetLastMessageViewCountForContact(this ChatEntities entities, Guid userId) {
            const string sql = "select count(*) from [Msg].[LastMessageView] "
                + " where[UserId] = @userId "
                + " and[UserId] != [UserOppId] ";
            return entities.Database.SqlQuery<int>(sql, new SqlParameter("@userId", userId)).Single();
        }

        public static LastMessageView[] GetLastMessageViewForContact(this ChatEntities entities, Guid userId, int offset, int Count) {
            const string sql =
                  "select * from [Msg].[LastMessageView] "
                + " where [UserId] = @userId "
                + " and [UserId] != [UserOppId] "
                + " order by case when [UnreadCnt] > 0 then 1 else 0 end desc, [MsgToUserIdx] desc"
                + " OFFSET @offset ROWS FETCH NEXT @cnt ROWS ONLY";
            return entities.Database.SqlQuery<LastMessageView>(sql,
                new SqlParameter("@userId", userId),
                new SqlParameter("@offset", offset),
                new SqlParameter("@cnt", Count)).ToArray();
        }
    }
}
