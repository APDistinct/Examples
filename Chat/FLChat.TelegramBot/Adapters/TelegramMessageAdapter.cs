using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using FLChat.Core;
using FLChat.DAL;

namespace FLChat.TelegramBot.Adapters
{
    public class TelegramMessageAdapter : IOuterMessage
    {
        public TelegramMessageAdapter(Message message) {
            Message = message;
            if (message.Photo != null) {
                File = new TelegramPhotoAdapter(message);
                Text = message.Caption;
            } else if (message.Document != null) {
                File = new TelegramDocumentAdapter(message);
                Text = message.Caption ?? Message.Text;
            } else
                Text = Message.Text;
        }

        /// <summary>
        /// Telegram original message
        /// </summary>
        public Message Message { get; }

        public string MessageId => Message.MessageId.ToString();

        public string FromId => Message.From.Id.ToString();

        public string FromName {
            get {
                StringBuilder sb = new StringBuilder();
                if (Message.From.FirstName != null)
                    sb.Append(Message.From.FirstName);
                if (Message.From.LastName != null) {
                    if (sb.Length != 0)
                        sb.Append(" ");
                    sb.Append(Message.From.LastName);
                }
                if (Message.From.Username != null) {
                    if (sb.Length != 0)
                        sb.Append(" ");
                    sb.Append(Message.From.Username);
                }
                return sb.ToString();
            }
        }

        public TransportKind TransportKind => TransportKind.Telegram;

        public string Text { get; }

        /// <summary>
        /// Contact's phone number.
        /// Contains value if Telegram message contains field "contact". It happens when button 'send phone number' was pressed.
        /// Otherwise is null
        /// </summary>
        public string PhoneNumber =>
            (Message.Contact != null && Message.Contact.UserId == Message.From.Id) ? Message.Contact.PhoneNumber : null;

        public string AvatarUrl { get; }

        public string DeepLink {
            get {
                if (Message.Entities == null)
                    return null;

                MessageEntity entity = Message.Entities
                    .Where(e => e.Type == MessageEntityType.BotCommand && Message.Text.Substring(e.Offset, e.Length) == "/start")
                    .FirstOrDefault();
                if (entity != null && Message.Text.Length > entity.Offset + entity.Length + 1) {
                    return Message.Text.Substring(entity.Offset + entity.Length + 1);
                    //int index = Array.IndexOf(Message.Entities, entity);
                    //return Message.EntityValues.Skip(index).FirstOrDefault();
                } else
                    return null;
            }
        }

        /// <summary>
        /// Message's id which current message is replied for or null
        /// </summary>
        public string ReplyToMessageId => Message.ReplyToMessage?.MessageId.ToString();

        public bool IsTransportEnabled => true;

        public Core.IInputFile File { get; }
    }
}
