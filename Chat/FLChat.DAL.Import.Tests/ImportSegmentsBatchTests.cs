using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FLChat.DAL.Model;

namespace FLChat.DAL.Import.Tests
{
    [TestClass]
    public class ImportSegmentsBatchTests
    {
        ChatEntities entities;

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void LoadSegmentsIds() {
            Segment seg1 = entities.GetSegment(s => s.PartnerName != null, s => s.PartnerName = Guid.NewGuid().ToString());
            Segment seg2 = entities.GetSegment(s => s.PartnerName != null && s.Id != seg1.Id, s => s.PartnerName = Guid.NewGuid().ToString());
            Dictionary<string, Guid> dict = ImportSegmentsBatch.LoadSegmentsIds(
                entities,
                new string[] { seg1.PartnerName, seg2.PartnerName },
                out List<string> inserted);

            Assert.AreEqual(2, dict.Count);
            Assert.AreEqual(seg1.Id, dict[seg1.PartnerName]);
            Assert.AreEqual(seg2.Id, dict[seg2.PartnerName]);
            Assert.IsNull(inserted);
        }

        [TestMethod]
        public void LoadSegmentIds_InsertNew() {
            Segment seg1 = entities.GetSegment(s => s.PartnerName != null, s => s.PartnerName = Guid.NewGuid().ToString());
            string newName = Guid.NewGuid().ToString();
            Dictionary<string, Guid> dict = ImportSegmentsBatch.LoadSegmentsIds(
                entities,
                new string[] { seg1.PartnerName, newName },
                out List<string> inserted);

            Assert.AreEqual(2, dict.Count);
            Assert.AreEqual(seg1.Id, dict[seg1.PartnerName]);
            Assert.IsTrue(dict.ContainsKey(newName));

            Assert.IsNotNull(inserted);
            CollectionAssert.AreEquivalent(new string[] { newName }, inserted.ToArray());

            Segment nseg = entities.Segment.Where(s => s.PartnerName == newName).SingleOrDefault();
            Assert.IsNotNull(nseg);
            Assert.AreEqual(newName, nseg.PartnerName);

            entities.Entry(nseg).State = System.Data.Entity.EntityState.Deleted;
            entities.SaveChanges();
        }

        [TestMethod]
        public void Execute() {
            Segment seg1 = entities.GetSegment(s => s.PartnerName != null, s => s.PartnerName = Guid.NewGuid().ToString());
            User user = entities.GetUserQ(
                where: q => q.Where(u => u.FLUserNumber != null),
                create: u => u.FLUserNumber = -1);            
            if (seg1.Members.Contains(user)) {
                seg1.Members.Remove(user);
                entities.SaveChanges();
            }

            Dictionary<string, Guid> segNames = new Dictionary<string, Guid> () {
                { seg1.PartnerName, seg1.Id }
            };
            using (ImportSegmentsBatch import = new ImportSegmentsBatch(segNames)) {
                import.Add(user.FLUserNumber.Value, new string[] { seg1.PartnerName });
                ImportSegmentsBatch.Result result = import.Execute(entities);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Inserted);
                Assert.AreEqual(0, result.Deleted);
            }

            using (ImportSegmentsBatch import = new ImportSegmentsBatch(segNames)) {
                import.Add(user.FLUserNumber.Value, new string[] { });
                ImportSegmentsBatch.Result result = import.Execute(entities);
                Assert.IsNotNull(result);
                Assert.AreEqual(0, result.Inserted);
                Assert.AreEqual(1, result.Deleted);
            }
        }
    }
}
