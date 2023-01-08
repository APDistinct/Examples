using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.Core.Algorithms
{
    public class ComplexDeepLinkStrategy : IDeepLinkStrategy
    {
        private readonly IDeepLinkStrategy[] _items;

        public ComplexDeepLinkStrategy(IEnumerable<IDeepLinkStrategy> items) : this(items.ToArray()) {
        }

        public ComplexDeepLinkStrategy(IDeepLinkStrategy []items) {
            _items = items;
        }

        public bool AcceptDeepLink(ChatEntities entities, IDeepLinkData message, 
            out User user, out Message answerTo, out object context, out IDeepLinkStrategy sender) {
            foreach (var item in _items) {
                if (item.AcceptDeepLink(entities, message, out user, out answerTo, out context, out sender))
                    return true;
            }

            user = null;
            answerTo = null;
            context = null;
            sender = null;
            return false;
        }

        public void AfterAddTransport(ChatEntities entities, IDeepLinkData message, Transport transport, object context) {
            throw new NotImplementedException();
        }
    }
}
