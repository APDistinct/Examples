using System;
using System.Collections.Generic;
using System.Linq;
using FLChat.VKBotClient.Types.Attachments;
using Newtonsoft.Json;

namespace FLChat.VKBotClient.AttachmentManager.Responses
{
    public class MessagesUploadServerResponse
    {
        [JsonProperty(PropertyName = "response")]
        public MessagesUploadServerResponseInfo Response { get; set; }
    }

    public class UploadPhotoResponseInfo
    {
        [JsonProperty(PropertyName = "server")]
        public int Server { get; set; }
        [JsonProperty(PropertyName = "photo")]
        public string Photo { get; set; }
        [JsonProperty(PropertyName = "hash")]
        public string Hash { get; set; }
    }

    public class MessagesUploadServerResponseInfo
    {
        [JsonProperty(PropertyName = "album_id")]
        public string AlbumId { get; set; }
        [JsonProperty(PropertyName = "upload_url")]
        public string UploadUrl { get; set; }
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }
        [JsonProperty(PropertyName = "group_id")]
        public string GroupId { get; set; }
    }

    public partial class SaveMessagesPhotoResponse
    {
        [JsonProperty(PropertyName = "response")]
        public SaveMessagesPhotoResponseInfo[] Response { get; set; }
    }

    public partial class SaveMessagesPhotoResponseInfo
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("album_id")]
        public long AlbumId { get; set; }

        [JsonProperty("owner_id")]
        public long OwnerId { get; set; }

        [JsonProperty("user_id")]
        public long UserId { get; set; }

        [JsonProperty("sizes")]
        public SaveMessagesPhotoSizeResponseInfo[] Sizes { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("date")]
        public /*DateTime*/ long Date { get; set; }

        [JsonProperty("access_key")]
        public string AccessKey { get; set; }

        public PhotoAttachment PhotoAttachment()
        {
            return new PhotoAttachment()
            {
                    Id = Id,
                    AlbumId = AlbumId,
                    OwnerId = OwnerId,
                    UserId = UserId,
                    Text = Text,                    
                    Sizes = Sizes.Select(a => new Size { Height = a.Height, Type = a.Type, Url = a.Url, Width = a.Width}).ToList(),
                    
            };
        }
    }

    public partial class SaveMessagesPhotoSizeResponseInfo
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }
}
