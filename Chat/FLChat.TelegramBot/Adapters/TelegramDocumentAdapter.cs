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
    /// Adapts telegram message with document
    /// </summary>
    public class TelegramDocumentAdapter : Core.IInputFile
    {
        public TelegramDocumentAdapter(Message message) {
            if (message.Document == null)
                throw new ArgumentException("Message property document can't be null");
            Message = message;
        }

        /// <summary>
        /// Telegram original message
        /// </summary>
        public Message Message { get; }

        public MediaGroupKind Type => MediaGroupKind.Document;

        public string Media => Message.Document.FileId;

        public string FileName => Message.Document.FileName;

        public string MediaType => Message.Document.MimeType;
    }
}
