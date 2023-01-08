using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.FDAL.Model;
using FLChat.WebService.DataTypes;
using FLChat.WebService.Handlers.File;
using FLChat.WebService.MediaType;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.WebService.Tests.Handlers.File
{
    [TestClass]
    public class GetFileTests
    {
        FileEntities fileEntities;
        ChatEntities entities;
        GetFile handler;
        DAL.Model.User user;

        [TestInitialize]
        public void Init()
        {
            fileEntities = new FileEntities();
            entities = new ChatEntities();
            handler = new GetFile();

            user = entities.GetUser(u => u.Enabled, u => u.Enabled = true);
        }

        [TestCleanup]
        public void Clean()
        {
            entities.Dispose();
            fileEntities.Dispose();
        }

        /// <summary>
        /// Successfull set File 
        /// </summary>
        [TestMethod]
        public void Get_Correct()
        {
            // File
            var fileSaved = GetFile();

            byte[] requestData = fileSaved.Item2.Data;
            string requestContentType = fileSaved.Item1.MediaType.Name;
            byte[] responseData;
            string responseContentType;
            NameValueCollection parameters = new NameValueCollection(1);
            string key = fileSaved.Item2.Id.ToString();
            parameters.Add(BinFileHandlerStrategyAdapter.KeyName, key);

            var ret = handler.ProcessRequest(entities, user, parameters, requestData, requestContentType, 
                out responseData, out responseContentType, out string fileName);

            //var newfile = entities.FileInfo.Where(x => x.Id == ret.FileId).First();

            Assert.AreEqual(requestContentType, responseContentType);            
            CollectionAssert.AreEquivalent(requestData, responseData);
        }

        /// <summary>
        /// Not Successfull set File - bad file's Id
        /// </summary>
        [TestMethod]
        public void Get_Not_Correct_NoFile() {
            // File
            byte[] requestData = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
            string requestContentType = "image/png";

            byte[] responseData;
            string responseContentType;
            NameValueCollection parameters = new NameValueCollection(1);
            string key = "Bad Guid"; // fileSaved.Id.ToString();
            parameters.Add(BinFileHandlerStrategyAdapter.KeyName, key);

            var e = Assert.ThrowsException<ErrorResponseException>(() => handler.ProcessRequest(
               entities, user, parameters, requestData, requestContentType,
               out responseData, out responseContentType, out string fileName));
            e.Check(HttpStatusCode.NotFound, ErrorResponse.Kind.not_found);
        }

        [TestMethod]
        public void GetFile_CheckFileName() {
            var tuple = GetFile(
                q => q.Where(f => f.FileName != null && f.FileName != ""), 
                f => f.FileName = "somefile.png");

            NameValueCollection parameters = new NameValueCollection(1) {
                { BinFileHandlerStrategyAdapter.KeyName, tuple.Item1.Id.ToString() }
            };
            var resp = handler.ProcessRequest(entities, user, parameters, null, null,
                out byte[] data, out string contentType, out string fn);
            Assert.AreEqual(tuple.Item1.FileName, fn);
        }

        [TestMethod]
        public void GetFile_CheckFileName_Null() {
            var tuple = GetFile(
                q => q.Where(f => f.FileName == null),
                f => f.FileName = null);

            NameValueCollection parameters = new NameValueCollection(1) {
                { BinFileHandlerStrategyAdapter.KeyName, tuple.Item1.Id.ToString() }
            };
            var resp = handler.ProcessRequest(entities, user, parameters, null, null,
                out byte[] data, out string contentType, out string fn);
            Assert.IsNull(fn);
        }

        private Tuple<FileInfo, FileData> GetFile(
            Func<IQueryable<FileInfo>, IQueryable<FileInfo>> where = null,
            Action<FileInfo> create = null) {
            IQueryable<FileInfo> q = entities.FileInfo;
            if (where != null)
                q = where(q);
            foreach (FileInfo fi in q.ToArray()) {
                //FileData fileSaveData = null;
                FileData fd = fileEntities.FileData.Where(f => f.Id == fi.Id).FirstOrDefault();
                if (fd != null)
                    return Tuple.Create(fi, fd);
            }

            byte[] fileData = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
            var fileSaved = new FileInfo() {
                FileOwnerId = user.Id,
                FileName = "test.png",
                FileLength = fileData.Length,
                MediaTypeId = 1,
            };
            create?.Invoke(fileSaved);
            entities.FileInfo.Add(fileSaved);
            entities.SaveChanges();
            //fileSaved = entities.FileInfo.FirstOrDefault();

            FileData fileSaveData = new FileData() {
                Id = fileSaved.Id,
                Data = fileData,
                MediaTypeId = 1,
            };

            fileEntities.FileData.Add(new FileData() {
                Id = fileSaved.Id,
                Data = fileData,
                MediaTypeId = 1,
            });
            fileEntities.SaveChanges();

            return Tuple.Create(fileSaved, fileSaveData);
        }

    }
}
