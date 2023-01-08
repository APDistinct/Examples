using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Model
{
    public partial class MessageToUser
    {
        public TransportKind ToTransportKind
        {
            get => (TransportKind)this.ToTransportTypeId;
            set => this.ToTransportTypeId = (int)value;
        }
    }

    public static class MessageToUserExtentions
    {
        static public ICollection<MessageToUser> GetToSend(this ICollection<MessageToUser> q, int _transport)
        {
            var qmes = q.Where(mt =>
                               mt.ToTransportTypeId == _transport
                            && mt.IsSent == false
                            && mt.IsFailed == false
                            && mt.Message.IsDeleted == false);
            return (System.Collections.Generic.ICollection<MessageToUser>)qmes;
        }
    }
}
