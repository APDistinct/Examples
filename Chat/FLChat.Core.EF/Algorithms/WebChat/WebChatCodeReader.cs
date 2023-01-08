using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.Core.Algorithms.WebChat
{
    public class WebChatCodeReader : IWebChatCodeGenerator
    {
        public string Gen(MessageToUser msg) => msg
            .WebChatDeepLink
            .Where(wc => wc.ToUserId == msg.ToUserId)
            .Single()
            .Link
            ?? throw new ArgumentNullException($"Webchat link code is null in MessageToUser idx {msg.Idx}");
    }
}
