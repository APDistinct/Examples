using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.WebService.DataTypes
{
    public class FileInfoShort : FileInfoDataBase
    {
        public FileInfoShort() {
        }

        public FileInfoShort(FileInfo fi) : base(fi) {
            FileId = fi.Id;
            Length = fi.FileLength;
            Width = fi.Width;
            Height = fi.Height;
        }

        [JsonProperty(PropertyName = "file_id")]
        public Guid FileId { get; set; }

        [JsonProperty(PropertyName = "length")]
        public int Length { get; set; }

        [JsonProperty(PropertyName = "width")]
        public int? Width { get; set; }        

        [JsonProperty(PropertyName = "height")]
        public int? Height { get; set; }
    }
}
