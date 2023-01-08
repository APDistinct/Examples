using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using FLChat.DAL;
using FLChat.DAL.Model;
using Newtonsoft.Json;
using FLChat.WebService.Handlers.Auth;

namespace FLChat.WebService
{
    /// <summary>
    /// Template HttpHandler for standart action with checking authentification
    /// </summary>
    public class CheckAuthHttpHandler : IHttpHandler
    {
        private readonly IHandlerStrategy _handlerStrategy;
        private readonly IVerifyTokenStrategy _verifyTokenStrategy;
        private readonly bool _needAuth;    //only authorized users

        public const string Bearer = "Bearer ";

        /// <summary>
        /// Handle http request strategy 
        /// </summary>
        public interface IHandlerStrategy
        {
            /// <summary>
            /// Handle http request
            /// </summary>
            /// <param name="entities">Database entities</param>
            /// <param name="currUserInfo">Information about current user</param>
            /// <param name="context">Http context</param>
            void ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, HttpContext context);

            bool IsReusable { get; }
        }

        /// <summary>
        /// Perform check authentification token
        /// </summary>
        public interface IVerifyTokenStrategy
        {
            /// <summary>
            /// Verify token is valid or not
            /// </summary>
            /// <param name="authorizationHeader">Authorization field</param>
            /// <param name="entities">database entities</param>
            /// <param name="isExpired">if returns null, this field has information about token expired or invalid</param>
            /// <returns>Information about user</returns>
            IUserAuthInfo CheckToken(string authorizationHeader, ChatEntities entities, out bool isExpired);
        }

        /// <summary>
        /// Create handler with authorization
        /// </summary>
        /// <param name="handlerStrategy">Handle strategy</param>
        /// <param name="needAuth">check authorization</param>
        public CheckAuthHttpHandler(IHandlerStrategy handlerStrategy, IVerifyTokenStrategy verifyTokenStrategy) {
            _handlerStrategy = handlerStrategy;
            _verifyTokenStrategy = verifyTokenStrategy;
            _needAuth = _verifyTokenStrategy != null;
        }

        /// <summary>
        /// Create handler without authorization
        /// </summary>
        /// <param name="handlerStrategy">Handle strategy</param>
        public CheckAuthHttpHandler(IHandlerStrategy handlerStrategy, bool auth = true, bool bot = false) 
            : this(handlerStrategy, auth ? new VerifyTokenStrategy(bot) : null ) {
        }

        public bool IsReusable => _handlerStrategy.IsReusable;

        public void ProcessRequest(HttpContext context) {
            try {
                //if (!String.IsNullOrEmpty(context.Request.PathInfo)) {
                //    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                //    return;
                //}

                string token = null;
                if (_needAuth) {
                    //extract auth token
                    token = context.Request.Headers["Authorization"];
                    
                    if (token == null) {
                        context.Response.MakeJsonResponse(new ErrorResponse(ErrorResponse.Kind.missed_auth_token, "Missed authorization token"), HttpStatusCode.Unauthorized);
                        return;
                    }

                    if (token.StartsWith(Bearer)) {
                        token = token.Substring(Bearer.Length);
                    } else
                        throw new ErrorResponseException(HttpStatusCode.Unauthorized, ErrorResponse.Kind.invalid_auth_token, "Need Bearer token");
                    
                }

                using (ChatEntities entities = new ChatEntities()) {
                    IUserAuthInfo user = null;
                    using (var trans = entities.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted)) {
                        //check auth token
                        if (_needAuth) {
                            user = _verifyTokenStrategy.CheckToken(token, entities, out bool isExpired);
                            if (user == null) {
                                context.Response.MakeJsonResponse(
                                    new ErrorResponse(
                                        isExpired ? ErrorResponse.Kind.expired : ErrorResponse.Kind.invalid_auth_token,
                                        "Authorization failed, token is invalid or expired"),
                                    HttpStatusCode.Unauthorized);
                                return;
                            }
                        }

                        trans.Commit();
                    }

                    _handlerStrategy.ProcessRequest(entities, user, context);
                }

            } catch (ErrorResponseException e) {
                context.Response.MakeJsonResponse(e.Error, e.GetHttpCode());
            } catch (Exception e) {
                context.Response.MakeJsonResponse(new ErrorResponse(e), HttpStatusCode.InternalServerError);
            }        
        }
    }
}
