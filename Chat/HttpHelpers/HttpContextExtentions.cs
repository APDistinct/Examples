using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace System.Web
{
    public static class HttpContextExtentions
    {
        public static string GetBaseSiteUrl(this HttpRequest request) {
            string baseUrl = request.Url.Scheme + "://" + request.Url.Authority + request.ApplicationPath.TrimEnd('/') + '/';
            return baseUrl;
        }

        public static string GetBaseVirtualAppPath(this HttpRequest request) {
            string url = request.ApplicationPath;
            if (url.EndsWith("/"))
                return url;
            else
                return url + "/";
        }

        /// <summary>
        /// Extract Json object from HttpContext requst's data
        /// </summary>
        /// <typeparam name="T">Context data type</typeparam>
        /// <param name="request">Http request</param>
        /// <returns>Extracted data or null, if ContentLenth = 0</returns>
        /// <exception cref="JsonException">Json deserialization error</exception>
        public static T ReadJson<T>(this HttpRequest request) where T : class {
            if (request.HttpMethod == "GET" || request.ContentLength == 0)
                return null;

            byte[] PostData = request.BinaryRead(request.ContentLength);
            string requestString = Encoding.UTF8.GetString(PostData);
            return JsonConvert.DeserializeObject<T>(requestString);
        }

        /// <summary>
        /// Make Json responce with http status 200 OK
        /// </summary>
        /// <typeparam name="T">Type of responce object</typeparam>
        /// <param name="response">HttpResponse</param>
        /// <param name="output">output parameter, may be NULL</param>
        /// <param name="statusCode">response http status code</param>
        public static void MakeJsonResponse<T>(this HttpResponse responce, T output) where T : class {
            responce.MakeJsonResponse(output, HttpStatusCode.OK);
        }

        /// <summary>
        /// Make Json responce
        /// </summary>
        /// <typeparam name="T">Type of responce object</typeparam>
        /// <param name="response">HttpResponse</param>
        /// <param name="output">output parameter, may be NULL</param>
        /// <param name="statusCode">response http status code</param>
        public static void MakeJsonResponse<T>(this HttpResponse response, T output, HttpStatusCode statusCode) where T : class {
            response.MakeJsonResponse(output, (int)statusCode);
        }

        /// <summary>
        /// Make Json responce
        /// </summary>
        /// <typeparam name="T">Type of responce object</typeparam>
        /// <param name="response">HttpContext</param>
        /// <param name="output">output parameter, may be NULL</param>
        /// <param name="statusCode">response http status code</param>
        public static void MakeJsonResponse<T>(this HttpResponse response, T output, int statusCode) where T : class {
            response.StatusCode = statusCode;
            if (output != null) {
                response.Write(JsonConvert.SerializeObject(output));
                response.ContentType = "application/json";
            }
        }
    }
}
