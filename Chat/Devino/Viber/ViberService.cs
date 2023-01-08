using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Devino.Logger;
using FLChat.Devino;

namespace Devino.Viber
{
    public class ViberService : AbstractHttpService
    {
        public ViberService(string login, string password, IDevinoLogger logger) : base(logger)
        {
            Host = "viber.devinotele.com:444";
            var byteArray = Encoding.ASCII.GetBytes($"{login}:{password}");
            var auth = Convert.ToBase64String(byteArray);
            var header = new AuthenticationHeaderValue("Basic", auth);
            HttpClient.DefaultRequestHeaders.Authorization = header;
        }

        public Task<SendMessageResponse> Send(ViberSendMessage message, CancellationToken cancellationToken)
        {
            var request = new SendMessageRequest() { Message = message };
            return MakeRequestAsync(request, cancellationToken);
        }

        public Task<GetStatusResponse> GetStatus(GetStatusMessage message, CancellationToken cancellationToken)
        {
            var request = new GetStatusRequest() { Message = message };
            return MakeRequestAsync(request, cancellationToken);
        }
    }
}
