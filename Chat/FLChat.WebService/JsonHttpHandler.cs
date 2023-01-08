using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Newtonsoft.Json;

namespace FLChat.WebService
{
    /// <summary>
    /// Process Http request with input and output Json's strings
    /// </summary>
    public class JsonHttpHandler<TIn, TOut> : IHttpHandler
         where TIn : class
         where TOut : class
    {
        /// <summary>
        /// Handling strategy
        /// </summary>
        public interface IHandlerStrategy
        {
            /// <summary>
            /// Process Http request
            /// </summary>
            /// <param name="requestData">input data, may be NULL if GET request</param>
            /// <returns>output data, may be NULL</returns>
            TOut ProcessRequest(TIn input);

            bool IsReusable { get; }
        }

        private readonly IHandlerStrategy _handlerStrategy;

        public bool IsReusable => _handlerStrategy.IsReusable;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="strategy"></param>
        public JsonHttpHandler(IHandlerStrategy strategy) {
            _handlerStrategy = strategy;
        }

        /// <summary>
        /// Handle Http request
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context) {
            TIn input;
            try {
                input = context.Request.ReadJson<TIn>();
            } catch (JsonException e) {
                context.Response.MakeJsonResponse(new ErrorResponse(ErrorResponse.Kind.input_data_error, e), HttpStatusCode.BadRequest);
                return;                
            }

            //perform handler action
            TOut output = _handlerStrategy.ProcessRequest(input);
            // serialize responce 
            context.Response.MakeJsonResponse(output);
        }        
    }
}
