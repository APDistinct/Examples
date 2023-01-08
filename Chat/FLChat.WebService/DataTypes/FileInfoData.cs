using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FLChat.WebService.DataTypes
{
    public class FileInfoData : FileInfoDataBase
    {
        
        /// <summary>
        ///  Пока убрали ввиду непонимания необходимости
        /// </summary>
        //[JsonProperty(PropertyName = "caption")]
        //public string Caption { get; set; } //"название", //опционально        
        
        /// <summary>
        /// File data
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; } // "base64 string" //данные файла
        [JsonIgnore]
        public int FileMediaTypeId { get; set; }
        [JsonIgnore]
        public byte[] DataBin => Convert.FromBase64String(Data);//  { get; set; }
    }
}
