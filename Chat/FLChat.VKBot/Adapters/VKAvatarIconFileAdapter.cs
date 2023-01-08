using FLChat.DAL;

namespace FLChat.VKBot.Adapters
{
    public class VKAvatarIconFileAdapter:Core.IInputFile

    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public VKAvatarIconFileAdapter(string media)
        {
            Type = MediaGroupKind.Image;
            Media = media;
        }

        /// <summary>
        /// File media type
        /// </summary>
        public MediaGroupKind Type { get; }

        /// <summary>
        /// Url to file or some information about how to get file
        /// </summary>
        public string Media { get; }

        /// <summary>
        /// File name, may be null
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// File media type, may be null
        /// </summary>
        public string MediaType { get; }
    }
}