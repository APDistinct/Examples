using System.Net.Http;
using FLChat.VKBotClient.AttachmentManager.Responses;

namespace FLChat.VKBotClient.AttachmentManager.Requests
{
    class UploadPhotoRequest : BaseRequest
    {
        public UploadPhotoRequest(VkApiConfiguration config) : base(config)
        {
        }

        public HttpRequestMessage GetRequestMessage(string uploadUrl)
        {
            return new HttpRequestMessage(HttpMethod.Post, uploadUrl);
        }
    }
}
