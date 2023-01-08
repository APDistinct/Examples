using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.VKBotClient.Types.Attachments;

namespace FLChat.VKBotClient.Types
{
    public class AttachmentFile
    {
        public string Name { get; set; }
        public byte[] Bytes { get; set; }
        public AttachmentType Type { get; set; }
    }
}
