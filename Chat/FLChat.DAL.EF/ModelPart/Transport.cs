using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.SqlClient;

namespace FLChat.DAL.Model
{
    public partial class Transport : ITransport
    {
        public TransportKind Kind
        {
            get => (TransportKind)TransportTypeId;
            set => TransportTypeId = (int)value;
        }

        /// <summary>
        /// Change addressee for current transport
        /// Note: direct assign null to MsgAddressee does not clear the value
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="toAddr"></param>
        public void ChangeAddressee(ChatEntities entities, User toAddr) {
            if (toAddr == null || toAddr.Id == User.OwnerUserId)
                RemoveAddressee(entities); //transport.MsgAddressee = null;
            else
                MsgAddressee = toAddr;
        }

        public bool IsAddressee(User user) {
            if (user == null)
                return false;
            if ((MsgAddressee != null && MsgAddressee.Id == user.Id)
                || (MsgAddressee == null && User.OwnerUserId == user.Id))
                return true;
            return false;
        }

        private void RemoveAddressee(ChatEntities entities) {
            entities.Database.ExecuteSqlCommand(
                "delete from [Usr].[MsgAddressee] where [UserId] = @uid",
                new SqlParameter("@uid", User.Id));
        }
    }

    public static class TransportExtentions
    {
        /// <summary>
        /// Returns first transport of specific kind or null
        /// </summary>
        /// <param name="transports"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static Transport Get(this ICollection<Transport> transports, TransportKind kind)
            => transports.Where(t => t.TransportTypeId == (int)kind).FirstOrDefault();

        /// <summary>
        /// Make query for search transports by transport outer id
        /// </summary>
        /// <param name="transport"></param>
        /// <param name="transportKind"></param>
        /// <param name="transId"></param>
        /// <param name="include"></param>
        /// <param name="onlyEnabled"></param>
        /// <returns></returns>
        public static IQueryable<Transport> QueryTransportByOuterId(
            this DbSet<Transport> transport, 
            TransportKind transportKind, 
            string transId,
            Func<IQueryable<Transport>, IQueryable<Transport>> include = null, 
            bool onlyEnabled = true) {
            IQueryable<Transport> q = transport
                .Where(t =>
                    (t.Enabled || onlyEnabled == false)
                    && t.User.Enabled
                    && t.TransportTypeId == (int)transportKind
                    && t.TransportOuterId == transId);
            if (include != null)
                q = include(q);
            return q.OrderByDescending(t => t.Enabled).ThenBy(t => t.User.IsTemporary);
        }

        /// <summary>
        /// Search outer transport with external transport's id
        /// </summary>
        /// <param name="transport">Transport entities</param>
        /// <param name="transportKind">kind of transport</param>
        /// <param name="transId">external transport id</param>
        /// <returns>Transport or null</returns>
        public static Transport GetTransportByOuterId(
            this DbSet<Transport> transport, 
            TransportKind transportKind, 
            string transId, 
            Func<IQueryable<Transport>, IQueryable<Transport>> include = null, 
            bool onlyEnabled = true)
            => transport.QueryTransportByOuterId(transportKind, transId, include, onlyEnabled).FirstOrDefault();
    }
}
