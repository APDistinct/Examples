using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FLChat.Core.Routers;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.PDAL.Model;
//using FLChat.VKBotClient.Types;
//using FLChat.VKBotClient.Types.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using Message = FLChat.VKBotClient.Types.Message;

namespace FLChat.VkWidget
{
    public class VkWidgetHook : IHttpHandler
    {
        public bool IsReusable => true;

        private IVKWidgetUpdateHandler Handler = new VKWidgetUpdateHandler();

        public void ProcessRequest(HttpContext context)
        {        
            using (var entities = new ProtEntities())
            {
                var log = CreateLog(entities, context);

                try
                {
                    if (context.Request.ContentLength == 0)
                    {
                        throw new Exception("Context request content is empty");
                    }

                    var requestString = GetRequestString(context);
                    log.Request = requestString;

                    var callbackData = JsonConvert.DeserializeObject<VkWidgetCallbackData>(requestString);
                    //GetCallbackData(requestString);
                    Handler.MakeUpdate(callbackData);

                    var responseOk = "ok";
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.Write(responseOk);
                }
                catch (Exception e)
                {
                    log.Exception = e.ToString();
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
                finally
                {
                    log.StatusCode = context.Response.StatusCode;
                    entities.SaveChanges();
                }
            }
        }

        protected virtual string GetRequestString(HttpContext context)
        {
            var requestData = context.Request.BinaryRead(context.Request.ContentLength);
            var requestString = Encoding.UTF8.GetString(requestData);
            return requestString;
        }

        //private static CallbackData GetCallbackData(string str)
        //{
        //    var callbackData = JsonConvert.DeserializeObject<VkWidgetCallbackData>(str);
        //    var result = new CallbackData
        //    {
        //        Event = CallbackEvent.Message,
        //        Object = new JObject(new Message() { Id = callbackData.Id, Ref = callbackData.DeepLink })
        //    };
        //    return result;
        //}

        private static TransportLog CreateLog(ProtEntities entities, HttpContext context)
        {
            return entities.TransportLog.Add(new TransportLog
            {
                Outcome = false,
                Url = "WidgetWebhook",
                Method = context.Request.HttpMethod,
                TransportTypeId = (int)TransportKind.VK
            });
        }

       
    }
}
