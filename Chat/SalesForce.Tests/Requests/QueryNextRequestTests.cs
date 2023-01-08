using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SalesForce.Requests.Tests
{
    [TestClass]
    public class QueryNextRequestTests
    {
        [TestMethod]
        public void QueryNextRequest() {
            QueryNextRequest<object> request = new QueryNextRequest<object>("qwerty");
            Assert.AreEqual(System.Net.Http.HttpMethod.Get, request.Method);
            Assert.AreEqual("/services/data/v47.0/query/qwerty", request.MethodName);
            Assert.IsNull(request.RequestBody);
            Assert.IsNull(request.QueryParams);
        }
    }
}
