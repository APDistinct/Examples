using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.FDAL.Model.Tests
{
    [TestClass]
    public class FileTests
    {
        FileEntities entities;

        [TestInitialize]
        public void Init() {
            entities = new FileEntities();
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void File_New()
        {
            FileData f = entities.FileData.Add(new FileData() {
                Data = new byte[] { 1, 2, 3 },
                //FileOwnerId = Guid.Empty,
                Id = Guid.NewGuid(),
                MediaTypeId = 1
            });
            entities.SaveChanges();

            Assert.AreNotEqual(0, f.Idx);
            Assert.AreNotEqual(Guid.Empty, f.Id);
            //Assert.IsTrue((DateTime.UtcNow - f.LoadDate).TotalMinutes < 1);
            entities.Entry(f).State = System.Data.Entity.EntityState.Deleted;
            entities.SaveChanges();
        }

        //[TestMethod]
        //public void File_New() {
        //    File f = entities.File.Add(new File() {
        //        FileData = new byte[] { 1, 2, 3 },
        //        FileOwnerId = Guid.Empty,
        //        MediaTypeId = 1
        //    });
        //    entities.SaveChanges();

        //    Assert.AreNotEqual(Guid.Empty, f.Id);
        //    Assert.IsTrue((DateTime.UtcNow - f.LoadDate).TotalMinutes < 1);


        //    entities.Entry(f).State = System.Data.Entity.EntityState.Deleted;
        //    entities.SaveChanges();
        //}
    }
}
