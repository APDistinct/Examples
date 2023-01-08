using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.VKBot.Types
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

    }
}
