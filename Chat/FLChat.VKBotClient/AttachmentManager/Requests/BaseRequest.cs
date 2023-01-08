using System.Net.Http;

namespace FLChat.VKBotClient.AttachmentManager.Requests
{
    public class BaseRequest
    {
        protected VkApiConfiguration Config;
        
        protected BaseRequest(VkApiConfiguration config)
        {
            Config = config;
        }


        protected string GetUrl(string method)
        {
            return $"{Config.DefaultBaseUrl}{method}?access_token={Config.Token}&v={Config.Version}";
        }
    }
}
