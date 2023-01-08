using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core
{
    /// <summary>
    /// Deep link accepted status
    /// </summary>
    public enum DeepLinkResultStatus
    {
        /// <summary>
        /// Deep link was accepted
        /// </summary>
        Accepted,

        /// <summary>
        /// Deep link was accepted by same user early
        /// </summary>
        AcceptedEarly,

        /// <summary>
        /// Deep link was rejected, because was accepted by another user early
        /// </summary>
        Rejected,

        /// <summary>
        /// Unknown deep link code
        /// </summary>
        Unknown
    }

    /// <summary>
    /// Result of accepting deep ling message
    /// </summary>
    public class DeepLinkResult
    {
        public DeepLinkResult(DeepLinkResultStatus status, object context = null) {
            Status = status;
            Context = context;
            if (status != DeepLinkResultStatus.Unknown && context == null)
                throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Deep link accept status
        /// </summary>
        public DeepLinkResultStatus Status { get; }

        public object Context { get; }
    }
}
