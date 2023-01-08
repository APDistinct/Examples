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
    public class GetFile : IByteArrayHandlerStrategy // IBinFileHandlerStrategy
    {
        protected readonly IMediaTypeChecker _fileChecker;
        public bool IsReusable => true;

        public GetFile(IMediaTypeChecker fileChecker = null)
        {
            if (fileChecker == null)
                fileChecker = new FileChecker();
            _fileChecker = fileChecker;
        }

        public virtual int ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, NameValueCollection parameters,
            byte[] requestData, string requestContentType, 
            out byte[] responseData, out string responseContentType, out string fileName)
        {
            if (Guid.TryParse(this.GetKey(parameters), out Guid fileId))
                return ProcessRequest(entities, 
                    fileId, requestData, 
                    requestContentType, 
                    out responseData, out responseContentType, out fileName);
            throw new ErrorResponseException(
               (int)HttpStatusCode.NotFound,
               new ErrorResponse(ErrorResponse.Kind.not_found, $"User with id {this.GetKey(parameters)} not found"));
        }

        protected int ProcessRequest(ChatEntities entities, Guid fileId, byte[] requestData, string requestContentType, 
            out byte[] responseData, out string responseContentType, out string fileName)
        {
            responseData = null;
            responseContentType = null;

            FileInfo fi = entities
                .FileInfo
                .Where(f => f.Id == fileId)
                .Include(f => f.MediaType)
                .SingleOrDefault();
            if (fi != null) {

                using (FileEntities fileEntities = new FileEntities()) {
                    var fileSaved = fileEntities.FileData.Where(x => x.Id == fileId).FirstOrDefault();

                    if (fileSaved != null) {
                        responseData = fileSaved.Data;
                        responseContentType = fi.MediaType.Name;
                        fileName = fi.FileName;
                        return (int)HttpStatusCode.OK;
                    }
                }
            }

            throw new ErrorResponseException(
                (int)HttpStatusCode.NotFound,
                new ErrorResponse(ErrorResponse.Kind.not_found, $"File with id {fileId} has not found"));
        }
    }
}

