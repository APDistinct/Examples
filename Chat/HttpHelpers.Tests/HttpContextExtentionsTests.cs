using System;
using System.Web;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

using Newtonsoft.Json;

namespace FLChat.WebService.Tests
{
    [TestClass]
    public class HttpContextExtentionsTests
    {
        class TestData
        {
            public int i { get; set; }
            public string s { get; set; }

            public override bool Equals(object obj) {
                if (obj is TestData d) {
                    return i == d.i && s == d.s;
                } else 
                return base.Equals(obj);
            }

            public override int GetHashCode() {
                return base.GetHashCode();
            }
        }

        [TestMethod]
        public void HttpContextExtentions_MakeJsonResponse_OK() {
            // HttpContext context = new HttpContext()
            StringWriter sw = new StringWriter();
            HttpResponse response = new HttpResponse(sw);
            TestData d = new TestData() { i = 10, s = "123" };
            response.MakeJsonResponse(d);
            Assert.AreEqual(200, response.StatusCode);
            Assert.AreEqual(d, JsonConvert.DeserializeObject<TestData>(sw.ToString()));
        }

        [TestMethod]
        public void HttpContextExtentions_MakeJsonResponse_WithStatusCode() {
            // HttpContext context = new HttpContext()
            StringWriter sw = new StringWriter();
            HttpResponse response = new HttpResponse(sw);
            TestData d = new TestData() { i = 10, s = "123" };
            response.MakeJsonResponse(d, HttpStatusCode.InternalServerError);
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.AreEqual(d, JsonConvert.DeserializeObject<TestData>(sw.ToString()));
        }
    }
}
