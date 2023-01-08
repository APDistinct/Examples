using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FLChat.WebService.DataTypes
{
    //public class GetPhoneFileRequest
    //{
    //    /// <summary>
    //    /// Phone file data
    //    /// </summary>
    //    [JsonProperty(PropertyName = "file")]
    //    public PhoneFileInfo File { get; set; }
    //}

    public class SetPhoneFileRequest
    {
        /// <summary>
        /// Caption
        /// </summary>
        [JsonProperty(PropertyName = "caption")]
        public string Caption { get; set; } // название списка
        
        /// <summary>
        /// File name
        /// </summary>
        [JsonProperty(PropertyName = "file_name")]
        public string FileName { get; set; }

        /// <summary>
        /// File data
        /// </summary>
        [JsonProperty(PropertyName = "file_data")]
        public string FileData { get; set; } // "base64 string" //данные файла

        [JsonIgnore]
        public byte[] DataBin => Convert.FromBase64String(FileData);
    }
}
