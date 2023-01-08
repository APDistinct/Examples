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
    public class VkFileManager : BaseVkFileManager
    {
        public VkFileManager(VkApiConfiguration config) : base(config)
        {
            AttachmentName = "file";
            config.UploadServer = "docs.getMessagesUploadServer";
            config.Save = "docs.save";
        }

        public async Task<List<PhotoAttachment>> GetDocAttachments(IEnumerable<AttachmentFile> docs)
        {
            var uploadServerResponse = await GetMessagesUploadServerResponse<MessagesUploadServerResponse>();
            var uploadPhotoResponse = await UploadPhotoResponse<UploadPhotoResponseInfo>(uploadServerResponse, docs);
            var saveMessagesPhoto = await SaveMessagesPhotoResponse(uploadPhotoResponse);

            var attachments = saveMessagesPhoto.Response.Select(a => a.PhotoAttachment()).ToList();

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
