using System;
using FLChat.Core;
using FLChat.Core.Algorithms;
using FLChat.DAL.Model;
using FLChat.Viber.Bot.Adapters;

namespace FLChat.Viber.Bot.Algorithms
{
    public class ViberAvatarProvider : IAvatarProvider
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public byte[] GetAvatarPicture(string messageAvatarUrl, string outerSystemUserId)
        {
            DownloadFileResult downloadFileResult;
         
            downloadFileResult = new FileLoaderByUrl().Download(new ViberAvatarIconFileAdapter(messageAvatarUrl));

            if (downloadFileResult.Data.Length > 0)
                return downloadFileResult.Data;

            return downloadFileResult.Data;
        }
    }
}