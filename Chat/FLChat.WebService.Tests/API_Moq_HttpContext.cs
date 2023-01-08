using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Moq;

namespace FLChat.WebService.Tests
{
    public class API_Moq_HttpContext
    {
        public Mock<HttpContextBase> MockContext { get; set; }
        public Mock<HttpRequestBase> MockRequest { get; set; }
        public Mock<HttpResponseBase> MockResponse { get; set; }
        public Mock<HttpSessionStateBase> MockSession { get; set; }
        public Mock<HttpServerUtilityBase> MockServer { get; set; }
        public Mock<IPrincipal> MockUser { get; set; }
        public Mock<IIdentity> MockIdentity { get; set; }

        public HttpContextBase HttpContextBase { get; set; }
        public HttpRequestBase HttpRequestBase { get; set; }
        public HttpResponseBase HttpResponseBase { get; set; }

        public API_Moq_HttpContext() {
            createBaseMocks();
            setupNormalRequestValues();
        }

        public API_Moq_HttpContext createBaseMocks() {
            MockContext = new Mock<HttpContextBase>();
            MockRequest = new Mock<HttpRequestBase>();
            MockResponse = new Mock<HttpResponseBase>();
            MockSession = new Mock<HttpSessionStateBase>();
            MockServer = new Mock<HttpServerUtilityBase>();


            MockContext.Setup(ctx => ctx.Request).Returns(MockRequest.Object);
            MockContext.Setup(ctx => ctx.Response).Returns(MockResponse.Object);
            MockContext.Setup(ctx => ctx.Session).Returns(MockSession.Object);
            MockContext.Setup(ctx => ctx.Server).Returns(MockServer.Object);


            HttpContextBase = MockContext.Object;
            HttpRequestBase = MockRequest.Object;
            HttpResponseBase = MockResponse.Object;

            return this;
        }

        public API_Moq_HttpContext setupNormalRequestValues() {
            //Context.User
            var MockUser = new Mock<IPrincipal>();
            var MockIdentity = new Mock<IIdentity>();
            MockContext.Setup(context => context.User).Returns(MockUser.Object);
            MockUser.Setup(context => context.Identity).Returns(MockIdentity.Object);

            //Request
            MockRequest.Setup(request => request.InputStream).Returns(new MemoryStream());

            //Response
            MockResponse.Setup(response => response.OutputStream).Returns(new MemoryStream());
            return this;
        }
    }

    public static class API_Moq_HttpContext_ExtensionMethods
    {
        public static HttpContextBase httpContext(this API_Moq_HttpContext moqHttpContext) {
            return moqHttpContext.HttpContextBase;
        }

        public static HttpContextBase request_Write(this HttpContextBase httpContextBase, string text) {
            httpContextBase.stream_Write(httpContextBase.Request.InputStream, text);
            return httpContextBase;
        }

        public static HttpContextBase request_Write(this HttpContextBase httpContextBase, byte[] data) {
            httpContextBase.stream_Write(httpContextBase.Request.InputStream, data);
            return httpContextBase;
        }

        public static string request_Read(this HttpContextBase httpContextBase) {
            return httpContextBase.stream_Read(httpContextBase.Request.InputStream);
        }

        public static HttpContextBase response_Write(this HttpContextBase httpContextBase, string text) {
            httpContextBase.stream_Write(httpContextBase.Response.OutputStream, text);
            return httpContextBase;
        }

        public static string response_Read(this HttpContextBase httpContextBase) {
            return httpContextBase.stream_Read(httpContextBase.Response.OutputStream);
        }

        public static HttpContextBase stream_Write(this HttpContextBase httpContextBase, Stream inputStream, string text) {
            var streamWriter = new StreamWriter(inputStream);

            inputStream.Position = inputStream.Length;
            streamWriter.Write(text);
            streamWriter.Flush();
            inputStream.Position = 0;

            return httpContextBase;
        }

        public static HttpContextBase stream_Write(this HttpContextBase httpContextBase, Stream inputStream, byte []data) {
            var streamWriter = new BinaryWriter(inputStream);

            inputStream.Position = inputStream.Length;
            streamWriter.Write(data);
            streamWriter.Flush();
            inputStream.Position = 0;

            return httpContextBase;
        }

        public static string stream_Read(this HttpContextBase httpContextBase, Stream inputStream) {
            var originalPosition = inputStream.Position;
            var streamReader = new StreamReader(inputStream);
            var requestData = streamReader.ReadToEnd();
            inputStream.Position = originalPosition;
            return requestData;
        }
    }
}
