using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devino.Args;
using FLChat.DAL;
using FLChat.PDAL;

namespace Devino.Logger
{
    public class DevinoLogWriter
    {
        private readonly RequestResponseLogWritter _writer;

        /// <summary>
        /// called when log operations throws exception
        /// </summary>
        public EventHandler<Exception> OnLogError;

        public DevinoLogWriter(bool outcome, TransportKind kind = TransportKind.Sms)
        {
            _writer = new RequestResponseLogWritter(outcome, kind);
        }

        public void Request(object sender, ApiRequestEventArgs request)
        {
            try
            {
                _writer.Request(request, request.Url, request.MethodName, request.HttpContent.ReadAsStringAsync().Result);
            }
            catch (Exception e)
            {
                OnLogError?.Invoke(this, e);
            }
        }

        public void Response(object sender, ApiResponseEventArgs response)
        {
            try
            {
                _writer.Response(
                    response.ApiRequestEventArgs,
                    (int)response.ResponseMessage.StatusCode,
                    response.ResponseMessage.Content.ReadAsStringAsync().Result);
            }
            catch (Exception e)
            {
                OnLogError?.Invoke(this, e);
            }
        }

        public void Exception(object sender, ApiRequestExceptionEventArgs exception)
        {
            try
            {
                _writer.Exception(
                    exception.ApiRequestEventArgs,
                    exception.Exception);
            }
            catch (Exception e)
            {
                OnLogError?.Invoke(this, e);
            }
        }
    }
}
