using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.DataTypes;
using FLChat.DAL.Utils;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;

namespace FLChat.DAL.Model
{
    public partial class StructureNode : IStructureNodeInfo
    {
        public const string Prefix = MainDef.PrefixNod;  //"nod-";

        public string NodeId => String.Concat(Prefix, Id.ToString());

        public string Caption => Name;
    }

    public static class StructureNodeExtentions
    {
        /// <summary>
        /// Read info about node
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sids"></param>
        public static StructureNodeFullInfo ExecuteStructureNode_GetInfo(this ChatEntities entities, string sid, Guid userId,
            int? offset, int? count) {
            List<StructureNodeVirtual> nodes;
            List<User> users;
            StructureNodeVirtual node;

            using (var opener = new ConnectionOpener(entities)) {

                using (var cmd = entities.Database.Connection.CreateCommand()) {
                    cmd.Transaction = entities.Database.CurrentTransaction?.UnderlyingTransaction;
                    cmd.CommandText = "exec [Ui].[StructureNode_GetInfo] @sid, @userId, @offset, @count";
                    cmd.Parameters.Add(new SqlParameter("@userId", userId));
                    cmd.Parameters.Add(new SqlParameter("@sid", sid ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@offset", offset ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@count", count ?? (object)DBNull.Value));

                    // Run the sproc
                    using (var reader = cmd.ExecuteReader()) {
                        if (!reader.Read())
                            return null;

                        node = new StructureNodeVirtual {
                            Id = reader.GetString(Fields.Id),
                            Name = reader.GetString(Fields.Name),
                            Count = reader.IsDBNull(Fields.Count) ? 0 : reader.GetInt32(Fields.Count)
                        };

                        // Move to second result set and read Nodes
                        reader.NextResult();

                        // Read Users from the first result set

                        nodes = new List<StructureNodeVirtual>();
                        while (reader.Read()) {
                            StructureNodeVirtual xnode = new StructureNodeVirtual {
                                Id = reader.GetString(Fields.Id),
                                Name = reader.GetString(Fields.Name),
                                Count = reader.IsDBNull(Fields.Count) ? 0 : reader.GetInt32(Fields.Count),
                                Final = reader.GetBoolean(Fields.Final),
                            };
                            nodes.Add(xnode);
                        }
                        node.Final = nodes.Count == 0;

                        // Move to second result set and read Users
                        reader.NextResult();

                        var xusers = ((IObjectContextAdapter)entities)
                            .ObjectContext
                            .Translate<User>(reader, "User", MergeOption.AppendOnly);
                        users = xusers.ToList();

                        int? totalCount = null;
                        reader.NextResult();
                        if (reader.Read()) {
                            totalCount = reader.GetInt32(0);
                        }

                        return new StructureNodeFullInfo(node, nodes, users, totalCount);
                    }
                }
            }

        }

        /// <summary>
        /// DB fields
        /// </summary>
        private static class Fields
        {
            public const int Id = 0;
            public const int Name = 1;
            public const int Count = 2;
            public const int Final = 3;
        }

        public static void Update_StructureNodeCount(this ChatEntities entities, Guid userId) {
            using (var opener = new ConnectionOpener(entities))
            using (var cmd = entities.Database.Connection.CreateCommand()) {
                cmd.CommandTimeout = 60 * 10;   //10 minutes
                cmd.Transaction = entities.Database.CurrentTransaction.UnderlyingTransaction;
                cmd.CommandText = "exec [Cache].[Update_StructureNodeCount] @userId";
                cmd.Parameters.Add(new SqlParameter("@userId", userId));
                cmd.ExecuteNonQuery();
            }
        }
    }
}
