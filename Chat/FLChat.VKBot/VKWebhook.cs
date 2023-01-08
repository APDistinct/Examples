using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FLChat.DAL;
using FLChat.PDAL.Model;
using FLChat.VKBotClient.Callback;
using FLChat.VKBotClient.Types;
using Newtonsoft.Json;


namespace FLChat.VKBot
{
    public class VKWebhook : IHttpHandler
    {
        private readonly IVKUpdateHandler _handler;
        private string responseOk = "ok";

        public VKWebhook() : this(new VKUpdateHandler())
        {
        }

        public VKWebhook(IVKUpdateHandler handler)
        {
            _handler = handler;
        }

        public bool IsReusable => true;

        public void ProcessRequest(HttpContext context)
        {
            responseOk = "ok";
            using (ProtEntities entities = new ProtEntities())
            {
                TransportLog log = entities.TransportLog.Add(new TransportLog()
                {
                    Outcome = false,
                    Url = "Webhook",
                    Method = context.Request.HttpMethod,
                    TransportTypeId = (int)TransportKind.VK
                });

                try
                {
                    string requestString = null;
                    if (context.Request.ContentLength != 0)
                    {
                        byte[] PostData = context.Request.BinaryRead(context.Request.ContentLength);
                        requestString = Encoding.UTF8.GetString(PostData);
                        log.Request = requestString;
                    }

                    var upd = JsonConvert.DeserializeObject<CallbackData>(requestString);
                    ////Update upd = JsonConvert.DeserializeObject<Update>(requestString);
                    var responseObj = _handler.MakeUpdate(upd);

                    if (responseObj != null)
                    {
                        responseOk = responseObj.ToString();
                    }

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

        //private Update MakeUpdate(CallbackCommonResponse ret)
        //{
        //    Update update = new Update();
        //    switch (ret.Type)
        //    {
        //        //case "message_allow"
        //        //case "message_deny"
        //        case "confirmation":
        //            //< appSettings >
        //            //< add key = "VK_confirmation" value = "c30c7e45" />
        //            //</ appSettings >
        //            update = null;
        //            //  Добыть ответ из ... ?
        //            responseOk = ConfigurationManager.AppSettings["VK_confirmation"] ?? "c30c7e45";
        //            break;
        //        case "message_new":
        //            var mess = JsonConvert.DeserializeObject<Message>(ret.Object.ToString());
        //            update.Message = mess;
        //            update.Id = mess.Id;
        //            break;
        //        default:
        //            update = null;
        //            break;
        //    }
        //    return update;
        //}


    }
}
