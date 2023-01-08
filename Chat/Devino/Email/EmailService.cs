using RestApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Devino.Logger;
using FLChat.Devino;
using FLChat.Devino.Request;
using FLChat.Devino.DvTypes;

namespace FLChat.Devino.EMAIL
{
    public class EmailService : AbstractHttpService
    {
        public EmailService(string login, string password, IDevinoLogger logger) : base(logger)
        {
            Method = "email/v2";

            var byteArray = Encoding.ASCII.GetBytes($"{login}:{password}");
            var auth = Convert.ToBase64String(byteArray);
            var header = new AuthenticationHeaderValue("Basic", auth);
            HttpClient.DefaultRequestHeaders.Authorization = header;
        }

        public Task<SourceAddressesResponse> SendSourceAddresses(CancellationToken cancellationToken)
        {
            var request = new SourceAddressesRequest();
            return MakeRequestAsync(request, cancellationToken);
        }

        public Task<SendEmailResponse> SendEmail(DvEmailRequest dvEmail, CancellationToken cancellationToken)
        {
            var request = new SendEmailRequest(){ DvEmail = dvEmail};
            return MakeRequestAsync(request, cancellationToken);
        }
    }
}
