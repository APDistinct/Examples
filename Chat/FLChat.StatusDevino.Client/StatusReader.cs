using Devino.Logger;
using Devino.Viber;
using FLChat.Devino;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FLChat.StatusDevino.Client
{
    public interface IStatusReader
    {
        Task<GetStatusResponse> GetInfo(string[] num);
        Task<GetStatusResponse> GetInfo(string[] num, CancellationToken ct);
    }

    public class StatusReader : IStatusReader
    {
        //private IDevinoLogger _logger;
        private static DevinoProvider Sender;

        public StatusReader(IDevinoLogger logger = null)
        {
            //_logger = logger ?? GetLogger(TransportKind.WebChat);
            Sender = new DevinoProvider(logger ?? DevinoLogger.GetLogger(DAL.TransportKind.WebChat));
        }
        
        public Task<GetStatusResponse> GetInfo(string[] num)
        {
            var message = new GetStatusMessage() { Messages = new List<string>(num) };
            var ret = Sender.ViberGetStatus(message)/*.ConfigureAwait(false).GetAwaiter().GetResult()*/;
            return ret;
        }

        public Task<GetStatusResponse> GetInfo(string[] num, CancellationToken ct)
        {
            var message = new GetStatusMessage() { Messages = new List<string>(num) };
            var ret = Sender.ViberGetStatus(message, ct)/*.ConfigureAwait(false).GetAwaiter().GetResult()*/;
            return ret;
        }
    }
}
