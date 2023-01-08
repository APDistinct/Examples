using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Import;

namespace FLChat.SalesForce.DataTypes
{
    public static class ContactExtentions
    {
        /// <summary>
        /// Convert list of sales force data type <seealso cref="Contact"/> 
        /// to SQL data type [Usr].[ImportUsersSalesForceTable] for importing
        /// </summary>
        /// <param name="list">list of SalesForce Contacts</param>
        /// <returns>Data table of [Usr].[ImportUsersSalesForceTable] sql type</returns>
        public static DataTable ToImportUsersSalesForceTable(this IEnumerable<Contact> list) {
            DataTable dt = CreateTable();
            int index = 0;
            foreach (var item in list) {
                dt.AddRow(item, index);

                index += 1;
            }

            return dt;
        }

        /// <summary>
        /// Create tables for SQL procedure [Usr].[UpdateSegmentsBatchByForeignId]
        /// </summary>
        /// <param name="list">list of contacts</param>
        /// <param name="segmentNameResolver">return segment id by segment partner name</param>
        /// <param name="users">resulting table of [Usr].[ForeignIdList] type</param>
        /// <param name="segments">resulting table of [Usr].[ForeignIdGuidList] type</param>
        public static void CreateImportSegmentsTables(
            this IEnumerable<Contact> list,
            Func<string, Guid> segmentNameResolver,
            out DataTable users,
            out DataTable segments) {

            users = CreateForeignIdListTable();
            segments = CreateForeignIdGuidListTable();
            foreach (var item in list) {
                DataRow userRow = users.NewRow();
                userRow[0] = item.Id;
                users.Rows.Add(userRow);

                if (item.Segments != null) {
                    foreach (string s in item.ParseSegments()) {
                        DataRow segRow = segments.NewRow();
                        segRow[0] = item.Id;
                        segRow[1] = segmentNameResolver(s);
                        segments.Rows.Add(segRow);
                    }
                }
            }
        }

        /// <summary>
        /// Create new row with data from <paramref name="item"/> and add to table <paramref name="dt"/>
        /// </summary>
        /// <param name="dt">Data table</param>
        /// <param name="item">Contact item</param>
        /// <param name="index">current index</param>
        private static void AddRow(this DataTable dt, Contact item, int index) {
            DataRow row = dt.NewRow();
            row[0] = item.LastName ?? (object)DBNull.Value;
            row[1] = item.FirstName ?? (object)DBNull.Value;
            row[2] = DBNull.Value;
            row[3] = item.Birthdate ?? (object)DBNull.Value;
            row[4] = item.Phone?.RemoveNonDigits() ?? (object)DBNull.Value;
            row[5] = item.Email ?? (object)DBNull.Value;
            row[6] = item.Title ?? (object)DBNull.Value;
            row[7] = item.Country ?? (object)DBNull.Value;
            row[8] = item.Region ?? (object)DBNull.Value;
            row[9] = item.City ?? (object)DBNull.Value;
            row[10] = item.CreatedDate;
            row[11] = item.EmailPermission ?? (object)DBNull.Value;
            row[12] = item.SMSPermission ?? (object)DBNull.Value;
            row[13] = item.LastOrderDate ?? (object)DBNull.Value;
            row[14] = item.BonusPoints ?? (object)DBNull.Value;
            row[15] = index;
            row[16] = item.Id;
            row[17] = item.ReportsToId ?? (object)DBNull.Value;
            row[18] = !item.IsDeleted;
            dt.Rows.Add(row);
        }

        /// <summary>
        /// Create DataTable for SQL table-type [Usr].[ImportUsersSalesForceTable]
        /// </summary>
        /// <returns></returns>
        private static DataTable CreateTable() {
            DataTable dt = new DataTable();
            dt.Columns.Add("SURNAME", typeof(string));  //0
            dt.Columns.Add("NAME", typeof(string));     //1
            dt.Columns.Add("PATRONYMIC", typeof(string));//2
            dt.Columns.Add("BIRTHDAY", typeof(DateTime));//3
            dt.Columns.Add("MOBILE", typeof(string));   //4
            dt.Columns.Add("EMAIL", typeof(string));    //5
            dt.Columns.Add("TITLE", typeof(string));    //6
            dt.Columns.Add("COUNTRY", typeof(string));  //7
            dt.Columns.Add("REGION", typeof(string));   //8
            dt.Columns.Add("CITY", typeof(string));     //9
            dt.Columns.Add("REGISTRATIONDATE", typeof(DateTime));//10
            dt.Columns.Add("EMAILPERMISSION", typeof(bool));//11
            dt.Columns.Add("SMSPERMISSION", typeof(bool));  //12
            dt.Columns.Add("LASTORDERDATE", typeof(DateTime));  //13
            dt.Columns.Add("FLCLUBPOINTS", typeof(decimal));    //14
            dt.Columns.Add("ROWNUMBER", typeof(int));   //15
            dt.Columns.Add("ForeignID", typeof(string));    //16
            dt.Columns.Add("ForeignOwnerID", typeof(string));   //17
            dt.Columns.Add("Enabled", typeof(bool));    //18
            return dt;
        }

        /// <summary>
        /// Create DataTable for SQL table-type [Usr].[ForeignIdList]
        /// </summary>
        /// <returns></returns>
        private static DataTable CreateForeignIdListTable() {
            DataTable dt = new DataTable();
            dt.Columns.Add("ForeignId", typeof(string));
            return dt;
        }

        /// <summary>
        /// Create DataTable for SQL table-type [Usr].[ForeignIdGuidList]
        /// </summary>
        /// <returns></returns>
        private static DataTable CreateForeignIdGuidListTable() {
            DataTable dt = new DataTable();
            dt.Columns.Add("ForeignId", typeof(string));
            dt.Columns.Add("Guid", typeof(Guid));
            return dt;
        }

        /// <summary>
        /// Return array of segments or null
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public static string[] ParseSegments(this Contact contact) {
            if (contact.Segments != null)
                return contact.Segments.Split(';');
            else
                return null;
        }
    }
}
