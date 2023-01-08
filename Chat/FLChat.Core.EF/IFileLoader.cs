using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.FDAL.Model;
using FLChat.Core.Media;

namespace FLChat.Core
{
    public class DownloadFileResult
    {
        public DownloadFileResult(IInputFile inputFile, string mediaType, byte[] data) {
            InputFile = inputFile;
            MediaType = mediaType;
            Data = data;
        }

        public IInputFile InputFile { get; }
        public string MediaType { get; }
        public byte[] Data { get; }
    }


    public interface IFileLoader
    {
        DownloadFileResult Download(IInputFile file);
    }

    public static class IFileLoaderExtentions {
        /// <summary>
        /// Save downloaded file info and data to database
        /// </summary>
        /// <param name="entities">database entities</param>
        /// <param name="file">information about downloaded file</param>
        /// <param name="owner">file owner. If null then system bot become to file owner</param>
        /// <returns>database entitiy <see cref="FileInfo"/></returns>
        /// <exception cref="NotSupportedException">throws if media type is forbidden</exception>
        /// <exception cref="InvalidOperationException">calculate image size was failed</exception>
        public static FileInfo SaveFile(this ChatEntities entities, DownloadFileResult file, Guid? owner) {
            string mediaTypeName = file.MediaType;
            if (file.MediaType == null) {
                mediaTypeName = DetectMediaType(entities, file.InputFile.FileName, file.InputFile.Type) ?? "unknown";
            }

            MediaType mediaType = entities.FindOrCreateMediaType(mediaTypeName, file.InputFile.Type, onlyEnabled: true);
            if (mediaType == null)
                throw new NotSupportedException($"Media type {file.MediaType} is not supported");

            FileInfo fi = new FileInfo() {
                FileOwnerId = owner ?? Global.SystemBotId,
                FileName = file.InputFile.FileName,
                MediaType = mediaType,
                MediaTypeId = mediaType.Id,
                FileLength = file.Data.Length
            };
            if (mediaType.Kind == MediaGroupKind.Image) {
                if (!file.Data.GetImageSize(out int? width, out int? height))
                    throw new InvalidOperationException("Can't calculate image width and height");
                fi.Height = height.Value;
                fi.Width = width.Value;
            }
            entities.FileInfo.Add(fi);
            entities.SaveChanges();

            using (FileEntities fileEntities = new FileEntities()) {
                FileData fd = new FileData() {
                    Id = fi.Id,
                    MediaTypeId = mediaType.Id,
                    Data = file.Data
                };
                fileEntities.FileData.Add(fd);
                fileEntities.SaveChanges();
            }

            return fi;
        }

        public static string[] imageExt = new string[] { "jpg", "jpeg", "bmp", "png", "giff", "tiff" };

        public static string DetectMediaType(ChatEntities entities, string FileName, MediaGroupKind group) {
            if (FileName != null) {
                int index = FileName.LastIndexOf('.');
                if (index > 0) {
                    string ext = FileName.Substring(index + 1).ToLower();
                    if (group == MediaGroupKind.Image || imageExt.Contains(ext)) {
                        if (ext == "jpg")
                            ext = "jpeg";
                        return "image/" + ext;
                    } else
                        return "application/" + ext;
                }                              
            }
            return null;
        }

        public static string GetFileNameFromUrl(this string Url)
        {
            string FileName = null;
            int index = Url.LastIndexOf('/');
            if (index >= 0)
            {
                int index2 = Url.IndexOf('?', index);
                if (index2 > 0)
                    FileName = Url.Substring(index + 1, index2 - index - 1);
                else
                    FileName = Url.Substring(index + 1);
            }
            else
                FileName = Url;
            return FileName;
        }
    }
}
