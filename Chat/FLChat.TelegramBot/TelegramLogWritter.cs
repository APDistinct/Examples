using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Args;

using FLChat.DAL;
using FLChat.PDAL.Model;
using FLChat.PDAL;

namespace FLChat.TelegramBot
{
    public class TelegramLogWritter
    {
        private readonly RequestResponseLogWritter _writter;

        /// <summary>
        /// called when log operations throws exception
        /// </summary>
        public EventHandler<Exception> OnLogError;

        public TelegramLogWritter(bool outcome, TransportKind kind = TransportKind.Telegram) {
            _writter = new RequestResponseLogWritter(outcome, kind);
        }

        public void Request(object sender, ApiRequestEventArgs request) {
            try {
                _writter.Request(request, request.MethodName, null, request.HttpContent.ReadAsStringAsync().Result);
            } catch (Exception e) {
                OnLogError?.Invoke(this, e);
            }
        }

        public void Response(object sender, ApiResponseEventArgs response) {
            try {
                _writter.Response(
                    response.ApiRequestEventArgs,
                    (int)response.ResponseMessage.StatusCode,
                    response.ResponseMessage.Content.ReadAsStringAsync().Result);
            } catch (Exception e) {
                OnLogError?.Invoke(this, e);
            }
        }

        public void Exception(object sender, ApiRequestExceptionEventArgs exception) {
            try {
                _writter.Exception(
                    exception.ApiRequestEventArgs,
                    exception.Exception);
            } catch (Exception e) {
                OnLogError?.Invoke(this, e);
            }
        }
    }
}
