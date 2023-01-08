using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SalesForce.Requests.Tests
{
    [TestClass]
    public class QueryRequestTests
    {
        [TestMethod]
        public void QueryRequest() {
            QueryRequest<object> request = new QueryRequest<object>("select 1");
            Assert.AreEqual(System.Net.Http.HttpMethod.Get, request.Method);
            Assert.AreEqual("/services/data/v47.0/query", request.MethodName);
            Assert.IsNull(request.RequestBody);
            Assert.IsNotNull(request.QueryParams);
            Assert.AreEqual(1, request.QueryParams.Count);
            Assert.AreEqual("select 1", request.QueryParams["q"]);
        }
    }
}
