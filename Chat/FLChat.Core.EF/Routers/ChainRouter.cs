using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.Core.Routers
{
    /// <summary>
    /// List of IMessageRouters
    /// Perform routers in added order and stop performing when someone returns true.
    /// Perform order is from first to last
    /// </summary>
    public class ChainRouter : IMessageRouter, IEnumerable<IMessageRouter>
    {
        private readonly LinkedList<IMessageRouter> _list = new LinkedList<IMessageRouter>();

        /// <summary>
        /// Create empty router
        /// </summary>
        public ChainRouter() {
        }

        /// <summary>
        /// Create and add routers form <paramref name="list"/>
        /// First item in <paramref name="list"/> become first item in ChainRouter 
        /// </summary>
        /// <param name="list">List of routers</param>
        public ChainRouter(IEnumerable<IMessageRouter> list) {
            foreach (IMessageRouter router in list)
                _list.AddLast(router);
        }

        /// <summary>
        /// Add router to first position
        /// </summary>
        /// <param name="router"></param>
        public void Add(IMessageRouter router) {
            _list.AddFirst(router);
        }

        public Guid? RouteMessage(ChatEntities entities, IOuterMessage message, Message dbmsg) {
            foreach (IMessageRouter router in _list) {
                Guid? result = router.RouteMessage(entities, message, dbmsg);
                if (result != null)
                    return result;
            }
            return null;
        }

        public IEnumerator<IMessageRouter> GetEnumerator() {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
