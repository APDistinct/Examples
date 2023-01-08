using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using FLChat.Viber.Client.Types;
using System.IO;

namespace FLChat.Viber.Client.Requests
{
    /// <summary>
    /// Send file message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class SendFileMessageRequest : SendMessageRequest
    {
        public const int MaxFileSize = 50 * 1024 * 1024;
        public const int FileNameMaxLength = 256;

        public SendFileMessageRequest(Sender sender, string receiver, string media, int size, string fileName) 
            : base(sender, receiver, MessageType.File) {
            Media = media;
            Size = size;
            if (fileName.Length <= FileNameMaxLength)
                FileName = fileName;
            else {
                int index = fileName.LastIndexOf('.');
                if (index >= 0)
                    FileName = fileName.Substring(0, FileNameMaxLength - (fileName.Length - index)) 
                        + fileName.Substring(index);
                else
                    FileName = fileName.Substring(0, FileNameMaxLength);
            }
        }

        /// <summary>
        /// URL of the file
        /// required. Max size 50 MB.
        /// </summary>
        [JsonProperty]
        public string Media { get; set; }

        /// <summary>
        /// Size of the file in bytes
        /// required
        /// </summary>
        [JsonProperty]
        public int Size { get; set; }

        /// <summary>
        /// Name of the file
        /// required. File name should include extension. Max 256 characters (including file extension). 
        /// Sending a file without extension or with the wrong extension might cause the client to be unable to open the file
        /// </summary>
        [JsonProperty]
        public string FileName { get; set; }
    }
}
