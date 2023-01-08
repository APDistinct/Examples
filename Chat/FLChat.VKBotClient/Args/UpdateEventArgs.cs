using System;
using FLChat.VKBotClient.Types;

namespace FLChat.VKBotClient.Args
{
    /// <summary>
    /// <see cref="EventArgs"/> containing an <see cref="FLChat.VKBotClient.Types.Update"/>
    /// </summary>
    /// <seealso cref="EventArgs" />
    public class UpdateEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the update.
        /// </summary>
        /// <value>
        /// The update.
        /// </value>
        public Update Update { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateEventArgs"/> class.
        /// </summary>
        /// <param name="update">The update.</param>
        internal UpdateEventArgs(Update update)
        {
            Update = update;
        }
    }
}