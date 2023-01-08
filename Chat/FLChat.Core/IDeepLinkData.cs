using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core
{
    public interface IDeepLinkData : ICallbackBase
    {
        string FromId { get; }

        /// <summary>
        /// If message has deep link, this property contains deep link code. Otherwise is null
        /// </summary>
        string DeepLink { get; }

        /// <summary>
        /// User's transport is enabled or disabled
        /// </summary>
        bool IsTransportEnabled { get; }
    }
}
