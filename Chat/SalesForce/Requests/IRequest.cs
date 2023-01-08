using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

//using FLChat.Viber.Client.Types;

namespace SalesForce.Requests
{
    /// <summary>
    /// Represents a request to Bot API
    /// </summary>
    /// <typeparam name="TResponse">Type of result expected in result</typeparam>
    public interface IRequest<TRequest, TResponse> 
        where TRequest : class 
        where TResponse : class
    {
        /// <summary>
        /// HTTP method of request
        /// </summary>
        HttpMethod Method { get; }

        /// <summary>
        /// API method name
        /// </summary>
        string MethodName { get; }

        /// <summary>
        /// Content of HTTP message
        /// </summary>
        /// <returns>Content of HTTP request</returns>
        TRequest RequestBody { get; }

        /// <summary>
        /// Query parameters
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> QueryParams { get; }
    }
}
