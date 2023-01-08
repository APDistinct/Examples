using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService
{
    /// <summary>
    /// Handling strategy with json request and json response
    /// </summary>
    public interface IObjectedHandlerStrategy<TIn, TOut>
         where TIn : class
         where TOut : class {
        /// <summary>
        /// Process Http request
        /// </summary>
        /// <param name="userInfo">Information about user who made this request. Can't be NULL</param>
        /// <param name="input">input data, may be NULL if it's GET request</param>
        /// <returns>output data, may be NULL</returns>
        TOut ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, TIn input);

        bool IsReusable { get; }
    }

    public interface IObjectedHandlerStrategyExt<TIn, TOut> : IObjectedHandlerStrategy<TIn, TOut>
        where TIn : class
        where TOut : class
    {
        IsolationLevel Isolation { get; }
    }

    /// <summary>
    /// Adapt IObjectedHandlerStrategy to CheckAuthHttpHandler.IHandlerStrategy
    /// </summary>
    public class ObjectHandlerStrategyAdapter<TIn, TOut> : CheckAuthHttpHandler.IHandlerStrategy
        where TIn : class
        where TOut : class
    {
        private readonly IObjectedHandlerStrategyExt<TIn, TOut> _handler;
        private readonly int _keyIndex;
        //private readonly MethodInfo _setMethod;
        private readonly IEnrichMethod<TIn> _enrichMethod;

        /// <summary>
        /// Create adapter without keys parameters in Uri
        /// </summary>
        /// <param name="handler">Handler strategy</param>
        public ObjectHandlerStrategyAdapter(IObjectedHandlerStrategyExt<TIn, TOut> handler) {
            _handler = handler;
            _keyIndex = 0;
            _enrichMethod = CreateEnrichMethod(null);
        }

        /// <summary>
        /// Create adapter with single key parameter in Uri
        /// TIn can be string type, that case input value will assign to key parameter 
        /// </summary>
        /// <param name="handler">Handler strategy</param>
        /// <param name="keyIndex">Key index from end in Uri</param>
        /// <param name="keyPropertyName">Key property name in TIn class</param>
        public ObjectHandlerStrategyAdapter(IObjectedHandlerStrategyExt<TIn, TOut> handler, int keyIndex, string keyPropertyName = null) {
            _handler = handler;
            _keyIndex = keyIndex;

            if (keyIndex == 0)
                throw new Exception("Key field index must be greater then zero");

            _enrichMethod = CreateEnrichMethod(keyPropertyName);
        }

        private static IEnrichMethod<TIn> CreateEnrichMethod(string keyPropertyName) {
            Type t = typeof(TIn);
            if (t == typeof(string))
                return (IEnrichMethod<TIn>)new EnrichString();
            else if (t == typeof(JObject))
                return (IEnrichMethod<TIn>)new EnrichJObject(keyPropertyName);
            else
                return new EnrichProperty<TIn>(keyPropertyName);
        }

        public bool IsReusable => _handler.IsReusable;

        public void ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, HttpContext context) {
            //extract input data
            TIn input;
            try {
                input = context.Request.ReadJson<TIn>();
            } catch (JsonException e) {
                context.Response.MakeJsonResponse(new ErrorResponse(ErrorResponse.Kind.input_data_error, e), HttpStatusCode.BadRequest);
                return;
            }

            if (_keyIndex != 0) {
                string key = context.Request.Url.Segments[context.Request.Url.Segments.Length - _keyIndex];
                if (key[key.Length - 1] == '/')
                    key = key.Substring(0, key.Length - 1);
                _enrichMethod.Enrich(ref input, key);
            }

            _enrichMethod.Enrich(ref input, context.Request.QueryString);

            using (var trans = entities.Database.BeginTransaction(_handler.Isolation)) {
                //perform handler action
                TOut output = _handler.ProcessRequest(entities, currUserInfo, input);
                // serialize responce 
                context.Response.MakeJsonResponse(output);

                trans.Commit();
            }
        }
    }

    public static class IObjectedHandlerStrategyExtentions
    {
        /// <summary>
        /// Adopt IObjectedHandlerStrategy to IObjectedHandlerStrategyExt 
        /// with ReadUncommitted isolation level.
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        public class ExtAdapter<TIn, TOut> : IObjectedHandlerStrategyExt<TIn, TOut>
            where TIn : class
            where TOut : class
        {
            private readonly IObjectedHandlerStrategy<TIn, TOut> _handler;

            public ExtAdapter(IObjectedHandlerStrategy<TIn, TOut> handler) {
                _handler = handler;
            }

            public IsolationLevel Isolation => IsolationLevel.ReadUncommitted;

            public bool IsReusable => _handler.IsReusable;

            public TOut ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, TIn input)
                => _handler.ProcessRequest(entities, currUserInfo, input);
        }

        public static TOut ProcessRequestTransaction<TIn, TOut>(
            this IObjectedHandlerStrategy<TIn, TOut> handler,
            ChatEntities entities, IUserAuthInfo currUserInfo, TIn input)
            where TIn : class
            where TOut : class 
        {
            using (var trans = entities.Database.BeginTransaction(IsolationLevel.ReadUncommitted)) {
                var result = handler.ProcessRequest(entities, currUserInfo, input);
                trans.Commit();
                return result;
            }
        }

        public static CheckAuthHttpHandler.IHandlerStrategy Adapt<TIn, TOut>(this IObjectedHandlerStrategy<TIn, TOut> handler)
            where TIn : class
            where TOut : class {

            return new ObjectHandlerStrategyAdapter<TIn, TOut>(new ExtAdapter<TIn, TOut>(handler));
        }

        /// <summary>
        /// Create adapter with single key parameter in Uri
        /// TIn can be string type, that case input value will assign to key parameter 
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="handler">Handler strategy</param>
        /// <param name="keyIndex">Key index from end in Uri</param>
        /// <param name="keyPropertyName">Key property name in TIn class</param>
        /// <returns></returns>
        public static CheckAuthHttpHandler.IHandlerStrategy Adapt<TIn, TOut>(
            this IObjectedHandlerStrategy<TIn, TOut> handler,
            int keyIndex, string keyPropertyName = null)
            where TIn : class
            where TOut : class {

            return new ObjectHandlerStrategyAdapter<TIn, TOut>(new ExtAdapter<TIn, TOut>(handler), keyIndex, keyPropertyName);
        }
    }
}
