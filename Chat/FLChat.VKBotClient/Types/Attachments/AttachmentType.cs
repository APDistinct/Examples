using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace FLChat.VKBotClient.Types.Attachments
{    
    public enum AttachmentType
    {
        [EnumMember(Value = "photo")]
        Photo,        

        [EnumMember(Value = "video")]
        Video,

        [EnumMember(Value = "audio")]
        Audio,

        [EnumMember(Value = "doc")]
        Doc,

        [EnumMember(Value = "link")]
        Link,

        [EnumMember(Value = "market")]
        Market,

        [EnumMember(Value = "market_album")]
        MarketAlbum,

        [EnumMember(Value = "wall")]
        Wall,

        [EnumMember(Value = "wall_reply")]
        WallReply,

        [EnumMember(Value = "sticker")]
        Sticker,

        [EnumMember(Value = "gift")]
        Gift,
    }
}
