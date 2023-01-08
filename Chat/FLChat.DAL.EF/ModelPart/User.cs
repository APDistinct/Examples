using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Utils;
using FLChat.DAL.DataTypes;

namespace FLChat.DAL.Model
{
    public partial class User : IUserAuthInfo
    {
        public Guid UserId => Id;

        /// <summary>
        /// List of available transport's kind
        /// </summary>
        public IEnumerable<TransportKind> AvailableTransports => GetTransports();
        /// <summary>
        /// List of available segment's names
        /// </summary>
        public IEnumerable<string> SegmentNames => GetSegments();

        private IEnumerable<TransportKind> GetTransports()
        {
            return Transports
                .Where(t => t.Enabled && t.TransportType.Enabled && t.TransportType.VisibleForUser)
                .Select(t => t.TransportTypeId)
                .ToList()
                .Cast<TransportKind>();
        }

        private IEnumerable<string> GetSegments()
        {
            return Segments
                .Where(t => !t.IsDeleted /*&& t.Members.Where(x => x.UserId == UserId).Any()*/)
                .Select(t => t.Name)
                .ToList();
        }

        /// <summary>
        /// returns true if user is sentry
        /// </summary>
        public bool IsSentry => UserSentry != null;
    }

    public static class UserChatEntitiesExtentions
    {
        /// <summary>
        /// Returns LastDelivered event id for user
        /// </summary>
        /// <param name="entities">Database entities</param>
        /// <param name="userId">user id</param>
        /// <returns>last delivered event</returns>
        public static long? LastDeliveryEventId(this ChatEntities entities, Guid userId) {
            const string sql = "select [LastEventId] from [Msg].[EventDelivered] where [UserId] = @userId";
            return entities.Database.SqlQuery<long?>(sql, new SqlParameter("@userId", userId)).FirstOrDefault();
        }

