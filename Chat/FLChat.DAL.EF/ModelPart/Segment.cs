using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Model
{
    public partial class Segment : IStructureNodeInfo
    {
        /// <summary>
        /// Get user from segment who contains in userId structure
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<User> GetMembersForUser(ChatEntities entities, Guid userId) {
            const string sql = "select * from [Usr].[User] where [Id] in (select [UserId] from [Usr].[Segment_GetMembers](@segmentId, @userId, default, default))";
            return entities.Database.SqlQuery<User>(sql,
                new SqlParameter("@segmentId", Id),
                new SqlParameter("@userId", userId))
                .ToList();
        }
        public const string Prefix = MainDef.PrefixSeg; // "seg-";

        public string NodeId => String.Concat(Prefix, Id.ToString()/*Name*/);

        public string Caption => Descr;
    }

    public static class SegmentExtentions
    {
        class UserSegmentTag
        {
            public Guid Guid { get; set; }
            public string Name { get; set; }
            public string Tag { get; set; }
        }

        /// <summary>
        /// Load segments name for users
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="users"></param>
        /// <returns>returns dictionary with segment names or null if has't found</returns>
        public static Dictionary<Guid, List<string>> LoadShortInfoSegments(
            this ChatEntities entities,
            IEnumerable<Guid> users) {
            Dictionary<Guid, List<string>> dictionary = null;
            LoadShortInfoSegments(entities, users, ref dictionary);
            return dictionary;
        }

        /// <summary>
        /// Load segments name for users
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="users"></param>
        /// <param name="dictionary"></param>
        public static void LoadShortInfoSegments(
            this ChatEntities entities, 
            IEnumerable<Guid> users,
            ref Dictionary<Guid, List<string>> dictionary) {

            if (users.Any() == false)
                return;

            const string sql =
                "select u.[Guid], s.[Name], s.[Tag] from [Usr].[Segment] s " +
                "inner join[Usr].[SegmentMember] sm on s.[Id] = sm.[SegmentId] " +
                "inner join @users u on sm.[UserId] = u.[Guid] " +
                "where s.IsDeleted = 0 and s.[ShowInShortProfile] = 1";
            UserSegmentTag[] pairs = entities.Database.SqlQuery<UserSegmentTag>(sql, users.CreateGuidListParameter("@users")).ToArray();
            if (pairs.Length > 0 && dictionary == null)
                dictionary = new Dictionary<Guid, List<string>>();

            foreach (UserSegmentTag p in pairs) {
                if (!dictionary.TryGetValue(p.Guid, out List<string> values)) {
                    values = new List<string>();
                    dictionary.Add(p.Guid, values);
                }
                values.Add(p.Tag ?? p.Name);
            }
        }
    }
}
