using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Viber.Client.Types
{
    public enum MessageType
    {
        [EnumMember(Value = "text")]
        Text,

        [EnumMember(Value = "picture")]
        Picture,

        [EnumMember(Value = "video")]
        Video,

        [EnumMember(Value = "file")]
        File,

        [EnumMember(Value = "location")]
        Location,

        [EnumMember(Value = "contact")]
        Contact,

        [EnumMember(Value = "sticker")]
        Sticker,

        [EnumMember(Value = "url")]
        Url,
        
        [EnumMember(Value = "rich_media")]
        Carousel
    }
}
