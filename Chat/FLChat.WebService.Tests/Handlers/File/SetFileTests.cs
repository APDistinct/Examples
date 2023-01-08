using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.FDAL.Model;
using FLChat.WebService.DataTypes;
using FLChat.WebService.Handlers.File;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.WebService.Tests.Handlers.File
{
    [TestClass]
    public class SetFileTests
    {
        FileEntities fileEntities;
        ChatEntities entities;
        SetFile handler;
        DAL.Model.User user;

        [TestInitialize]
        public void Init()
        {
            fileEntities = new FileEntities();
            entities = new ChatEntities();
            handler = new SetFile();

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
        public void Set_Correct()
        {
            // File            
            byte[] requestData = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
            string requestContentType = "image/png";
            byte[] responseData;
            string responseContentType;
            NameValueCollection parameters = null;

            var ret = handler.ProcessRequest(entities, user, parameters, requestData, requestContentType, out responseData, out responseContentType);

            var newfile = entities.FileInfo.Where(x => x.Id == ret.FileId).First();

            Assert.AreEqual(ret.FileMimeType, requestContentType);
            Assert.AreEqual(user.Id, newfile.FileOwnerId);
            //CollectionAssert.AreEquivalent(requestData, user.UserAvatar.Data);
        }

        /// <summary>
        /// Not Successfull set File - no such type
        /// </summary>
        [TestMethod]
        public void Set_Not_Correct_Type()
        {
            // File            
            byte[] requestData = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
            string requestContentType = "fdfdfdf";
            byte[] responseData;
            string responseContentType;
            NameValueCollection parameters = null;

            SetFile setFile = new SetFile();
            try
            {
                FileInfoShort responce = handler.ProcessRequest(entities, user, parameters
                    , requestData, requestContentType, out responseData, out responseContentType);
                Assert.Fail("Exception has not thrown for not_support file's kind ");
            }
            catch (ErrorResponseException e)
            {
                e.Check(HttpStatusCode.UnsupportedMediaType, ErrorResponse.Kind.not_support);
            }
        }

        /// <summary>
        /// Not Successfull set File - size limit
        /// </summary>
        [TestMethod]
        public void Set_Not_Correct_Size()
        {
            // File            
            byte[] requestData = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
            string requestContentType = "image/png";
            byte[] responseData;
            string responseContentType;
            int sizeLimit = 12;
            int mediaTypeGroupId = 1;

            NameValueCollection parameters = null;

            var fileTypeName = entities.MediaType.Where(x => x.Name == requestContentType)
               .FirstOrDefault();

            if (fileTypeName == null)
            {
                DAL.Model.MediaType mediaType = new DAL.Model.MediaType()
                {
                    Name = requestContentType,
                    CanBeAvatar = true,
                    MediaTypeGroupId = mediaTypeGroupId,
                    Enabled = true,
                };
                var trans = entities.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

                entities.MediaType.Add(mediaType);
                entities.SaveChanges();
                trans.Commit();
            }

            var fileSize = entities.MediaTypeGroup.Where(x => x.Id == mediaTypeGroupId)
            .FirstOrDefault();

            if (fileSize != null)
            {
                sizeLimit = fileSize.MaxLength ?? 12;
            }

            var addBytes = Enumerable.Repeat<byte>(1, sizeLimit).ToArray();

            var result = requestData.Concat(addBytes).ToArray();

            SetFile setFile = new SetFile();
            try
            {
                FileInfoShort responce = handler.ProcessRequest(entities, user, parameters
                    , result, requestContentType, out responseData, out responseContentType);
                Assert.Fail("Exception has not thrown for file's max_size_limit ");
            }
            catch (ErrorResponseException e)
            {
                e.Check(HttpStatusCode.UnsupportedMediaType, ErrorResponse.Kind.max_size_limit);
            }
        }

        /// <summary>
        /// Not Successfull set File - mime type not Enabled
        /// </summary>
        [TestMethod]
        public void Set_Not_Enable()
        {
            // File            
            byte[] requestData = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
            string requestContentType = "image/png";
            byte[] responseData;
            string responseContentType;            
            int mediaTypeGroupId = 1;

            bool nowEnabled = false;

            NameValueCollection parameters = null;

            var fileTypeName = entities.MediaType.Where(x => x.Name == requestContentType)
               .FirstOrDefault();

            if (fileTypeName == null)
            {
                DAL.Model.MediaType mediaType = new DAL.Model.MediaType()
                {
                    Name = requestContentType,
                    CanBeAvatar = true,
                    MediaTypeGroupId = mediaTypeGroupId,
                    Enabled = false,
                };
                var trans = entities.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

                entities.MediaType.Add(mediaType);
                entities.SaveChanges();
                trans.Commit();
            }
            else
            {
                nowEnabled = fileTypeName.Enabled;
                fileTypeName.Enabled = false;
                entities.SaveChanges();
            }
            
            SetFile setFile = new SetFile();
            try
            {
                FileInfoShort responce = handler.ProcessRequest(entities, user, parameters
                    , requestData, requestContentType, out responseData, out responseContentType);
                Assert.Fail("Exception has not thrown for not_support file's kind ");
            }
            catch (ErrorResponseException e)
            {
                e.Check(HttpStatusCode.UnsupportedMediaType, ErrorResponse.Kind.not_support);
            }
            finally
            {
                if(nowEnabled)
                {
                    fileTypeName.Enabled = true;
                    entities.SaveChanges();
                }
            }
        }
    }
}
