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
    public class VkDocManager : BaseVkFileManager
    {
        public VkDocManager(VkApiConfiguration config) : base(config)
        {
            AttachmentName = "file";
            config.UploadServer = "docs.getMessagesUploadServer";
            config.Save = "docs.save";
        }

        public async Task<List<DocAttachment>> GetDocAttachments(IEnumerable<AttachmentFile> docs, string userId)
        {
            var uploadServerResponse = await GetMessagesUploadServerResponse<DocUploadServerResponse>(userId);
            //  Проверка на отправку - пустое ли поле Error?
            if (uploadServerResponse.Error != null)
            {
                //  Ошибка отправки, надо всё запротоколировать и прекратить работу
                throw new /*Exception*/AggregateException($"Ошибка обработки файла {uploadServerResponse.Error.Code} : {uploadServerResponse.Error.Msg}");
            }
            var uploadDocResponse = await UploadPhotoResponse<UploadDocResponseInfo>(uploadServerResponse.Response.UploadUrl, docs);
            var saveMessagesPhoto = await SaveMessagesDocResponse(uploadDocResponse);

            var attachments = new List<DocAttachment> {saveMessagesPhoto.Response.Doc};

            return attachments;
        }

        private async Task<SaveMessagesDocResponse> SaveMessagesDocResponse(UploadDocResponseInfo uploadDocResponse)
        {
            var httpRequest = new SaveDocRequest(Config).GetRequestMessage(uploadDocResponse);
            var httpResponse = await _httpClient.SendAsync(httpRequest);
            var responseJson = await httpResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SaveMessagesDocResponse>(responseJson);

            return result;
        }
    }
}
