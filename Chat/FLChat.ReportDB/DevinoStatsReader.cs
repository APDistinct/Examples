using Devino.Logger;
using Devino.Viber;
using FLChat.Devino;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.ReportDB
{
    public interface IDevinoStatsReader
    {
        GetStatusResponse GetInfo(string[] num);
    }

    public class DevinoStatsReader : IDevinoStatsReader
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
            var ret = Sender.ViberGetStatus(list).ConfigureAwait(false).GetAwaiter().GetResult();
            return ret;
        }

        public GetStatusResponse GetInfo(string[] num)
        {
            var message = new GetStatusMessage() { Messages = new List<string>(num) };
            var ret = Sender.ViberGetStatus(message).ConfigureAwait(false).GetAwaiter().GetResult();
            return ret;
        }
    }
}
