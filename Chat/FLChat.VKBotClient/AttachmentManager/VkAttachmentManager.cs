using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FLChat.VKBotClient.AttachmentManager;
using FLChat.VKBotClient.AttachmentManager.Requests;
using FLChat.VKBotClient.AttachmentManager.Responses;
using FLChat.VKBotClient.Types;
using FLChat.VKBotClient.Types.Attachments;
using Newtonsoft.Json;

namespace FLChat.VKBotClient.AttachmentManager
{
    public class VkAttachmentManager
    {
        private readonly VkPhotoManager _vkPhotoManager;
        private readonly VkDocManager _vkDocManager;

        public VkAttachmentManager()
        {
            _vkPhotoManager = new VkPhotoManager(new VkApiConfiguration());
            _vkDocManager = new VkDocManager(new VkApiConfiguration());
        }

        public async Task<List<string>> GetAttachments(IEnumerable<AttachmentFile> files, string userId)
        {
            
            var attachments = new List<string>();
            var pfiles = files.Where(a => a.Type == AttachmentType.Photo);
            if (pfiles.Any())
            {
                var photos = await GetPhotoAttachments(pfiles, userId);
                var plist = photos.Select(a => $"photo{a.OwnerId}_{a.Id}_{a.AccessKey}");
                    attachments.AddRange(plist);
            }
            var dfiles = files.Where(a => a.Type == AttachmentType.Doc);
            if (dfiles.Any())
            {
                var docs = await GetDocAttachments(dfiles, userId);
                var dlist = docs.Select(a => $"doc{a.OwnerId}_{a.Id}");
                    attachments.AddRange(dlist);
            }
            return attachments;
        }

        public async Task<List<SaveMessagesPhotoResponseInfo>> GetPhotoAttachments(IEnumerable<AttachmentFile> photos, string userId)
        {
            return await _vkPhotoManager.GetPhotoAttachments(photos, userId);
        }

        public async Task<List<DocAttachment>> GetDocAttachments(IEnumerable<AttachmentFile> docs, string userId)
        {
            return await _vkDocManager.GetDocAttachments(docs, userId);
        }
    }
}
