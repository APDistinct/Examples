using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
//using Telegram.Bot.Args;

using FLChat.DAL;
using FLChat.PDAL.Model;
using FLChat.PDAL;
using FLChat.VKBotClient.Args;
using Newtonsoft.Json;

namespace FLChat.VKBot
{
    public class VKLogWritter
    {
        private readonly RequestResponseLogWritter _writter;

        /// <summary>
        /// called when log operations throws exception
        /// </summary>
        public EventHandler<Exception> OnLogError;

        public VKLogWritter(bool outcome, TransportKind kind = TransportKind.VK)
        {
            _writter = new RequestResponseLogWritter(outcome, kind);
        }

        public void Request(object sender, ApiRequestEventArgs request)
        {
            try
            {
                _writter.Request(request, request.Url, request.MethodName, request.HttpContent/*.ReadAsStringAsync().Result*/);
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
                _writter.Response(
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
                _writter.Exception(
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
