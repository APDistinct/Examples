using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FLChat.VKBotClient.Types
{
    /// <summary>
    /// The type of an <see cref="Update"/>
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter), true)]
    public enum UpdateType
    {
        /// <summary>
        /// Update Type is unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The <see cref="Update"/> contains a <see cref="Types.Message"/>.
        /// </summary>
        Message,

        /// <summary>
        /// The <see cref="Update"/> contains an edited <see cref="Types.Message"/>
        /// </summary>
        EditedMessage,

    }
}
