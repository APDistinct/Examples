using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.VKBotClient.Types.Attachments
{
    public interface IAttachment
    {
        AttachmentType Type { set; get; }
    }
}
