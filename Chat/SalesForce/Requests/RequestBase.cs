using Newtonsoft.Json;
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
    /// Represents a API request
    /// </summary>
    /// <typeparam name="TResponse">Type of result expected in result</typeparam>
    public abstract class RequestBase<TRequest, TResponse> : IRequest<TRequest, TResponse> 
        where TRequest : class
        where TResponse : class
    {
        /// <inheritdoc />
        public HttpMethod Method { get; }

        /// <inheritdoc />
        public string MethodName { get; protected set; }

        /// <summary>
        /// Initializes an instance of request
        /// </summary>
        /// <param name="methodName">Bot API method</param>
        protected RequestBase(string methodName)
            : this(methodName, HttpMethod.Post) { }

        /// <summary>
        /// Initializes an instance of request
        /// </summary>
        /// <param name="methodName">Bot API method</param>
        /// <param name="method">HTTP method to use</param>
        protected RequestBase(string methodName, HttpMethod method) {
            MethodName = methodName;
            Method = method;
        }

        /// <inheritdoc />
        public abstract TRequest RequestBody { get; }

        /// <inheritdoc />
        public abstract Dictionary<string, string> QueryParams { get; }
    }
}
