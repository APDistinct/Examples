using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using FLChat.DAL.Model;
using FLChat.WebService.Handlers;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService
{
    public class HttpHandlerFactory : IHttpHandler
    {
        public bool IsReusable => false;

        public void ProcessRequest(HttpContext context) {
            try {
                IHttpHandler handler = GetHandler(context);
                if (handler != null) {
                    handler.ProcessRequest(context);
                } else {
                    context.Response.MakeJsonResponse(new ErrorResponse(ErrorResponse.Kind.url_not_found, null, null), HttpStatusCode.NotFound);
                }
            } catch (ErrorResponseException e) {
                context.Response.MakeJsonResponse(e.Error, e.GetHttpCode());
            } catch (Exception e) {
                context.Response.MakeJsonResponse(new ErrorResponse(e), HttpStatusCode.InternalServerError);
            }
        }

        private IHttpHandler GetHandler(HttpContext context) {
            Uri uri = context.Request.Url;
            MyUrls urls = new MyUrls(context.Request.GetBaseSiteUrl());

            if (urls.IsAlive(uri)) {
                context.Response.Write(String.Concat("I'm alive!"));
                context.Response.End();
                return null;
            }

            //string key1;
            //if (urls.IsUser(uri, out key1) && context.Request.HttpMethod == "GET")
            //    return Create(new GetUserInfo(key1));

            //if (urls.IsUserAvatar(uri, out key1) && context.Request.HttpMethod == "GET")
            //    return Create(new GetUserAvatar(key1));

            //if (urls.IsUserAvatar(uri, out key1) && context.Request.HttpMethod == "POST")
            //    return Create(new SetUserAvatar(key1));

            //if (urls.IsProfile(uri) && context.Request.HttpMethod == "GET")
            //{
            //    return Create(new GetProfile());
            //}
            return null;
        }

        private CheckAuthHttpHandler Create<TIn, TOut>(IObjectedHandlerStrategy<TIn, TOut> s) 
            where TIn : class
            where TOut : class {
            return new CheckAuthHttpHandler(s.Adapt());
        }

        private CheckAuthHttpHandler Create(IByteArrayHandlerStrategy s) {
            return new CheckAuthHttpHandler(s.Adapt());
        }
    }
}
