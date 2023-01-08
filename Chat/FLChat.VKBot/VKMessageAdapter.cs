using FLChat.Core;
using FLChat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.VKBotClient.Types;
using Newtonsoft.Json;
using FLChat.VKBot.Adapters;

namespace FLChat.VKBot
{    
    public class VKMessageAdapter : IOuterMessage
    {
        public VKMessageAdapter(Message message)
        {
            Message = message;
            if (VkInputFileAdapter.IsContainsFile(message))
                File = new VkInputFileAdapter(message);
        }

        /// <summary>
        /// Telegram original message
        /// </summary>
        public Message Message { get; }


        public string MessageId => Message.Id.ToString();

        public string FromId => Message.FromId.ToString();

        public string FromName { get; set; }
        //String.Concat(Message.From.Username, " ", Message.From.FirstName, " ", Message.From.LastName);

        public TransportKind TransportKind => TransportKind.VK;

        public string Text => Message.Text;

        /// <summary>
        /// Contact's phone number.
        /// Contains value if Telegram message contains field "contact". It happens when button 'send phone number' was pressed.
        /// Otherwise is null
        /// </summary>
        public string PhoneNumber { get; set; }

        public string AvatarUrl { get; }
        //(Message.Contact != null && Message.Contact.UserId == Message.From.Id) ? Message.Contact.PhoneNumber : null;

        public string DeepLink => Message.Ref;
        /// <summary>
        /// ? Есть возможность отдать номер
        /// </summary>
        public string ReplyToMessageId => null;
        /// <summary>
        /// Comman fo pressed button
        /// </summary>
        public string Command => Message.Payload != null ? 
            (JsonConvert.DeserializeObject <VKPayloadConverter> (Message.Payload)).Button :
            null;

        public bool IsTransportEnabled => true;

        public IInputFile File { get; }
    }
}
