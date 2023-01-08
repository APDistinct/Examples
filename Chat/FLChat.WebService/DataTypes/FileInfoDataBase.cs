using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using FLChat.WebService.MediaType;

using FLChat.DAL;
using FLChat.DAL.Model;

namespace FLChat.WebService.DataTypes
{
    public class FileInfoDataBase
    {
        public FileInfoDataBase() {
        }

        public FileInfoDataBase(FileInfo fi) {
            FileMediaType = fi.MediaType.Kind;
            FileMimeType = fi.MediaType.Name;
            FileName = fi.FileName;
        }

        /// <summary>
        /// File type обобщённо
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter), new object[] { typeof(SnakeCaseNamingStrategy) })]
        public MediaGroupKind FileMediaType { get; set; }  // "image" | "document" | "audio", //обязательно
        
        /// <summary>
        /// File mediatype
        /// </summary>
        [JsonProperty(PropertyName = "mime_type")]
        public string FileMimeType { get; set; } //"image/jpeg", //опционально

        /// <summary>
        /// File name
        /// </summary>
        [JsonProperty(PropertyName = "file_name")]
        public string FileName { get; set; } //"имя файла", //только для документов (а надо ли?)
    }
}
