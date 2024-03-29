﻿using System;
using FLChat.VKBotClient.Exceptions;

namespace FLChat.VKBotClient.Args
{
    /// <summary>
    /// <see cref="EventArgs"/> containing an <see cref="Telegram.Bot.Exceptions.ApiRequestException"/>
    /// </summary>
    /// <seealso cref="EventArgs" />
    public class ReceiveErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the API request exception.
        /// </summary>
        /// <value>
        /// The API request exception.
        /// </value>
        public ApiRequestException ApiRequestException { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceiveErrorEventArgs"/> class.
        /// </summary>
        /// <param name="apiRequestException">The API request exception.</param>
        internal ReceiveErrorEventArgs(ApiRequestException apiRequestException)
        {
            ApiRequestException = apiRequestException;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Telegram.Bot.Exceptions.ApiRequestException"/> to <see cref="ReceiveErrorEventArgs"/>.
        /// </summary>
        /// <param name="e">The <see cref="Telegram.Bot.Exceptions.ApiRequestException"/></param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ReceiveErrorEventArgs(ApiRequestException e) => new ReceiveErrorEventArgs(e);
    }
}
