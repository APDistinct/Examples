using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.WebService
{
    public static class ErrorResponseExceptionExtentions
    {
        public static void Check(this ErrorResponseException e, HttpStatusCode httpCode, ErrorResponse.Kind kind) {
            Assert.AreEqual((int)httpCode, e.GetHttpCode());
            Assert.AreEqual(kind, e.Error.Error);
        }
    }
}
