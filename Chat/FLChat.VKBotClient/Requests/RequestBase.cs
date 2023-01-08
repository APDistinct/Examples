using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FLChat.VKBotClient.Requests.Abstractions;
using Newtonsoft.Json;

namespace FLChat.VKBotClient.Requests
{
    public abstract class RequestBase<TResponse> : IRequest<TResponse>
    {
        /// <inheritdoc />
        public HttpMethod Method { get; }

        /// <inheritdoc />
        public string MethodName { get; protected set; }

        /// <summary>
        /// Initializes an instance of request
        /// </summary>
        /// <param name="methodName">API method</param>
        protected RequestBase(string methodName)
            : this(methodName, HttpMethod.Post)
        {
        }

        /// <summary>
        /// Initializes an instance of request
        /// </summary>
        /// <param name="methodName">API method</param>
        /// <param name="method">HTTP method to use</param>
        protected RequestBase(string methodName, HttpMethod method)
        {
            MethodName = methodName;
            Method = method;
        }

        /// <summary>
        /// Generate content of HTTP message
        /// </summary>
        /// <returns>Content of HTTP request</returns>
        public virtual HttpContent ToHttpContent()
        {
            string payload = JsonConvert.SerializeObject(this);
            return new StringContent(payload, Encoding.UTF8, "application/json");
        }

        public string GetQuery()
        {
            return $"{MethodName}?{Params}";
        }

        public virtual TResponse DeserializeResponse(string responseJson)
        {
            return JsonConvert.DeserializeObject<TResponse>(responseJson);
        }

        public abstract string Params { get; }
    }
}
