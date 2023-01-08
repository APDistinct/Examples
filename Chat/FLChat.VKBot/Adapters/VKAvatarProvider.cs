using System;
using System.Linq;
using System.Threading;
using FLChat.Core;
using FLChat.Core.Algorithms;

namespace FLChat.VKBot.Adapters
{
    public class VKAvatarProvider : IAvatarProvider
    {
        private readonly VKClient _vkClient;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        /// <param name="vkClient"></param>
        public VKAvatarProvider(VKClient vkClient)
        {
            _vkClient = vkClient;
        }

        public byte[] GetAvatarPicture(string messageAvatarUrl, string outerSystemUserId)
        {
            if (_vkClient != null)
            {
                try
                {
                    var task = _vkClient.Client.GetUserInfoAsync(outerSystemUserId, CancellationToken.None);
                    task.Wait();
                    messageAvatarUrl = task.Result.User.FirstOrDefault().AvatarUrl;

                    var downloadFileResult = new FileLoaderByUrl().Download(new VKAvatarIconFileAdapter(messageAvatarUrl));

                    if (downloadFileResult.Data.Length > 0)
                        return downloadFileResult.Data;
                }
                catch (Exception)
                { }
            }
            return new byte[0];
        }
    }
}