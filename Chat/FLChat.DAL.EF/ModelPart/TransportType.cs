using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace FLChat.DAL.Model
{
    public partial class TransportType //: ITransportType
    {
        public TransportKind Kind
        {
            get => (TransportKind) Id;
            set => Id = (int) value;        
        }
        public static List<TransportKind> GetTransport(ChatEntities entities, Guid id)
        {
            var transport = entities
                .Transport
                .Where(t => t.UserId == id)
                .Include(t => t.TransportType)
                .ToList();
            var kinds = transport.Select(t => t.TransportType.Kind).ToList();
            //var user = entities.User.Where(u => u.Id == id).FirstOrDefault();
            //if (user != null && user.EnabledInnerTransport)
            //{
            //    kinds.Add(TransportKind.FLChat);
            //}
            return kinds;
            //if (transport != null)
            //    return kinds;
            //else
            //    return new List<TransportKind>();
        }
    }
}
