using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.VKBotClient.Requests.Abstractions
{
    /// <summary>
    /// Represents a request to Bot API
    /// </summary>
    /// <typeparam name="TResponse">Type of result expected in result</typeparam>
    public interface IRequest<TResponse>
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
        /// Generate content of HTTP message
        /// </summary>
        /// <returns>Content of HTTP request</returns>
        HttpContent ToHttpContent();

        string GetQuery();
        TResponse DeserializeResponse(string responseJson);
    }
}
