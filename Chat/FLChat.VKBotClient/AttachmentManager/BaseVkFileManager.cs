using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FLChat.VKBotClient.AttachmentManager.Requests;
using FLChat.VKBotClient.AttachmentManager.Responses;
using FLChat.VKBotClient.Types;
using FLChat.VKBotClient.Types.Attachments;
using Newtonsoft.Json;

namespace FLChat.VKBotClient.AttachmentManager
{
    public abstract class BaseVkFileManager
    {
        protected string AttachmentName { get; set; }
        protected readonly HttpClient _httpClient;
        protected VkApiConfiguration Config;

        protected BaseVkFileManager(VkApiConfiguration config)
        {
            Config = config;
            _httpClient = new HttpClient();
        }

        public async Task<T> GetMessagesUploadServerResponse<T>(string userId)
        {
            var request = new GetServerRequest(Config).GetRequestMessage(userId);
            var response = await _httpClient.SendAsync(request);
            var responseJson = await response.Content.ReadAsStringAsync();
            //Проверка на ошибки?
            var result = JsonConvert.DeserializeObject<T>(responseJson);

            return result;
        }

        protected async Task<T> UploadPhotoResponse<T>(string uploadUrl, IEnumerable<AttachmentFile> files)
        {
            var httpRequest = new UploadPhotoRequest(Config).GetRequestMessage(uploadUrl);

            var byteContents = files.Select(a => new {Content = new ByteArrayContent(a.Bytes, 0, a.Bytes.Length), a.Name});

            var content = new MultipartFormDataContent();
            foreach (var byteContent in byteContents)
            {
                content.Add(byteContent.Content, AttachmentName, byteContent.Name);
            }

            httpRequest.Content = content;

            var httpResponse = await _httpClient.SendAsync(httpRequest);
            var responseJson = await httpResponse.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<T>(responseJson);

            return result;
        }
    }
}
