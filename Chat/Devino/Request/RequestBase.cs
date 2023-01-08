using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FLChat.Devino.Request
{
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

    public abstract class RequestBase<TResponse> : IRequest<TResponse>
    {
        public HttpMethod Method { get; }
        public string MethodName { get; protected set; }

        public List<Param> Params { get; } = new List<Param>();

        protected RequestBase(string methodName, HttpMethod method)
        {
            MethodName = methodName;
            Method = method;
        }

        public virtual HttpContent ToHttpContent()
        {
            string payload = JsonConvert.SerializeObject(this);
            return new StringContent(payload, Encoding.UTF8, "application/json");
        }

        public virtual TResponse DeserializeResponse(string responseJson)
        {
            return JsonConvert.DeserializeObject<TResponse>(responseJson);
        }

        public string GetQuery()
        {
            var queryParams = string.Join("&", Params.Select(a => $"{a.Name}={a.Value}"));
            return $"{MethodName}?{queryParams}";
        }
    }

    public class Param
    {
        public Param(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}
