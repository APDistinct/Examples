using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.DataTypes;

using FLChat.DAL;
using FLChat.DAL.Utils;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;

namespace FLChat.DAL.Model
{
    public partial class Message : IMessage
    {
        public MessageKind Kind {
            get => (MessageKind)MessageTypeId;
            set => MessageTypeId = (int)value;
        }
        
        public TransportKind FromTransportKind {
            get => (TransportKind)FromTransportTypeId;
            set => FromTransportTypeId = (int)value;
        }

        public MessageToUser ToUser {
            get => ToUsers.SingleOrDefault();
            set {
                if (ToUsers != null && ToUsers.Count > 0)
                    throw new Exception("Message addressee already exists");
                ToUsers = new MessageToUser[] { value };
            }
        }

        public Transport ToTransport {
            get => ToUser?.ToTransport;
            set {
                ToUser = new MessageToUser() {
                    ToTransport = value,
                    IsSent = (value.Kind == TransportKind.FLChat)
                };
            }
        }

        /// <summary>
        /// Is need to show 'get phone number' button
        /// </summary>
        public bool IsPhoneButton {
            get => GetSpecificValue(PhoneButtonValue, out string val);
            set {
                if (value)
                    Specific = String.Concat(Specific ?? "", PhoneButtonValue, ";");
            }
        }

        private const string PhoneButtonValue = "PHONE";

        /// <summary>
        /// Cashed specific values
        /// </summary>
        private Dictionary<string, string> _values = null;

        /// <summary>
        /// Get specific value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool GetSpecificValue(string name, out string value) {
            value = null;
            if (Specific == null)
                return false;

            if (_values == null)
                _values = Specific.ToDictionary();

            return _values.TryGetValue(name, out value);
        }

        public bool HasSpecificValue(string name) => GetSpecificValue(name, out string tmp);
    }

    public static class MessageExtentions
    {
        private class GuidInt
        {
            public Guid Guid { get; set; }
            public int Int { get; set; }
        }

        /// <summary>
        /// returns count of unread messages
        /// </summary>
        /// <param name="fromUser"></param>
        /// <param name="toUsers"></param>
        /// <returns></returns>
        public static Dictionary<Guid, int> CountOfUnreadMessages(this ChatEntities entities, Guid user, IEnumerable<Guid> fromUsers) {
            return entities.MessageToUser
                .Where(mtu =>
                    mtu.IsRead == false
                    && mtu.ToUserId == user
                    && fromUsers.Contains(mtu.Message.FromUserId)
                    && mtu.Message.IsDeleted == false
                    && mtu.ToTransport.TransportType.InnerTransport)
                .GroupBy(mtu => mtu.Message.FromUserId, (u, list) => new GuidInt() { Guid = u, Int = list.Count() })
                .ToDictionary(i => i.Guid, i => i.Int);
        }

        /// <summary>
        /// produce MessageToUser records for Message
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="msgId"></param>
        /// <param name="limit"></param>
        /// <param name="addressee"></param>
        /// <returns></returns>
        public static bool Message_ProduceToUsers(this ChatEntities entities, Guid msgId, 
            out LimitInfoResult limit, out List<MessageToUser> addressee) {

            using (var opener = new ConnectionOpener(entities)) {
                using (var cmd = entities.Database.Connection.CreateCommand()) {
                    cmd.Transaction = entities.Database.CurrentTransaction.UnderlyingTransaction;
                    cmd.CommandText = "exec [Msg].[Message_ProduceToUsers] @msgId";
                    cmd.Parameters.Add(new SqlParameter("@msgId", msgId));

                    using (var reader = cmd.ExecuteReader()) {
                        //read information about limits
                        if (!reader.Read())
                            throw new Exception("[Msg].[Message_ProduceToUsers] has not returns any result set");

                        limit = new LimitInfoResult() {
                            SelectionCount = reader.GetInt32(0),
                            ExceedDay = reader.GetBoolean(1),
                            DayLimit = reader.IsDBNull(2) ? null : (int?)reader.GetInt32(2),
                            SentOverToday = reader.IsDBNull(3) ? null : (int?)reader.GetInt32(3),
                            ExceedOnce = reader.GetBoolean(4),
                            OnceLimit = reader.IsDBNull(5) ? null : (int?)reader.GetInt32(5),
                        };

                        // result set with MessageToUser
                        if (reader.NextResult()) {
                            var mtu = ((IObjectContextAdapter)entities)
                                .ObjectContext
                                .Translate<MessageToUser>(reader, "MessageToUser", MergeOption.AppendOnly);
                            addressee = mtu.ToList();
                            return true;
                        } else {
                            addressee = null;
                            return false;
                        }
                    }
                }
            }
        }
    }
}
