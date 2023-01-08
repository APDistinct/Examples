using System;

namespace Devino.Args
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
