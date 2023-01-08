using FLChat.DAL.Model;
using FLChat.DAL.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FLChat.DAL.Import
{
    public static class ImportUsersFLBatchExtentions
    {       
        public static void AdjustPhoneNumbers(this DataTable dt) {
            int colNumber = dt.Columns.IndexOf("MOBILE");
            Regex regEx = new Regex(@"\D");
            for (int i = 0; i < dt.Rows.Count; ++i) {
                object val = dt.Rows[i][colNumber];
                if ((val is DBNull) == false)
                    dt.Rows[i][colNumber] = regEx.Replace((string)val, "");
            }
        }

        public static ImportResult Import(this ChatEntities entities, DataTable data,
            List<Tuple<int, string>> clearedPhones, 
            List<Tuple<int, string>> clearedEmails,
            List<int> missedOwner) {
            using (var opener = new ConnectionOpener(entities))
            using (var cmd = entities.Database.Connection.CreateCommand()) {
                cmd.Transaction = entities.Database.CurrentTransaction.UnderlyingTransaction;
                cmd.CommandText = "exec [Usr].[UpdateFLUsersBatch] @table";
                cmd.Parameters.Add(new SqlParameter("@table", SqlDbType.Structured) {
                    TypeName = "[Usr].[UserFLImportTable]",
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

                //return entities.Database.SqlQuery<Result>(
                //"exec [Usr].[UpdateFLUsersBatch] @table",
                //new SqlParameter("@table", SqlDbType.Structured) {
                //   TypeName = "[Usr].[UserFLImportTable]",
                //    Value = data,
                //    Direction = ParameterDirection.Input
                //}).Single();
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

        private static void ExtractList(DbDataReader reader, List<Tuple<int, string>> list) {
            while (reader.Read()) {
                list.Add(Tuple.Create(
                    reader.GetInt32(0),
                    reader.GetString(1)
                    ));
            }
        }

        private static void ExtractList(DbDataReader reader, List<int> list) {
            while (reader.Read()) {
                list.Add(reader.GetInt32(0));
            }
        }
    }
}
