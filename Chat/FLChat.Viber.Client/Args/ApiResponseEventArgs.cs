using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Viber.Client.Args
{
    /// <summary>
    /// Provides data for ApiResponseReceived event
    /// </summary>
    public class ApiResponseEventArgs
    {
        /// <summary>
        /// HTTP response received from API
        /// </summary>
        public HttpResponseMessage ResponseMessage { get; internal set; }

        /// <summary>
        /// Event arguments of this request
        /// </summary>
        public ApiRequestEventArgs ApiRequestEventArgs { get; internal set; }
    }
}
