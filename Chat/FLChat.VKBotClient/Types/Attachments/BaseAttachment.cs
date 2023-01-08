using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace FLChat.VKBotClient.Types.Attachments
{
    public class BaseAttachment : IAttachment
    {
        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AttachmentType Type { get; set ; }  
    }
}
