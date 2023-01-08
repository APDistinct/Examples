using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FLChat.VKBotClient.AttachmentManager.Responses;

namespace FLChat.VKBotClient.AttachmentManager.Requests
{
    public class SaveDocRequest : BaseRequest
    {
        public SaveDocRequest(VkApiConfiguration config) : base(config)
        {
        }

        public HttpRequestMessage GetRequestMessage(UploadDocResponseInfo upload)
        {
            var url = $"{GetUrl(Config.Save)}&file={upload.File}";
            return new HttpRequestMessage(HttpMethod.Get, url);
        }
    }
}
