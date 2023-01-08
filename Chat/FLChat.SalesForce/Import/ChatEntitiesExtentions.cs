using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Import;
using FLChat.DAL.Model;
using FLChat.DAL.Utils;

namespace FLChat.SalesForce.Import
{
    public static class ChatEntitiesExtentions
    {
        public static ImportResult Import(this ChatEntities entities, DataTable data,
            List<Tuple<string, string>> clearedPhones,
            List<Tuple<string, string>> clearedEmails,
            List<string> missedOwner) {
            using (var opener = new ConnectionOpener(entities))
            using (var cmd = entities.Database.Connection.CreateCommand()) {
                cmd.Transaction = entities.Database.CurrentTransaction.UnderlyingTransaction;
                cmd.CommandText = "exec [Usr].[ImportUsersSalesForce] @table";
                cmd.Parameters.Add(new SqlParameter("@table", SqlDbType.Structured) {
                    TypeName = "[Usr].[ImportUsersSalesForceTable]",
                    Value = data,
                    Direction = ParameterDirection.Input
                });

                using (var reader = cmd.ExecuteReader()) {
                    if (!reader.Read())
                        throw new Exception("Reader has't return rows");

                    ImportResult result = ExtractResult(reader);

                    reader.NextResult();
                    ExtractList(reader, clearedPhones);

                    reader.NextResult();
                    ExtractList(reader, clearedEmails);

                    reader.NextResult();
                    ExtractList(reader, missedOwner);

                    return result;
                }
            }
        }

        private static ImportResult ExtractResult(DbDataReader reader) {
            return new ImportResult() {
                Updated = reader.GetInt32(0),
                Inserted = reader.GetInt32(1),
                ClearedPhone = reader.GetInt32(2),
                ClearedEmail = reader.GetInt32(3),
                OwnerUpdated = reader.GetInt32(4),
                MissedOwner = reader.GetInt32(5)
            };
        }

        private static void ExtractList(DbDataReader reader, List<Tuple<string, string>> list) {
            while (reader.Read()) {
                list.Add(Tuple.Create(
                    reader.GetString(0),
                    reader.GetString(1)
                    ));
            }
        }

        private static void ExtractList(DbDataReader reader, List<string> list) {
            while (reader.Read()) {
                list.Add(reader.GetString(0));
            }
        }

        /// <summary>
        /// Update segments members
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="users">table of [Usr].[ForeignIdList] type</param>
        /// <param name="segments">table of [Usr].[ForeignIdGuidList] type</param>
        /// <returns></returns>
        public static ImportSegmentResult ImportSegments(this ChatEntities entities, DataTable users, DataTable segments) {
            return entities.Database.SqlQuery<ImportSegmentResult>(
                "exec [Usr].[UpdateSegmentsBatchByForeignId] @users, @segments",
                new SqlParameter("@users", SqlDbType.Structured) {
                    TypeName = "[Usr].[ForeignIdList]",
                    Value = users,
                    Direction = ParameterDirection.Input
                },
                new SqlParameter("@segments", SqlDbType.Structured) {
                    TypeName = "[Usr].[ForeignIdGuidList]",
                    Value = segments,
                    Direction = ParameterDirection.Input
                }).Single();
        }
    }
}
