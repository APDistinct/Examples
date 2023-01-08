using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core.Algorithms
{
    public interface IReceiveUpdateStrategy<TDb>
    {
        /// <summary>
        /// Process receiving message
        /// </summary>
        /// <param name="db">database</param>
        /// <param name="message">outer message information</param>
        /// <param name="deepLinkResult">accepting deep link message. Null if message does not contain deep link</param>
        void Process(TDb db, IOuterMessage message, out DeepLinkResult deepLinkResult);
    }

    public static class IReceiveUpdateStrategyExtentions
    {
        /// <summary>
        /// Old interface method without IDeepLinkResult
        /// </summary>
        /// <typeparam name="TDb"></typeparam>
        /// <param name="strategy">strategy object</param>
        /// <param name="db">database</param>
        /// <param name="message">outer message information</param>
        public static void Process<TDb>(this IReceiveUpdateStrategy<TDb> strategy, TDb db, IOuterMessage message) {
            strategy.Process(db, message, out DeepLinkResult deepLinkResult);
        }
    }
}
