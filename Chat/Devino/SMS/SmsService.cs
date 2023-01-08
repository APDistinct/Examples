using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Devino.Logger;
using Devino.SMS;
using FLChat.Devino.Request;
using FLChat.Devino.SMS.Request;

namespace FLChat.Devino.SMS
{
    public class SmsService : AbstractHttpService
    {
        public SmsService(IDevinoLogger logger) : base(logger)
        {
            Method = "rest/v2";
        }

        public async Task<SessionIdResponse> GetSessionId(string login, string password)
        {
            Method = "rest";
            var request = new SessionIdRequest();
            request.Params.Add(new Param("login", login));
            request.Params.Add(new Param("password", password));
            return await MakeRequestAsync(request, new CancellationToken());
        }

        public async Task<List<string>> SmsSend(List<string> destinationAddresses, string data, string sourceAddress, string sessionId)
        //public async Task<SendBulkResponse> SmsSend(List<string> destinationAddresses, string data, string sourceAddress, string sessionId)
        {
            Method = "rest";
            var request = new SendBulkRequest();
            request.Params.Add(new Param("sessionId", sessionId));
            request.Params.Add(new Param("sourceAddress", sourceAddress));
            request.Params.Add(new Param("data", data));
            request.Params.Add(new Param("validity", "0"));

            foreach (var address in destinationAddresses)
            {
                request.DestinationAddresses.Add(address);
            }

            return await MakeRequestAsync(request, new CancellationToken());
        }

        public async Task<List<List<string>>> SmsSend(List<string> destinationAddresses, string data, string sourceAddress, string login, string password)
        //public async Task<SendBulkResponse> SmsSend(List<string> destinationAddresses, string data, string sourceAddress, string sessionId)
        {
            Method = "rest/v2";
            var request = new SendBulkRequest();
            request.Params.Add(new Param("login", login));
            request.Params.Add(new Param("password", password));
            request.Params.Add(new Param("sourceAddress", sourceAddress));
            request.Params.Add(new Param("data", data));
            request.Params.Add(new Param("validity", "0"));

            foreach (var address in destinationAddresses)
            {
                request.DestinationAddresses.Add(address);
            }

            var requestResult = await MakeRequestAsync(request, new CancellationToken());

            var result = SplitHelper.SplitMessages(requestResult, destinationAddresses.Count);

            return result;
        }

    }

}
