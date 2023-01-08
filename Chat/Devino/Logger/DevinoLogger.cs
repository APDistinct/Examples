﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Devino.Args;

namespace Devino.Logger
{
    public interface IDevinoLogger
    {
        void Log(string message);

        void LogApiRequest(ApiRequestEventArgs args);
        void LogApiResponse(ApiResponseEventArgs args);
        void LogApiRequestException(ApiRequestExceptionEventArgs args);
    }

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
    }
}
