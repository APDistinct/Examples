using FLChat.Core.Algorithms;
using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core
{
    public class ActionMessageStrategy : IReceiveUpdateStrategy<ChatEntities>
    {
        private readonly Func<ChatEntities, IOuterMessage, DeepLinkResult> _func;

        public ActionMessageStrategy(Func<ChatEntities, IOuterMessage, DeepLinkResult> func) {
            _func = func;
        }

        public void Process(ChatEntities db, IOuterMessage message, out DeepLinkResult deepLinkResult) {
            deepLinkResult = _func(db, message);
        }
    }
}
