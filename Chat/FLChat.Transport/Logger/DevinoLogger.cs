using FLChat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Devino.Args;

namespace Devino.Logger
{
    public class DevinoLogger : IDevinoLogger
    {
        public event EventHandler<ApiRequestEventArgs> MakingApiRequest;
        public event EventHandler<ApiResponseEventArgs> ApiResponseReceived;
        public event EventHandler<ApiRequestExceptionEventArgs> ApiRequestException;
        public void Log(string message)
        {
        }

        public void LogApiRequest(ApiRequestEventArgs args)
        {
            MakingApiRequest?.Invoke(this, args);
        }

        public void LogApiResponse(ApiResponseEventArgs args)
        {
            ApiResponseReceived?.Invoke(this, args);
        }

        public void LogApiRequestException(ApiRequestExceptionEventArgs args)
        {
            ApiRequestException?.Invoke(this, args);
        }

        public static IDevinoLogger GetLogger(TransportKind transportKind)
        {
            var writer = new DevinoLogWriter(true, transportKind);
            var log = new DevinoLogger();
            log.MakingApiRequest += writer.Request;
            log.ApiResponseReceived += writer.Response;
            log.ApiRequestException += writer.Exception;

            return log;
        }
    }


}
