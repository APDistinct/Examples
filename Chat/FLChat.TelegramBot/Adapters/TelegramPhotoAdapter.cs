using FLChat.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using Telegram.Bot.Types;

namespace FLChat.TelegramBot.Adapters
{
    /// <summary>
    /// Adapts telegram object for photo 
    /// </summary>
    public class TelegramPhotoAdapter : Core.IInputFile
    {
        public TelegramPhotoAdapter(Message message) {
            if (message.Photo == null || message.Photo.Length == 0)
                throw new ArgumentException("Message property Photo can't be null or empty");
            Message = message;
        }

        /// <summary>
        /// Telegram original message
        /// </summary>
        public Message Message { get; }

        public MediaGroupKind Type => MediaGroupKind.Image;

        public string Media {
            get {
                PhotoSize max = null;
                foreach (PhotoSize ps in Message.Photo) {
                    if (max == null || max.FileSize < ps.FileSize)
                        max = ps;
                }
                return max.FileId;
            }
        }

        /// <summary>
        /// File name, may be null
        /// </summary>
        public string FileName => Media;

        /// <summary>
        /// File media type, may be null
        /// </summary>
        public string MediaType => null;
    }
}
