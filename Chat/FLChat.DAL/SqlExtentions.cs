using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL
{
    public static class SqlExtentions
    {
        /// <summary>
        /// Create SqlParameter for [dbo].[GuidList] type
        /// </summary>
        /// <param name="sqlParamName">sql parameter name</param>
        /// <param name="ids">list of guids</param>
        /// <returns>created sql parameter</returns>
        public static SqlParameter CreateGuidListParameter(string sqlParamName, IEnumerable<Guid> ids) {
            SqlParameter parameter = new SqlParameter(sqlParamName, SqlDbType.Structured) {
                TypeName = "[dbo].[GuidList]",
                Value = ids.CreateDataTable<Guid>("Guid"),
                Direction = ParameterDirection.Input
            };

            return parameter;
        }

        /// <summary>
        /// Create SqlParameter for [dbo].[IntList] type
        /// </summary>
        /// <param name="sqlParamName">sql parameter name</param>
        /// <param name="list">list of int</param>
        /// <returns>created sql parameter</returns>
        public static SqlParameter CreateIntListParameter(string sqlParamName, IEnumerable<int> list) {
            return new SqlParameter(sqlParamName, SqlDbType.Structured) {
                TypeName = "[dbo].[IntList]",
                Value = list.CreateDataTable<int>("Value"),
                Direction = ParameterDirection.Input
            };
        }

        /// <summary>
        /// Create DataTable with single column
        /// </summary>
        /// <typeparam name="T">Field data type</typeparam>
        /// <param name="ids">list of values</param>
        /// <param name="field">Field name</param>
        /// <returns>Created DataTable</returns>
        public static DataTable CreateDataTable<T>(this IEnumerable<T> ids, string field) {
            var table = new DataTable();
            table.Columns.Add(field, typeof(T));

            if (ids != null) {
                foreach (var id in ids)
                    table.Rows.Add(id);
            }

            return table;
        }

        /// <summary>
        /// Create SqlParameter for [dbo].[GuidList] type
        /// </summary>
        /// <param name="sqlParamName">sql parameter name</param>
        /// <param name="ids">list of guids</param>
        /// <returns>created sql parameter</returns>
        public static SqlParameter CreateGuidListParameter( this IEnumerable<Guid> ids, string sqlParamName)
        {
            return CreateGuidListParameter(sqlParamName, ids);
        }

        /// <summary>
        /// Create SqlParameter for [dbo].[UserIdDeep] type
        /// </summary>
        /// <param name="sqlParamName">sql parameter name</param>
        /// <param name="ids">list of guids</param>
        /// <returns>created sql parameter</returns>
        public static SqlParameter CreateUserIdDeepParameter(this IEnumerable<Tuple<Guid, int?>> ids, string sqlParamName) {
            SqlParameter parameter = new SqlParameter(sqlParamName, SqlDbType.Structured) {
                TypeName = "[Usr].[UserIdDeep]",
                Value = ids.CreateUserIdDeepDT(),
                Direction = ParameterDirection.Input
            };

            return parameter;
        }

        /// <summary>
        /// Create DataTable for type [Usr].[UserIdDeep]
        /// </summary>
        /// <typeparam name="T">Field data type</typeparam>
        /// <param name="ids">list of values</param>
        /// <param name="field">Field name</param>
        /// <returns>Created DataTable</returns>
        public static DataTable CreateUserIdDeepDT(this IEnumerable<Tuple<Guid, int?>> ids) {
            var table = new DataTable();
            table.Columns.Add("UserId", typeof(Guid));
            table.Columns.Add("Deep", typeof(int));

            if (ids != null) {
                foreach (var id in ids)
                    table.Rows.Add(id.Item1, id.Item2 ?? (object)DBNull.Value);
            }

            return table;
        }
    }
}
