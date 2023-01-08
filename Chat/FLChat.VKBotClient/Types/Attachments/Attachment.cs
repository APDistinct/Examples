using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FLChat.VKBotClient.Types.Attachments
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class Attachment : BaseAttachment
    {
        [JsonProperty(PropertyName = "photo", NullValueHandling = NullValueHandling.Ignore)]
        public PhotoAttachment Photo { get; set; }
        [JsonProperty(PropertyName = "doc", NullValueHandling = NullValueHandling.Ignore)]
        public DocAttachment Doc { get; set; }


        //public IAttachment GetAttachment(AttachmentType type)
        //{
        //    IAttachment ret = null;
        //    switch (type)
        //    {
        //        case AttachmentType.Doc:
        //            ret = Doc;
        //            break;
        //        case AttachmentType.AvatarUrl:
        //            ret = AvatarUrl;
        //            break;
        //    }
        //    return ret;
        //}
    }
}
