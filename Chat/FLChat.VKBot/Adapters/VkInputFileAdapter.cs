using FLChat.Core;
using FLChat.DAL;
using FLChat.VKBotClient.Types;
using FLChat.VKBotClient.Types.Attachments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.VKBot.Adapters
{
    public class VkInputFileAdapter : IInputFile
    {
        public VkInputFileAdapter(Message message)
        {
            foreach (var att in message.Attachments)
            {
                if (att.Doc != null)
                {
                    FileName = att.Doc.Title;
                    Url = att.Doc.Url;
                    if (string.IsNullOrEmpty(FileName))
                        FileName = Url.GetFileNameFromUrl();
                    Type = MediaGroupKind.Document;                    
                    return;
                }
                if (att.Photo != null)
                {
                    Url = GetPhotoUrl(att.Photo);
                    FileName = Url.GetFileNameFromUrl();                    
                    Type = MediaGroupKind.Image;                    
                    return;
                }

            }
        }

        private string GetPhotoUrl(PhotoAttachment photo)
        {
            string[] photoTypes = { "w","z","y","x","m","s","r","q","p","o"};
            foreach(var ch in photoTypes)
            {
                var s = photo.Sizes.Where(ss => ss.Type == ch).FirstOrDefault();
                if(s != null)
                {
                    return s.Url;
                }
            }
            return null;
        }

        /// <summary>
        /// VK original message
        /// </summary>
        public Message Message { get; }

        public MediaGroupKind Type { get; }

        public string Url { get; }

        public string FileName { get; }

        public string Media => Url; //{ get; }

        public string MediaType { get; }

        public static bool IsContainsFile(Message message)
        {
            if (message.Attachments == null)
                return false;

            foreach(var att in message.Attachments)
            {
                if (att.Doc != null || att.Photo != null)
                    return true;
            }
            return false;
        }
    }
}
