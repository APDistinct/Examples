using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core
{
    public interface IOuterMessageStatus : IOuterMessageId
    {
        /// <summary>
        /// Message was failed flag
        /// </summary>
        bool IsFailed { get; }     
        
        /// <summary>
        /// Message fails reason, can be null
        /// </summary>
        string FailureReason { get; }

        /// <summary>
        /// Message was delivered flag
        /// </summary>
        bool IsDelivered { get; }
        
        /// <summary>
        /// Message was read flag
        /// </summary>
        bool IsRead { get; }

        /// <summary>
        /// Message addressee, for whom status was changed
        /// </summary>
        string UserId { get; }
    }
}
