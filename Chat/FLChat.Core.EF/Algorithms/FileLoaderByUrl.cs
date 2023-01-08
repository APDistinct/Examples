using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;
using System.Net.Http;
using FLChat.DAL;
using System.Net;

namespace FLChat.Core.Algorithms
{
    public class FileLoaderByUrl : IFileLoader, IDisposable
    {
        private readonly HttpClient _client = new HttpClient();

        public FileLoaderByUrl() {
            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls12;
        }

        public void Dispose() {
            _client.Dispose();
        }

        public DownloadFileResult Download(IInputFile file) {
            Task<DownloadFileResult> task = DownloadAsync(file);
            task.Wait();
            return task.Result;
        }

        public async Task<DownloadFileResult> DownloadAsync(IInputFile file) {
            HttpResponseMessage resp = await _client.GetAsync(file.Media);

            resp.EnsureSuccessStatusCode();

            byte[] data = await resp.Content.ReadAsByteArrayAsync();

            string mediaType = file.MediaType;            
            if (mediaType == null && resp.Headers.TryGetValues("Content-Type", out IEnumerable<string> values)) 
                mediaType = values.First();

            if (mediaType == "unknown")
                mediaType = null;

            return new DownloadFileResult(file, mediaType, data);
        }

    }
}
