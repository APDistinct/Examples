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
    public class VkPhotoManager : BaseVkFileManager
    {
        public VkPhotoManager(VkApiConfiguration config) : base(config)
        {
            AttachmentName = "photo";
            config.UploadServer = "photos.getMessagesUploadServer";
            config.Save = "photos.saveMessagesPhoto";
        }

        public async Task<List<SaveMessagesPhotoResponseInfo>> GetPhotoAttachments(IEnumerable<AttachmentFile> photos, string userId)
        {
            var uploadServerResponse = await GetMessagesUploadServerResponse<MessagesUploadServerResponse>(userId);
            var uploadPhotoResponse = await UploadPhotoResponse<UploadPhotoResponseInfo>(uploadServerResponse.Response.UploadUrl, photos);
            var saveMessagesPhoto = await SaveMessagesPhotoResponse(uploadPhotoResponse);

            var attachments = saveMessagesPhoto.Response.ToList();

            return attachments;
        }

        private async Task<SaveMessagesPhotoResponse> SaveMessagesPhotoResponse(UploadPhotoResponseInfo uploadPhotoResponse)
        {
            var httpRequest = new SaveMessagesPhotoRequest(Config).GetRequestMessage(uploadPhotoResponse);
            var httpResponse = await _httpClient.SendAsync(httpRequest);
            var responseJson = await httpResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SaveMessagesPhotoResponse>(responseJson);

            return result;
        }
    }
}
