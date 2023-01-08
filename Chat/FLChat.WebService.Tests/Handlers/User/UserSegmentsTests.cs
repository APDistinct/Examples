using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL.Model;

using FLChat.WebService.DataTypes;
using System.Net;

namespace FLChat.WebService.Handlers.User.Tests
{
    [TestClass]
    public class UserSegmentsTests
    {
        ChatEntities entities;
        UserSegments handler = new UserSegments();

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void UserSegments_Success() {
            DAL.Model.User user = entities.GetUser(u => u.Segments.Any(),
                u => u.Segments.Add(entities.GetSegment(s => true, null))
                );
            SegmentListResponse resp = handler.ProcessRequest(entities, null, user.Id.ToString());
            Assert.IsNotNull(resp);
            Assert.IsNotNull(resp.Segments);
            CollectionAssert.AreEqual(
                user.Segments.Select(s => s.Id).ToArray(),
                resp.Segments.Select(s => s.Id).ToArray());
        }

        [TestMethod]
        public void UserSegments_Fail() {
            ErrorResponseException e;
            e = Assert.ThrowsException<ErrorResponseException>(() => handler.ProcessRequest(entities, null, null));
            Assert.AreEqual((int)HttpStatusCode.BadRequest, e.GetHttpCode());
            Assert.AreEqual(ErrorResponse.Kind.input_data_error, e.Error.Error);

            e = Assert.ThrowsException<ErrorResponseException>(() => handler.ProcessRequest(entities, null, "123"));
            Assert.AreEqual((int)HttpStatusCode.BadRequest, e.GetHttpCode());
            Assert.AreEqual(ErrorResponse.Kind.input_data_error, e.Error.Error);

            e = Assert.ThrowsException<ErrorResponseException>(() => handler.ProcessRequest(entities, null, Guid.NewGuid().ToString()));
            Assert.AreEqual((int)HttpStatusCode.NotFound, e.GetHttpCode());
            Assert.AreEqual(ErrorResponse.Kind.user_not_found, e.Error.Error);
        }
    }
}
