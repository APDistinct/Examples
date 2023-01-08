using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.FDAL.Model;
using FLChat.WebService.DataTypes;
using FLChat.WebService.MediaType;
using FLChat.Core.Media;

namespace FLChat.WebService.Handlers.File
{
    public class FilePerformer
    {
        public Guid PerformMessage(FileInfoData file, Guid userId, ChatEntities entities/*, IMediaTypeChecker _fileChecker*/)
        {
            // Проверка на допустимость файла
            
            if (file.FileMediaType == MediaGroupKind.Document)
            {
                if ( file.FileMimeType == null)
                {
                    throw new ErrorResponseException(
                        (int)HttpStatusCode.UnsupportedMediaType,
                        new ErrorResponse(ErrorResponse.Kind.bad_file_type, $"File {file.FileName} Mime Type is required"));
                }
                var mediaTypeGroupId = (int)file.FileMediaType;
                GetMediaTypeId(file.FileMimeType, entities, out int? mediaTypeId, ref mediaTypeGroupId);
                file.FileMediaTypeId = mediaTypeId.Value;               
            }
            else
            {
                var mt = file.DataBin.GetFileMediaType();
                if(file.FileMimeType != null)
                {
                    if(mt != file.FileMimeType)
                    {
                        throw new ErrorResponseException(
                            (int)HttpStatusCode.UnsupportedMediaType,
                            new ErrorResponse(ErrorResponse.Kind.not_support, $"File type is not {file.FileMimeType},  it is {mt}"));
                    }
                }
                else
                {
                    file.FileMimeType = mt;
                }
                //  Проврка на совпадение типов
                if(!FindFileType(file.FileMimeType, entities, out int? mediaTypeId, out int? mediaTypeGroupId))
                {
                    throw new ErrorResponseException(
                        (int)HttpStatusCode.UnsupportedMediaType,
                        new ErrorResponse(ErrorResponse.Kind.not_support, $"File {file.FileName} media data is invalid"));
                }
                //_fileChecker.Check(entities, file.DataBin, mt, out mediaTypeId, out mediaTypeGroupId);
                if (mediaTypeGroupId != (int)file.FileMediaType)
                {
                    throw new ErrorResponseException(
                        (int)HttpStatusCode.UnsupportedMediaType,
                        new ErrorResponse(ErrorResponse.Kind.not_support, $"File {file.FileName} media data is invalid"));
                }
                file.FileMediaTypeId = mediaTypeId.Value;
            }
            //  Сохранение файла
            if(!CheckFileSize(file, entities, out int? sizeLimit))
            {
                throw new ErrorResponseException(
                   (int)HttpStatusCode.UnsupportedMediaType,
                   new ErrorResponse(ErrorResponse.Kind.max_size_limit, $"File's size more then {sizeLimit} is not allowed"));
            }
            var guid = SaveFile(file, userId, entities);
            return guid;  // Id сохранённого файла
        }

        public FileInfo PerformFile(byte[] fileData, string fileMimeType, Guid userId, ChatEntities entities)        
        {            
            // Проверка на допустимость файла

            // Поиск типа в БД
            if(!FindFileType(fileMimeType, entities, out int? mediaTypeId, out int? mediaTypeGroupId))
            {
                //  Поиск по сигнатуре
                mediaTypeGroupId = (int?)fileData.GetFileMediaGroup(fileMimeType);                
            }
            if(mediaTypeGroupId == null)
            {
                throw new ErrorResponseException(
                        (int)HttpStatusCode.UnsupportedMediaType,
                        new ErrorResponse(ErrorResponse.Kind.not_support, $"Type {fileMimeType} is not not allowed"));
            }
            FileInfoData file = new FileInfoData()
            {
                Data = Convert.ToBase64String(fileData),
                FileMediaType = (MediaGroupKind)mediaTypeGroupId,
                FileMimeType = fileMimeType,
            };

            // Найдены недостающие значения, теперь стандартная процедура фиксации
            var guid = PerformMessage(file, userId, entities);
            var fileInfo = entities.FileInfo.Where(x => x.Id == guid).First();
            return fileInfo;
        }

        public void GetMediaTypeId(string fileMimeType, ChatEntities entities, out int? mediaTypeId, ref int mediaTypeGroupId)
        {
            mediaTypeId = -1;
            //using (var trans = entities.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            //{
                DAL.Model.MediaType mt = entities.FindOrCreateMediaType(fileMimeType, (MediaGroupKind)mediaTypeGroupId);
                if (mt == null)
                    throw new ErrorResponseException(
                        (int)HttpStatusCode.UnsupportedMediaType,
                        new ErrorResponse(ErrorResponse.Kind.not_support, $"Type {fileMimeType} is not not allowed"));
                entities.SaveChanges();
                mediaTypeId = mt.Id;
                mediaTypeGroupId = mt.MediaTypeGroupId;                
            //    trans.Commit();
            //}
        }

