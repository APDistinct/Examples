using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService
{
    public class HttpHandlerRequestInfo : IHttpHandler
    {
        public bool IsReusable => true;

        public void ProcessRequest(HttpContext context) {
            JObject json = new JObject();
            json["Http method"] = context.Request.HttpMethod;
            json["Request.Url_string"] = context.Request.Url.ToString();
            json["Request.ApplicationPath"] = context.Request.ApplicationPath;
            json["Request.Url.Segments"] = new JArray(context.Request.Url.Segments);
            json["context.Request.PathInfo"] = context.Request.PathInfo;
            
            context.Response.Write(json.ToString());
            context.Response.StatusCode = (int)HttpStatusCode.OK;
        }
    }
}
