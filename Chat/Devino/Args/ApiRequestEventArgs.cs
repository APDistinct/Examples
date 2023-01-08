using System.Net.Http;

namespace Devino.Args
{
    public class ApiRequestEventArgs : EventArgs
    {
        public string MethodName { get; internal set; }

        public string Url { get; internal set; }

        public HttpContent HttpContent { get; internal set; }
    }
}
