using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.FDAL.Model;
using FLChat.WebService.DataTypes;
using FLChat.WebService.MediaType;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.Handlers.File
{
    public class SetFile : IBinFileHandlerStrategy
    {
        protected readonly IMediaTypeChecker _fileChecker;
        public int SizeLimit { get; set; } = 1024 * 1024;
        public bool IsReusable => true;

        public SetFile(IMediaTypeChecker fileChecker = null) 
        {
            if (fileChecker == null)
                fileChecker = new FileChecker();
            _fileChecker = fileChecker;
        }

        public FileInfoShort ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, NameValueCollection parameters,
            byte[] requestData, string requestContentType, out byte[] responseData, out string responseContentType)
        {
            return ProcessRequest(entities, currUserInfo.UserId, requestData, requestContentType,
                out responseData, out responseContentType, SizeLimit, false);
        }

        private FileInfoShort ProcessRequest(ChatEntities entities, Guid id, byte[] requestData, string requestContentType
            , out byte[] responseData, out string responseContentType, int _sizeLimit = 1024 * 1024, bool getAll = false)
        {
            responseData = null;
            responseContentType = null;

            //int mediaTypeId = 0;
            //int mediaTypeGroupId = 0;

            //var fType = requestData.GetFileMediaType();

            //if (fType != requestContentType)
            //{
            //    throw new ErrorResponseException(
            //        (int)HttpStatusCode.UnsupportedMediaType,
            //        new ErrorResponse(ErrorResponse.Kind.not_support, $"File type is not {requestContentType},  it is {fType}"));
            //}

            //if (!_fileChecker.Check(entities, requestData, requestContentType, out mediaTypeId, out mediaTypeGroupId))
            //{
            //    throw new ErrorResponseException(
            //        (int)HttpStatusCode.UnsupportedMediaType,
            //        new ErrorResponse(ErrorResponse.Kind.not_support, $"File {requestContentType} media data is invalid"));
            //}

            //if (requestData.Length > SizeLimit)
            //{
            //    throw new ErrorResponseException(
            //        (int)HttpStatusCode.UnsupportedMediaType,
            //        new ErrorResponse(ErrorResponse.Kind.max_size_limit, $"File's size more then {SizeLimit} is not allowed"));
            //}

            DAL.Model.User user = entities.User
                .Where(u => (u.Id == id) && (getAll || u.Enabled))
                .Include(t => t.Files)
                .SingleOrDefault();
            if (user != null)
            {
                //  Сохранение файла
                //    FileEntities fileEntities = new FileEntities();
                //    DbContextTransaction transInfo = entities.Database.BeginTransaction();
                //    DbContextTransaction transData = fileEntities.Database.BeginTransaction();

                //    FileInfo fileInfo = new FileInfo()
                //    {
                //        FileOwnerId = user.Id,
                //        MediaTypeId = mediaTypeId,                    
                //        FileLength = requestData.Length
                //    };
                //    user.Files.Add(fileInfo);
                //    entities.SaveChanges();
                //    var guid = fileInfo.Id;

                //    FileData newFile = new FDAL.Model.FileData
                //    {
                //        Id = guid,
                //        MediaTypeId = mediaTypeId,                    
                //        Data = requestData,
                //    };
                //    fileEntities.FileData.Add(newFile);
                //    fileEntities.SaveChanges();
                //    try
                //    {
                //        transData?.Commit();
                //        transInfo?.Commit();

                //    }
                //    catch (Exception ex)
                //    {
                //        throw new ErrorResponseException(HttpStatusCode.InternalServerError, ErrorResponse.Kind.error, ex.Message);
                //    }

                FilePerformer filePerformer = new FilePerformer();
                var fileInfo = filePerformer.PerformFile(requestData, requestContentType, id, entities);
                return new FileInfoShort(fileInfo);
                //(int)HttpStatusCode.OK;
            }

            throw new ErrorResponseException(
                (int)HttpStatusCode.NotFound,
                new ErrorResponse(ErrorResponse.Kind.user_not_found, $"User with id {id} not found"));
        }

        //private Guid SaveFileInfo(FileInfoData file, Guid UserId, ChatEntities entities)
        //{
        //    var fileInfo = new FileInfo
        //    {
        //        FileName = file.FileName,
        //        FileOwnerId = UserId,
        //        MediaTypeId = file.FileMediaTypeId,
        //        FileLength = file.DataBin.Length
        //    };
        //    entities.FileInfo.Add(fileInfo);
        //    entities.SaveChanges();
        //    return fileInfo.Id;
        //}

        //private void SaveFileData(FileInfoData file, Guid fileId, FileEntities fileEntities)
        //{
        //    //bool ret = true;            

        //    FileData //newFile = null;
        //    newFile = new FDAL.Model.FileData
        //    {
        //        Id = fileId,
        //        MediaTypeId = file.FileMediaTypeId,
        //        Data = file.DataBin,
        //    };
        //    fileEntities.FileData.Add(newFile);
        //    fileEntities.SaveChanges();
        //    //return ret;
        //}
    }
}

