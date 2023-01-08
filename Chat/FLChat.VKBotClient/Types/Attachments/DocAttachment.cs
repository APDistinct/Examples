using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.VKBotClient.Types.Attachments
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class DocAttachment 
    {        
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        
        [JsonProperty(PropertyName = "owner_id")]
        public int OwnerId { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "size")]
        public int Size { get; set; }

        [JsonProperty(PropertyName = "ext")]
        public string Ext { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "date")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Date { get; set; }
        
        [JsonProperty(PropertyName = "type")]
        public int Type { get; set; }

        [JsonProperty(PropertyName = "preview")]
        public PreviewDocAttachment Preview { get; set; }

        //public int sizes { get; set; }
    }

    public class PreviewDocAttachment
    {
        [JsonProperty("photo")]
        public PhotoDocAttachment Photo { get; set; }
    }

    public class PhotoDocAttachment
    {
        [JsonProperty("sizes")]
        public SizePhotoDocAttachment[] Sizes { get; set; }
    }


    public class SizePhotoDocAttachment
    {
        [JsonProperty("src")]
        public string Src { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
