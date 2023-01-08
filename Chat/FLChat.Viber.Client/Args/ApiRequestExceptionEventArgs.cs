using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Viber.Client.Args
{
    /// <summary>
    /// Provides data for exception during request
    /// </summary>
    public class ApiRequestExceptionEventArgs
    {
        /// <summary>
        /// HTTP response received from API
        /// </summary>
        public Exception Exception { get; internal set; }

        /// <summary>
        /// Event arguments of this request
        /// </summary>
        public ApiRequestEventArgs ApiRequestEventArgs { get; internal set; }
    }
}
