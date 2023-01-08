using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.DAL.Import.Tests
{
    [TestClass]
    public class ImportResultTests {
        [TestMethod]
        public void ImportResultAdd() {
            ImportResult t = new ImportResult() {
                ClearedEmail = 1,
                ClearedPhone = 2,
                Inserted = 3,
                MissedOwner = 4,
                OwnerUpdated = 5,
                Updated = 6
            };
            ImportResult r = new ImportResult() {
                ClearedEmail = 11,
                ClearedPhone = 12,
                Inserted = 13,
                MissedOwner = 14,
                OwnerUpdated = 15,
                Updated = 16
            };
            t.Add(r);
            Assert.AreEqual(12, t.ClearedEmail);
            Assert.AreEqual(14, t.ClearedPhone);
            Assert.AreEqual(16, t.Inserted);
            Assert.AreEqual(18, t.MissedOwner);
            Assert.AreEqual(20, t.OwnerUpdated);
            Assert.AreEqual(22, t.Updated);
        }

        [TestMethod]
        public void ImportSegmentsAdd() {
            ImportSegmentResult res1 = new ImportSegmentResult() { Inserted = 1, Deleted = 2 };
            ImportSegmentResult res2 = new ImportSegmentResult() { Inserted = 11, Deleted = 12 };

            res1.Add(res2);
            Assert.AreEqual(12, res1.Inserted);
            Assert.AreEqual(14, res1.Deleted);
        }
    }
}
