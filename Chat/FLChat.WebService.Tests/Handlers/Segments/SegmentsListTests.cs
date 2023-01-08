using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;

namespace FLChat.WebService.Handlers.Segments.Tests
{
    [TestClass]
    public class SegmentsListTests
    {
        ChatEntities entities;
        SegmentsList handler = new SegmentsList();

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void SegmentsList_AllResults() {
            SegmentListResponse resp = handler.ProcessRequest(entities, null, null);
            Assert.IsNotNull(resp);
            Assert.IsNotNull(resp.Segments);
            CollectionAssert.AreEquivalent(
                entities.Segment.Select(s => s.Id).ToArray(),
                resp.Segments.Select(s => s.Id).ToArray());
        }
    }
}
