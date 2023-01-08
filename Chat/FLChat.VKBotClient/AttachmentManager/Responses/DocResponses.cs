using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.VKBotClient.Types;
using FLChat.VKBotClient.Types.Attachments;
using Newtonsoft.Json;

namespace FLChat.VKBotClient.AttachmentManager.Responses
{
    public class DocUploadServerResponse : VkError
    {
        [JsonProperty(PropertyName = "response")]
        public DocUploadServerResponseInfo Response { get; set; }
    }

    public class DocUploadServerResponseInfo
    {
        [JsonProperty(PropertyName = "upload_url")]
        public string UploadUrl { get; set; }
    }

    public class UploadDocResponseInfo
    {
        [JsonProperty(PropertyName = "file")]
        public string File { get; set; }

    }

    public class SaveMessagesDocResponse
    {
        [JsonProperty(PropertyName = "response")]
        public SaveMessagesDocResponseInfo Response { get; set; }
    }

    public class SaveMessagesDocResponseInfo
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("doc")]
        public DocAttachment Doc { get; set; }
    }
}
