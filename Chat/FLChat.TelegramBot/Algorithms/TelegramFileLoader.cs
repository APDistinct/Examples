using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using FLChat.Core;
using FLChat.Core.Algorithms;
using System.Net;
using Telegram.Bot;
using FLChat.DAL;
using MihaZupan;
using Telegram.Bot.Types;
using System.Threading;


namespace FLChat.TelegramBot.Algorithms
{
    public class TelegramFileLoader : TelegramBotClientHandler, IFileLoader
    {
        private readonly FileLoaderByUrl _loader = new FileLoaderByUrl();

        public TelegramFileLoader(string token, string proxyAddr, int proxyPort, string proxyUser, string proxyPsw)
            : base(token, proxyAddr, proxyPort, proxyUser, proxyPsw) {
        }


        public DownloadFileResult Download(Core.IInputFile file) {
            Task<DownloadFileResult> task = DownloadAsync(file, CancellationToken.None);
            task.Wait();
            return task.Result;
        }

        public async Task<DownloadFileResult> DownloadAsync(Core.IInputFile file, CancellationToken ct) {
            MemoryStream stream = new MemoryStream();
            Telegram.Bot.Types.File tgFile = await Client.GetInfoAndDownloadFileAsync(file.Media, stream, ct);
            
            return new DownloadFileResult(new Adapters.TgFileAdapter(file, tgFile), file.MediaType, stream.ToArray());
        }
    }
}
