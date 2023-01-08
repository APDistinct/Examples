using System;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL.Model;
using FLChat.DAL;
using FLChat.FDAL.Model;

namespace FLChat.Core.Tests
{
    [TestClass]
    public class IFileLoaderTests
    {
        private ChatEntities entities;

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Cleanup() {
            entities.Dispose();
        }

        /// <summary>
        /// GetMatchedPhones file of existed media type
        /// </summary>
        [TestMethod]
        public void IFileLoaderExtentions_SaveFile_ExistedMediaType() {
            MediaType mt = entities.GetMediaType(group: MediaGroupKind.Document);
            DownloadFileResult dfr = new DownloadFileResult(
                new FakeInputFile() { Type = mt.Kind }, mt.Name, new byte[] { 1, 2, 3 });
            User owner = entities.GetUserQ(null, null);
            DAL.Model.FileInfo fi = entities.SaveFile(dfr, owner.Id);

            Assert.IsFalse(entities.ChangeTracker.HasChanges());
            Assert.IsNotNull(fi);

            DAL.Model.FileInfo db = entities.FileInfo.Where(f => f.Id == fi.Id).Single();
            Assert.AreEqual(fi.Id, db.Id);
            Assert.AreEqual(mt.Id, db.MediaTypeId);
            Assert.AreEqual(dfr.InputFile.FileName, db.FileName);
            Assert.AreEqual(owner.Id, db.FileOwnerId);
            Assert.AreEqual(dfr.Data.Length, db.FileLength);
            Assert.IsNull(db.Width);
            Assert.IsNull(db.Height);


            using (FileEntities fileEntities = new FileEntities()) {
                FileData fdb = fileEntities.FileData.Where(f => f.Id == db.Id).Single();
                Assert.AreEqual(fdb.MediaTypeId, db.MediaTypeId);
                CollectionAssert.AreEqual(fdb.Data, dfr.Data);
            }
        }

        /// <summary>
        /// GetMatchedPhones file of existed media type, but input data media group is different
        /// </summary>
        [TestMethod]
        public void IFileLoaderExtentions_SaveFile_ExistedMediaType_WrongGroup() {
            MediaType mt = entities.GetMediaType(group: MediaGroupKind.Document);
            DownloadFileResult dfr = new DownloadFileResult(
                new FakeInputFile() { Type = MediaGroupKind.Video }, //input media group and database media group are different
                mt.Name, 
                new byte[] { 1, 2, 3 });
            User owner = entities.GetUserQ(null, null);
            DAL.Model.FileInfo fi = entities.SaveFile(dfr, owner.Id);

            Assert.IsFalse(entities.ChangeTracker.HasChanges());
            Assert.IsNotNull(fi);

            DAL.Model.FileInfo db = entities.FileInfo.Where(f => f.Id == fi.Id).Single();
            Assert.AreEqual(fi.Id, db.Id);
            Assert.AreEqual(mt.Id, db.MediaTypeId);
            Assert.AreEqual(dfr.InputFile.FileName, db.FileName);
            Assert.AreEqual(owner.Id, db.FileOwnerId);
            Assert.AreEqual(dfr.Data.Length, db.FileLength);

            using (FileEntities fileEntities = new FileEntities()) {
                FileData fdb = fileEntities.FileData.Where(f => f.Id == db.Id).Single();
                Assert.AreEqual(fdb.MediaTypeId, db.MediaTypeId);
                CollectionAssert.AreEqual(fdb.Data, dfr.Data);
            }
        }

        /// <summary>
        /// GetMatchedPhones file of new media type
        /// </summary>
        [TestMethod]
        public void IFileLoaderExtentions_SaveFile_NewMediaType() {
            DownloadFileResult dfr = new DownloadFileResult(
                new FakeInputFile() { Type = MediaGroupKind.Document },
                Guid.NewGuid().ToString(),  //unknown media type
                new byte[] { 1, 2, 3 });
            User owner = entities.GetUserQ(null, null);
            DAL.Model.FileInfo fi = entities.SaveFile(dfr, owner.Id);

            Assert.IsNotNull(fi);
            DAL.Model.FileInfo db = entities.FileInfo.Where(f => f.Id == fi.Id).Single();
            Assert.AreEqual(fi.MediaTypeId, db.MediaTypeId);
            Assert.AreEqual(dfr.MediaType, db.MediaType.Name);

            MediaType mt = entities.MediaType.Where(i => i.Id == db.MediaTypeId).Single();
            Assert.AreEqual(dfr.MediaType, mt.Name);
            Assert.AreEqual(dfr.InputFile.Type, mt.Kind);
        }

