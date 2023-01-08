using FLChat.Core;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.DAL.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core
{
    public interface IStatusSaver
    {
        void Save(MessageStats[] dms, ChatEntities entities);
    }

    public class StatusSaver : IStatusSaver
    {
        public void Save(MessageStats[] dms, ChatEntities entities)
        {
            using (var trans = entities.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                entities.SaveStats(dms.ToStatsSaveTable());
                trans.Commit();
            }
        }

        //foreach (var dm in dms)
        //{
        //    try
        //    {
        //        var mess = entities.MessageTransportId.Where(x => x.TransportId == dm.TransportId && x.TransportTypeId == (int)TransportKind.WebChat)
        //            .Include(m => m.MessageToUser.WebChatDeepLink).FirstOrDefault();
        //        if (mess != null)
        //        {
        //            var wcdl = //entities.WebChatDeepLink.Where(x => x.MsgId == mess.MsgId && x.ToUserId == mess.ToUserId).FirstOrDefault();
        //              mess.MessageToUser.WebChatDeepLink?.FirstOrDefault();
        //            //mess?.TransportType?.WebChatDeepLink?.FirstOrDefault();
        //            if (wcdl != null)
        //            {
        //                //if (dm.Update)
        //                {
        //                    wcdl.SentTo = dm.SentTo;
        //                    //if(wcdl.SmsStatus < dm.SmsStatus)
        //                    //    wcdl.SmsStatus = dm.SmsStatus;
        //                    wcdl.SmsStatus = Replace(wcdl.SmsStatus, dm.SmsStatus);
        //                    //if (wcdl.ViberStatus < dm.ViberStatus)
        //                    //    wcdl.ViberStatus = dm.ViberStatus;
        //                    wcdl.ViberStatus = Replace(wcdl.ViberStatus, dm.ViberStatus);
        //                    wcdl.UpdatedTime = dm.UpdatedTime;
        //                }
        //                //wcdl.WebFormRequested = dm.WebFormRequested;
        //                wcdl.IsFinished = dm.IsFinished;
        //                //entities.SaveChanges();
        //            }
        //        }
        //    }
        //    catch
        //    { }
        //}


        //public int? Replace(int? oldV, int? newV)
        //{
        //    var ret = oldV;
        //    if (newV != null && (oldV == null || oldV.Value < newV.Value))
        //        ret = newV;
        //    return ret;
        //}

    }

    public static class MessageStatsExtentions
    {
        /// <summary>
        /// <param name="list">list of MessageStats</param>
        /// <returns>Data table of [Msg].[DeepLinkStatsType] sql type</returns>
        /// </summary>
        public static DataTable ToStatsSaveTable(this IEnumerable<MessageStats> list)
        {
            DataTable dt = CreateTable();
            foreach (var item in list)
            {
                dt.AddRow(item);
            }
            return dt;
        }

        /// <summary>
        /// Create new row with data from <paramref name="item"/> and add to table <paramref name="dt"/>        
        /// <param name="dt">Data table</param>
        /// <param name="item">MessageStats item</param>
        /// </summary>
        private static void AddRow(this DataTable dt, MessageStats item)
        {
            DataRow row = dt.NewRow();
            row[0] = item.TransportId ?? (object)DBNull.Value;
            row[1] = item.SentTo ?? (object)DBNull.Value;
            row[2] = item.ViberStatus ?? (object)DBNull.Value;
            row[3] = item.SmsStatus ?? (object)DBNull.Value;
            row[4] = item.UpdatedTime ?? (object)DBNull.Value;
            row[5] = item.IsFinished;            
            dt.Rows.Add(row);
        }

        /// <summary>
        /// Create DataTable for SQL table-type [Usr].[ImportUsersSalesForceTable]
        /// </summary>
        /// <returns></returns>
        private static DataTable CreateTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TransportId", typeof(string));   //0
            dt.Columns.Add("SentTo", typeof(int));           //1
            dt.Columns.Add("ViberStatus", typeof(int));      //2
            dt.Columns.Add("SmsStatus", typeof(int));        //3
            dt.Columns.Add("UpdatedTime", typeof(DateTime)); //4
            dt.Columns.Add("IsFinished", typeof(bool));      //5            
            return dt;
        }

    }

    public static class ChatEntitiesExtentions
    {
        public static void SaveStats(this ChatEntities entities, DataTable data)
        {
            using (var opener = new ConnectionOpener(entities))
            using (var cmd = entities.Database.Connection.CreateCommand())
            {
                cmd.Transaction = entities.Database.CurrentTransaction.UnderlyingTransaction;
                cmd.CommandText = "exec [Msg].[UpdateWebChatDeepLinkStats] @table";
                cmd.Parameters.Add(new SqlParameter("@table", SqlDbType.Structured)
                {
                    TypeName = "[Msg].[DeepLinkStatsType]",
                    Value = data,
                    Direction = ParameterDirection.Input
                });
                cmd.ExecuteNonQuery();                
            }
        }
    }
}
