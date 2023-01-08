using System;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;
using FLChat.DAL;
using FLChat.DAL.Model;
using System.Net;

namespace FLChat.WebService.Tests
{
    [TestClass]
    public class ByteArrayHandlerStrategyTests
    {
        //https://www.hanselman.com/blog/ASPNETMVCSessionAtMix08TDDAndMvcMockHelpers.aspx

        //public class ActionStrategy : IByteArrayHandlerStrategy
        //{
        //    Func<byte[], string, byte[], string, int> _action;

        //    public ActionStrategy(Func<byte[], string, byte[], string, int> action) {
        //        _action = action;
        //    }

        //    public bool IsReusable => false;

        //    public int ProcessRequest(ChatEntities entities, IUserInfo currUserInfo, byte[] requestData, string requestContentType, 
        //        out byte[] responseData, out string responseContentType) {
        //        responseData = null;
        //        responseContentType = null;
        //        return _action(requestData, requestContentType, responseData, responseContentType);
        //    }
        //}

        //[TestMethod]
        //public void TestMethod1() {
        //    byte[] data = new byte[] { 0, 1, 2, 3, 4 };

        //    var mockHttpContext = new API_Moq_HttpContext();
        //    var context = mockHttpContext.httpContext();

        //    new HttpContextWrapper()
        //    typeof(HttpRequest).GetField("_httpMethod", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(context.Request, "POST");
        //    context.request_Write(data);

        //    ByteArrayHandlerStrategyAdapter adapter = new ByteArrayHandlerStrategyAdapter(new ActionStrategy((d, ct, od, oct) => {
        //        Assert.AreEqual(data, d);
        //        return (int)HttpStatusCode.OK;
        //    }));
        //    adapter.ProcessRequest(null, null, context);
        //}

        public static HttpContext FakeHttpContext() {
            // var uri = new Uri(url);
            //var httpRequest = new HttpRequest(string.Empty, uri.ToString(), uri.Query.TrimStart('?'));
            var httpRequest = new HttpRequest("", "http://localhost", "");
            var stringWriter = new StringWriter();
            var httpResponse = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponse);

            //var sessionContainer = new HttpSessionStateContainer("id",
            //                                new SessionStateItemCollection(),
            //                                new HttpStaticObjectsCollection(),
            //                                10, true, HttpCookieMode.AutoDetect,
            //                                SessionStateMode.InProc, false);

            //SessionStateUtility.AddHttpSessionStateToContext(
            //                                     httpContext, sessionContainer);

            return httpContext;
        }
    }
}