        /// <summary>
        /// GetMatchedPhones file with parameter owner is null
        /// </summary>
        [TestMethod]
        public void IFileLoaderExtentions_SaveFile_NullOwner() {
            MediaType mt = entities.GetMediaType(group: MediaGroupKind.Document);
            DownloadFileResult dfr = new DownloadFileResult(
                new FakeInputFile() { Type = mt.Kind }, mt.Name, new byte[] { 1, 2, 3 });
            DAL.Model.FileInfo fi = entities.SaveFile(dfr, null);

            Assert.IsNotNull(fi);

            DAL.Model.FileInfo db = entities.FileInfo.Where(f => f.Id == fi.Id).Single();
            Assert.AreEqual(Global.SystemBotId, db.FileOwnerId);
        }

        /// <summary>
        /// GetMatchedPhones file of forbidden media type
        /// </summary>
        [TestMethod]
        public void IFileLoaderExtentions_SaveFile_ForbiddenMediaType() {
            MediaType mt = entities.GetMediaType(enabled: false);
            DownloadFileResult dfr = new DownloadFileResult(
                new FakeInputFile() { Type = mt.Kind }, mt.Name, new byte[] { 1, 2, 3 });
            User owner = entities.GetUserQ(null, null);

            Assert.ThrowsException<NotSupportedException>(() => entities.SaveFile(dfr, owner.Id));
        }

        /// <summary>
        /// GetMatchedPhones file of forbidden media type
        /// </summary>
        [TestMethod]
        public void IFileLoaderExtentions_SaveFile_InvalidImage() {
            MediaType mt = entities.GetMediaType(group: MediaGroupKind.Image);
            DownloadFileResult dfr = new DownloadFileResult(
                new FakeInputFile() { Type = mt.Kind }, mt.Name, new byte[] { 1, 2, 3 });
            User owner = entities.GetUserQ(null, null);

            Assert.ThrowsException<InvalidOperationException>(() => entities.SaveFile(dfr, owner.Id));
        }

        /// <summary>
        /// GetMatchedPhones file of forbidden media type
        /// </summary>
        [TestMethod]
        public void IFileLoaderExtentions_SaveFile_Image() {
            MediaType mt = entities.GetMediaType(name: "image/bmp", group: MediaGroupKind.Image);
            byte[] data = File.ReadAllBytes("./Resources/Chat.bmp");
            DownloadFileResult dfr = new DownloadFileResult(
                new FakeInputFile() { Type = mt.Kind }, mt.Name, data);
            User owner = entities.GetUserQ(null, null);

            DAL.Model.FileInfo fi = entities.SaveFile(dfr, owner.Id);

            Assert.IsNotNull(fi);

            DAL.Model.FileInfo db = entities.FileInfo.Where(f => f.Id == fi.Id).Single();
            Assert.AreEqual(mt.Id, db.MediaTypeId);
            Assert.AreEqual(dfr.InputFile.FileName, db.FileName);
            Assert.IsNotNull(db.Width);
            Assert.IsNotNull(db.Height);
        }

        /// <summary>
        /// GetMatchedPhones file of forbidden media type
        /// </summary>
        [TestMethod]
        public void IFileLoaderExtentions_SaveFile_WithoutMediaType() {
            byte[] data = File.ReadAllBytes("./Resources/Chat.bmp");
            DownloadFileResult dfr = new DownloadFileResult(
                new FakeInputFile() { Type = MediaGroupKind.Document, FileName = "some.bmp" }, null, data);
            User owner = entities.GetUserQ(null, null);

            DAL.Model.FileInfo fi = entities.SaveFile(dfr, owner.Id);

            Assert.IsNotNull(fi);

            DAL.Model.FileInfo db = entities.FileInfo.Where(f => f.Id == fi.Id).Single();
            Assert.AreEqual(dfr.InputFile.FileName, db.FileName);
            Assert.AreEqual("image/bmp", db.MediaType.Name);
            Assert.IsNotNull(db.Width);
            Assert.IsNotNull(db.Height);
        }

        [TestMethod]
        public void IFileLoader_DetectMediaType() {
            Assert.AreEqual("image/jpeg", IFileLoaderExtentions.DetectMediaType(null, "someimage.JPG", MediaGroupKind.Document));
            Assert.AreEqual("image/jpeg", IFileLoaderExtentions.DetectMediaType(null, "someimage.jpeg", MediaGroupKind.Image));
            Assert.AreEqual("application/pdf", IFileLoaderExtentions.DetectMediaType(null, "some.pdf", MediaGroupKind.Document));
            Assert.AreEqual("image/pdf", IFileLoaderExtentions.DetectMediaType(null, "some.pdf", MediaGroupKind.Image));
        }

        [TestMethod]
        public void IFileLoader_GetFileNameFromUrl()
        {
            string fname = "test.jpg";
            string url = "rrr//dfgfgrt/fdf/" + fname + "?understand";
            string getname = url.GetFileNameFromUrl();

            Assert.AreEqual(fname, getname);            
        }
    }
}
