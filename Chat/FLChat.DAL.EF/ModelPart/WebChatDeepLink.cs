using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Model
{
    public partial class WebChatDeepLink
    {
        public IEnumerable<TransportKind> AcceptedTransportKind => AcceptedTransportType.Select(t => (TransportKind)t.Id);
    }
}
