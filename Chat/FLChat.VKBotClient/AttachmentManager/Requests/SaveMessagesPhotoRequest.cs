using System.Net.Http;
using FLChat.VKBotClient.AttachmentManager.Responses;

namespace FLChat.VKBotClient.AttachmentManager.Requests
{
    public class SaveMessagesPhotoRequest : BaseRequest
    {
        public SaveMessagesPhotoRequest(VkApiConfiguration config) : base(config)
        {
        }

        public HttpRequestMessage GetRequestMessage(UploadPhotoResponseInfo uploadPhotoResponse)
        {
            var url =
                $"{GetUrl(Config.Save)}&photo={uploadPhotoResponse.Photo}&server={uploadPhotoResponse.Server}&hash={uploadPhotoResponse.Hash}";
            return new HttpRequestMessage(HttpMethod.Get, url);
        }
    }
}
