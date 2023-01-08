using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using SalesForce.Types;

namespace SalesForce.Exceptions
{
    /// <summary>
    /// Represents an API error
    /// </summary>
    public class ApiRequestException : Exception
    {
        /// <summary>
        /// Http status code
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// Error description
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestException"/> class.
        /// </summary>
        public ApiRequestException(HttpStatusCode httpStatusCode, ErrorResponse resp)
            : base($"{httpStatusCode.ToString()}: {resp.Error} - {resp.Description}") {
            Error = resp.Error;
            Description = resp.Description;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestException"/> class.
        /// </summary>
        public ApiRequestException(HttpStatusCode httpStatusCode, string response)
            : base($"{httpStatusCode.ToString()}: unknown error") {
            Error = "unknown";
            Description = response;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestException"/> class.
        /// </summary>
        public ApiRequestException(string description, Exception e)
            : base($"{description} : {e.Message}", e) {
            Error = "exception";
            Description = e.Message;
        }
    }
}