        public static Dictionary<Guid, TransportKind?> GetUserTransport(this ChatEntities entities, IEnumerable<Guid> ids)
        {
            var dic = new Dictionary<Guid, TransportKind?>();

            bool needClose = false;            
            if (entities.Database.Connection.State == ConnectionState.Closed)
            {
                entities.Database.Connection.Open();
                needClose = true;
            }
            using (var cmd = entities.Database.Connection.CreateCommand())
            {
                string paramname = "@userid";
         
                SqlParameter parameter = ids.CreateGuidListParameter(paramname);
                cmd.CommandText = $"select u.[Guid], t.DefaultTransportTypeId from {paramname} u " +
                    "left join[Usr].[UserDefaultTransportView] t on t.UserId = u.Guid";
                cmd.Transaction = entities.Database.CurrentTransaction?.UnderlyingTransaction;
                
                cmd.Parameters.Add(parameter);
                using (var reader = cmd.ExecuteReader( ))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            dic.Add(reader.GetGuid(Fields.Guid), 
                           reader.IsDBNull(Fields.TypeId) ? null : (TransportKind?)reader.GetInt32(Fields.TypeId));
                        }
                    }
                }
            }
            if (needClose)
            {
                entities.Database.Connection.Close();
            }
            return dic;
            
        }

        public static Dictionary<Guid, TransportKind?> GetUserMailingTransport(this ChatEntities entities, IEnumerable<Guid> ids)
        {
            var dic = new Dictionary<Guid, TransportKind?>();

            bool needClose = false;
            if (entities.Database.Connection.State == ConnectionState.Closed)
            {
                entities.Database.Connection.Open();
                needClose = true;
            }
            using (var cmd = entities.Database.Connection.CreateCommand())
            {
                string paramname = "@userid";

                SqlParameter parameter = ids.CreateGuidListParameter(paramname);
                cmd.CommandText = $"select u.[Guid], t.DefaultTransportTypeId from {paramname} u " +
                    "left join [Usr].[UserMailingTransportView] t on t.UserId = u.Guid";
                cmd.Transaction = entities.Database.CurrentTransaction?.UnderlyingTransaction;

                cmd.Parameters.Add(parameter);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            dic.Add(reader.GetGuid(Fields.Guid),
                           reader.IsDBNull(Fields.TypeId) ? null : (TransportKind?)reader.GetInt32(Fields.TypeId));
                        }
                    }
                }
            }
            if (needClose)
            {
                entities.Database.Connection.Close();
            }
            return dic;
        }

        /// <summary>
        /// DB fields
        /// </summary>
        private static class Fields
        {
            public const int Guid = 0;
            public const int TypeId = 1;
        }

        /// <summary>
        /// Merge two users
        /// </summary>
        /// <param name="chatEntities"></param>
        /// <param name="master"></param>
        /// <param name="donor"></param>
        //static public List<Guid> MergeUsers(this ChatEntities chatEntities, Guid master, Guid donor) {
        //    return chatEntities.Database.SqlQuery<Guid>("EXEC @return_value = [Usr].[MergeUsers] @master = @master, @donor = @donor",
        //        new SqlParameter("@master", master),
        //        new SqlParameter("@donor", donor)).ToList();
        //}

        static public User[] GetAddresseesForExternalTrans(this ChatEntities entities, Guid userId) {
            const string sql = "select u.* "
                + "from[Usr].[User_GetAddresseesForExternalTrans](@uid) a "
                + "inner join[Usr].[User] u on a.[UserId] = u.[Id] "
                + "order by a.[Order] asc, a.[SubOrder] desc";
            return entities.Database.SqlQuery<User>(sql, new SqlParameter("@uid", userId)).ToArray();
            //return entities
            //    .User_GetAddresseesForExternalTrans(userId)
            //    .Join(entities.User, g => g.UserId, u => u.Id, (g, u) => new Tuple<User, User_GetAddresseesForExternalTrans_Result>() { })
            //    .OrderBy(;
        }

        /// <summary>
        /// Order list of user: first cyrrilic symbols, then latin
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        static public IOrderedQueryable<User> OrderByName(this IQueryable<User> q) =>
            q.OrderByDescending(u => u.FullName != null ? 1 : 0)
            .ThenByDescending(u => SqlFunctions.Unicode(u.FullName) >= 1024 && SqlFunctions.Unicode(u.FullName) <= 1279 ? 1 : 0)
            .ThenBy(u => u.FullName);


        /// <summary>
        /// List of user: search string in FullName, Phone, Email or FLUserNumber is equal
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        static public IQueryable<User> FindString(this IQueryable<User> q, string searchString) {
            String like = String.Concat("%", searchString, "%");
            int? flUserNumber = null;
            if (int.TryParse(searchString, out int number))
                flUserNumber = number;
            var qUsers = q.Where(u =>
                       DbFunctions.Like(u.FullName, like)
                    || DbFunctions.Like(u.Phone, like)
                    || DbFunctions.Like(u.Email, like)
                    || (flUserNumber != null && u.FLUserNumber == flUserNumber.Value && u.FLUserNumber != null)
                    );

            return qUsers;
        }

        /// <summary>
        /// Calculate count of users in user's structure without structure owner
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="userId"></param>
        /// <param name="selection">information about selection</param>
        /// <returns></returns>
        static public int UserSelectionCount(
            this ChatEntities entities,
            Guid userId,
            UserSelection selection) {
            const string sql =
                "select count(*) from [Usr].[User_GetSelection](@userId, @include_ws, @exclude_ws, @include, @exclude, @segments, default)";
            return entities.Database.SqlQuery<int>(
                sql,
                new SqlParameter("@userId", userId),
                selection.IncludeWithStructure.CreateUserIdDeepParameter("@include_ws"),
                selection.ExcludeWithStructure.CreateGuidListParameter("@exclude_ws"),
                selection.Include.CreateGuidListParameter("@include"),
                selection.Exclude.CreateGuidListParameter("@exclude"),
                selection.Segments.CreateGuidListParameter("@segments")
                ).Single();
        }

        /// <summary>
        /// Insert records to [Msg].[MessageToUser] based on user's selection
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="msgId"></param>
        /// <param name="userId"></param>
        /// <param name="selection">information about selection</param>
        /// <param name="defaultTransportView"></param>
        static public void UserSelectionToMessageToUser(
            this ChatEntities entities,
            Guid msgId,
            Guid userId,
            UserSelection selection,
            string defaultTransportView) {

            string sql =
                "insert into [Msg].[MessageToUser] ([MsgId], [ToUserId], [ToTransportTypeId], [IsSent]) " +
                "select " +
                    "@msgId, t.[UserId], t.[DefaultTransportTypeId], " +
                    "case when t.[DefaultTransportTypeId] = /**FLChat**/0 then 1 else 0 end " +
                "from [Usr].[User_GetSelection](@userId, @include_ws, @exclude_ws, @include, @exclude, @segments, default) s " +
                "inner join " + defaultTransportView + " t on s.[UserId] = t.[UserId]";
            entities.Database.ExecuteSqlCommand(
                sql,
                new SqlParameter("@msgId", msgId),
                new SqlParameter("@userId", userId),
                selection.IncludeWithStructure.CreateUserIdDeepParameter("@include_ws"),
                selection.ExcludeWithStructure.CreateGuidListParameter("@exclude_ws"),
                selection.Include.CreateGuidListParameter("@include"),
                selection.Exclude.CreateGuidListParameter("@exclude"),
                selection.Segments.CreateGuidListParameter("@segments"));
        }

        /// <summary>
        /// Insert records to [Msg].[MessageToUser] based on user's selection
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="msgId"></param>
        /// <param name="userId"></param>
        /// <param name="selection">information about selection</param>
        /// <param name="defaultTransportView"></param>
        static public int MessageCountForUserSelection(
            this ChatEntities entities,
            Guid userId,
            UserSelection selection,
            string defaultTransportView) {

            string sql =
                "select count(*)" +
                "from [Usr].[User_GetSelection](@userId, @include_ws, @exclude_ws, @include, @exclude, @segments, default) s " +
                "inner join " + defaultTransportView + " t on s.[UserId] = t.[UserId]";
            return entities.Database.SqlQuery<int>(
                sql,
                new SqlParameter("@userId", userId),
                selection.IncludeWithStructure.CreateUserIdDeepParameter("@include_ws"),
                selection.ExcludeWithStructure.CreateGuidListParameter("@exclude_ws"),
                selection.Include.CreateGuidListParameter("@include"),
                selection.Exclude.CreateGuidListParameter("@exclude"),
                selection.Segments.CreateGuidListParameter("@segments")).Single();
        }

        /// <summary>
        /// Insert records to [Msg].[MessageToUser] based on user's selection
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="msgId"></param>
        /// <param name="userId"></param>
        /// <param name="selection">information about selection</param>
        /// <param name="defaultTransportView"></param>
        static public int MessageCountForUserSelection(
            this ChatEntities entities,
            Guid userId,
            UserSelection selection,
            MessageKind messageKind) {
            return MessageCountForUserSelection(entities, userId, selection,
                messageKind.DefaultTransportViewName());
        }

        /// <summary>
        /// returns set of users, which containts in structure of broadcast prohibited users
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="userId"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        static public HashSet<Guid> GetBroadcastProhibitionStructure(
            this ChatEntities entities, 
            Guid userId, 
            IEnumerable<Guid> users) {
            const string sql = "select [UserId] from [Usr].[BroadcastProhibition_StructureUpward](@userId, @ids)";
            return new HashSet<Guid>(entities.Database.SqlQuery<Guid>(
                sql,
                new SqlParameter("@userId", userId),
                users.CreateGuidListParameter("@ids"))
                .ToArray());
            //return new HashSet<Guid>(entities
            //    .BroadcastProhibition_Structure(userId)
            //    .Where(g => users.Contains(g.Value))
            //    .Select(g => g.Value)
            //    .ToArray());
        }

        /// <summary>
        /// return HasSet of users' id which has any child
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        static public HashSet<Guid> GetHasChilds(
            this ChatEntities entities,
            IEnumerable<Guid> users) {
            return new HashSet<Guid>(entities
                .User
                .Where(u => users.Contains(u.Id) && u.ChildUsers.Any() && u.Enabled)
                .Select(u => u.Id)
                .ToArray());
        }

        /// <summary>
        /// Update segments for user
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="userId"></param>
        /// <param name="flUserNumber"></param>
        /// <param name="segments"></param>
        static public void User_UpdateSegments(
            this ChatEntities entities,
            Guid? userId,
            int? flUserNumber,
            IEnumerable<Guid> segments) {
            entities.Database.ExecuteSqlCommand(
                "exec [Usr].[User_UpdateSegments] @userId, @segments, @userFLNumber",
                new SqlParameter("@userId", userId.HasValue ? userId.Value : (object)DBNull.Value),
                segments.CreateGuidListParameter("@segments"),
                new SqlParameter("@userFLNumber", flUserNumber.HasValue ? flUserNumber.Value : (object)DBNull.Value)
                );
        }
    }
}
