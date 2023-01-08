using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using Newtonsoft.Json;

namespace FLChat.WebService
{
    /// <summary>
    /// Exception with standart error response
    /// </summary>
    public class ErrorResponseException : HttpException
    {
        public ErrorResponse Error { get; }

        public ErrorResponseException(int httpCode, ErrorResponse error) 
            : base(httpCode, error.Descr) {
            Error = error;
        }

        public ErrorResponseException(HttpStatusCode httpCode, ErrorResponse error) 
            : this((int)httpCode, error) {
        }

        public ErrorResponseException(HttpStatusCode httpCode, ErrorResponse.Kind kind, string descr) 
            : this(httpCode, new ErrorResponse(kind, descr)) {
        }

        public ErrorResponseException(HttpStatusCode httpCode, ErrorResponse.Kind kind, Exception e)
            : this((int)httpCode, kind, e) {
        }

        public ErrorResponseException(int httpCode, ErrorResponse.Kind kind, Exception e) 
            : this(httpCode, new ErrorResponse(kind, e)) { }

        public ErrorResponseException(int httpCode, Exception e) 
            : this(httpCode, new ErrorResponse(e)) { }

        public override string Message => Convert(Error);

        private static string Convert(ErrorResponse error) {
            return JsonConvert.SerializeObject(error);
        }
    }
}
