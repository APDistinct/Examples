using System.Net.Http;


namespace FLChat.VKBotClient.AttachmentManager.Requests
{
    public class GetServerRequest : BaseRequest
    {
        public GetServerRequest(VkApiConfiguration config) : base(config)
        {
        }

        public HttpRequestMessage GetRequestMessage(string userId)
        {
            //return new HttpRequestMessage(HttpMethod.Get, $"{GetUrl(Config.UploadServer)}");
            return new HttpRequestMessage(HttpMethod.Get, $"{GetUrl(Config.UploadServer)}&peer_id={userId}");
        }
    }
}