        /// <summary>
        /// Сохранение файла в БД
        /// </summary>
        /// <param name="file">Информация о файле, в т.ч. данные</param>
        /// <param name="UserId">Владелец</param>
        /// <param name="entities">Контекст</param>
        /// <returns>Id файла в БД</returns>
        public Guid SaveFile(FileInfoData file, Guid UserId, ChatEntities entities) {
            //DbContextTransaction transInfo = null;
            DbContextTransaction transData = null;

            FileEntities fileEntities = new FileEntities();
            //transInfo = entities.Database.BeginTransaction();
            using (transData = fileEntities.Database.BeginTransaction()) {
                var guid = SaveFileInfo(file, UserId, entities);
                SaveFileData(file, guid, fileEntities);
                //            input.FileId = guid;  // Id сохранённого файла

                transData.Commit();
                return guid;
            }
        }

        /// <summary>
        /// Сохранение сведений о файле в БД(основная)
        /// </summary>
        /// <param name="file">Информация о файле, в т.ч. данные</param>
        /// <param name="UserId">Владелец</param>
        /// <param name="entities">Контекст</param>
        /// <returns>Id файла в БД</returns>
        private Guid SaveFileInfo(FileInfoData file, Guid UserId, ChatEntities entities)
        {
            var fileInfo = new FileInfo
            {
                FileName = file.FileName,
                FileOwnerId = UserId,
                MediaTypeId = file.FileMediaTypeId,
                FileLength = file.DataBin.Length
            };
            if(file.DataBin.GetImageSize(out int? width, out int? height))
            {
                fileInfo.Height = height;
                fileInfo.Width = width;
            }
            entities.FileInfo.Add(fileInfo);
            entities.SaveChanges();
            return fileInfo.Id;
        }

        /// <summary>
        /// Сохранение файла в БД(содержимое)
        /// </summary>
        /// <param name="file">Информация о файле, в т.ч. данные</param>
        /// <param name="fileId">Id файла в БД</param>
        /// <param name="entities">Контекст</param>
        private void SaveFileData(FileInfoData file, Guid fileId, FileEntities fileEntities)
        {
            //bool ret = true;            

            FileData //newFile = null;
            newFile = new FDAL.Model.FileData
            {
                Id = fileId,
                MediaTypeId = file.FileMediaTypeId,
                Data = file.DataBin,
            };
            fileEntities.FileData.Add(newFile);
            fileEntities.SaveChanges();
            //return ret;
        }

        /// <summary>
        /// Поиск файла в БД
        /// </summary>
        /// <param name="entities">Контекст</param>
        /// <param name="fileId">Id файла в БД</param>
        /// <returns>Да/нет</returns>
        public bool FileFind(ChatEntities entities, Guid fileId)
        {
            bool ret = true;
            //FileEntities fileEntities = new FileEntities();
            var fileSaved = entities.FileInfo.Where(x => x.Id == fileId)                
                .FirstOrDefault();
            ret = fileSaved != null;
            return ret;
        }

        /// <summary>
        /// Проверка на допустимость размера файла
        /// </summary>
        /// <param name="file">Информация о файле, в т.ч. данные</param>
        /// <param name="entities">Контекст</param>
        /// <param name="sizeLimit"></param>
        /// <returns>Да/нет</returns>        
        public bool CheckFileSize(FileInfoData file, ChatEntities entities, out int? sizeLimit)
        {
            bool ret = false;
            sizeLimit = 0;
            var fileSize = entities.MediaTypeGroup.Where(x => x.Id == (int)file.FileMediaType)
                .FirstOrDefault();

            if (fileSize != null)
            {
                sizeLimit = fileSize.MaxLength;
                ret = sizeLimit != null?  file.DataBin.Length <= sizeLimit : true;
            }
            return ret;
        }

        /// <summary>
        /// Поиск кода mime-типа и группы по наименованию
        /// </summary>
        /// <param name="fileMimeType">mime-тип от клиента</param>
        /// <param name="entities">Контекст</param>
        /// <param name="mediaTypeId">mime-тип код в БД</param>
        /// <param name="mediaTypeGroupId">код группы в БД</param>
        /// <returns>Найден или нет</returns>
        public bool FindFileType(string fileMimeType, ChatEntities entities, out int? mediaTypeId, out int? mediaTypeGroupId)
        {
            DAL.Model.MediaType mt = entities.FindMediaType(fileMimeType);
            if (mt != null) {
                mediaTypeId = mt.Id;
                mediaTypeGroupId = mt.MediaTypeGroupId;
                return true;
            } else {
                mediaTypeId = null;
                mediaTypeGroupId = null;
                return false;
            }
        }
    }
}
