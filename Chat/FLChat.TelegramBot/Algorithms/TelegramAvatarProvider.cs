using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.Core;
using FLChat.DAL.Model;

namespace FLChat.TelegramBot.Algorithms
{
    public class TelegramAvatarProvider :IAvatarProvider
    {
        private readonly TelegramClient _telegramClient;

        public TelegramAvatarProvider(TelegramClient telegramClient)
        {
            this._telegramClient = telegramClient;
        }

        public byte[] GetAvatarPicture(string messageAvatarUrl, string outerSystemUserId)
        {
            var photosTask = _telegramClient.Client.GetUserProfilePhotosAsync(Convert.ToInt32(outerSystemUserId));
            photosTask.Wait();
            var photos = photosTask.Result;
            if (photos.TotalCount > 0)
            {
                var photo = photos.Photos[0].OrderByDescending(i => i.FileSize).FirstOrDefault();
                if (photo != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        var fileAsync = _telegramClient.Client.GetFileAsync(photo.FileId);
                        fileAsync.Wait();
                        var image = fileAsync.Result;
                        var task = _telegramClient.Client.DownloadFileAsync(image.FilePath, ms);
                        task.Wait();
                        return ms.ToArray();
                    }
                }
            }
            
            return null;
        }
    }
}
