﻿using System;
using System.Net.Http;

namespace FLChat.VKBotClient.Args
{
    /// <summary>
    /// Provides data for MakingApiRequest event
    /// </summary>
    public class ApiRequestEventArgs : EventArgs
    {
        /// <summary>
        /// Bot API method name
        /// </summary>
        public string MethodName { get; internal set; }
        public string Url { get; internal set; }

        /// <summary>
        /// HTTP content of the request message
        /// </summary>
        //public HttpContent HttpContent { get; internal set; }
        public string HttpContent { get; internal set; }
    }
}