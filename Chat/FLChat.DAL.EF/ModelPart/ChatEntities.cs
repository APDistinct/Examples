using FLChat.DAL.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Model
{
    public partial class ChatEntities
    {
        private User _systemBot = null;
        private Transport _systemBotTransport = null;

        /// <summary>
        /// System bot
        /// </summary>
        public User SystemBot {
            get {
                if (_systemBot == null)
                    _systemBot = User.Where(u => u.Id == Global.SystemBotId).Single();
                return _systemBot;
            }
        }        

        /// <summary>
        /// System bot FLChat transport
        /// </summary>
        public Transport SystemBotTransport {
            get {
                if (_systemBotTransport == null)
                    _systemBotTransport = SystemBot.Transports.Get(TransportKind.FLChat);
                return _systemBotTransport ?? throw new NullReferenceException("System bot transport is null");
            }
        }

        /// <summary>
        /// Set messages IsDelivered flag
        /// </summary>
        /// <param name="ids"></param>
        public void ExecuteMessageSetDelivered(Guid userId, IEnumerable<Guid> ids, TransportKind transportKind = TransportKind.FLChat) {
            this.Database.ExecuteSqlCommand(
                "exec [Msg].[MessagesSetDelivered] @userId, @ids, @transportTypeId",
                new SqlParameter("@userId", userId),
                SqlExtentions.CreateGuidListParameter("@ids", ids),
                new SqlParameter("@transportTypeId", (int)transportKind));
        }

        /// <summary>
        /// Set messages IsRead flag
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ids"></param>
        /// <param name="transportKind"></param>
        public void ExecuteMessageSetRead(Guid userId, IEnumerable<Guid> ids, TransportKind transportKind = TransportKind.FLChat) {
            this.Database.ExecuteSqlCommand(
                "exec [Msg].[MessagesSetRead] @userId, @ids, @transportTypeId",
                new SqlParameter("@userId", userId),
                SqlExtentions.CreateGuidListParameter("@ids", ids),
                new SqlParameter("@transportTypeId", (int)transportKind));
        }

        /// <summary>
        /// Execute [Usr].[Segment_UpdateMembers_FLUserNumber]
        /// </summary>
        /// <param name="segmentId"></param>
        /// <param name="members"></param>
        public void ExecuteSegmentUpdateMembers(Guid segmentId, IEnumerable<int> members) {
            this.Database.ExecuteSqlCommand(
                "exec [Usr].[Segment_UpdateMembers_FLUserNumber] @segmentId, @FLUserNumberList",
                new SqlParameter("segmentId", segmentId),
                SqlExtentions.CreateIntListParameter("@FLUserNumberList", members));
        }

        /// <summary>
        /// [Usr].[GetUserData](@userId, @key)
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <param name="key">Ключ данных</param>
        public string GetUserData(Guid userId, string key)
        {
            string data = this.Database.SqlQuery<string>
                ("select [Usr].[GetUserData](@userId, @key)", 
                new SqlParameter("@userId", userId), new SqlParameter("@key", key) ).FirstOrDefault();
            return data;
        }
        
        /// <summary>
        /// Dublicate function for avoid dublicate parameter
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [DbFunction("ChatEntities", "BroadcastProhibition_Structure")]
        public virtual IQueryable<Nullable<System.Guid>> BroadcastProhibition_Structure(Nullable<System.Guid> userId, int index) {
            string param_name = String.Format("userId_{0}", index);

            var userIdParameter = userId.HasValue ?
                new ObjectParameter(param_name, userId) :
                new ObjectParameter(param_name, typeof(System.Guid));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<Nullable<System.Guid>>(
                String.Format("[ChatEntities].[BroadcastProhibition_Structure](@{0})", param_name), userIdParameter);
        }
    }

}
