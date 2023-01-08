using Devino.Logger;
using Devino.Viber;
using FLChat.DAL;
using FLChat.Devino;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Reports
{
    public class DevinoStatsReader
    {
        //private IDevinoLogger _logger;
        private static DevinoProvider Sender;

        public DevinoStatsReader(IDevinoLogger logger = null)
        {
            //_logger = logger ?? GetLogger(TransportKind.WebChat);
            Sender = new DevinoProvider(logger ?? DevinoLogger.GetLogger(DAL.TransportKind.WebChat));
        }

        public GetStatusResponse GetInfo(string num)
        {
            var list = new GetStatusMessage() { Messages = new List<string>() { num } };
            var ret = Sender.ViberGetStatus( list).ConfigureAwait(false).GetAwaiter().GetResult();
            return ret;
        }

        public GetStatusResponse GetInfo(string[] num)
        {
            var message = new GetStatusMessage() {Messages = new List<string>(num)};
            var ret = Sender.ViberGetStatus(message).ConfigureAwait(false).GetAwaiter().GetResult();
            return ret;
        }

        //private IDevinoLogger GetLogger(TransportKind transportKind)
        //{
        //    var writer = new DevinoLogWriter(true, transportKind);
        //    var log = new DevinoLogger();
        //    log.MakingApiRequest += writer.Request;
        //    log.ApiResponseReceived += writer.Response;
        //    log.ApiRequestException += writer.Exception;

        //    return log;
        //}
    }
}
