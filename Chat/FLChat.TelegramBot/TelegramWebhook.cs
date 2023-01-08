using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FLChat.DAL;
using FLChat.PDAL.Model;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace FLChat.TelegramBot
{
    public class Webhook : IHttpHandler
    {
        private readonly ITelegramUpdateHandler _handler;

        public Webhook() : this(new TelegramUpdateHandler()) {
        }

        public Webhook(ITelegramUpdateHandler handler) {
            _handler = handler;
        }

        public bool IsReusable => true;

        public void ProcessRequest(HttpContext context) {
            using (ProtEntities entities = new ProtEntities()) {
                TransportLog log = entities.TransportLog.Add(new TransportLog() {
                    Outcome = false,
                    Url = "Webhook",
                    Method = context.Request.HttpMethod,
                    TransportTypeId = (int)TransportKind.Telegram
                });

                try {
                    string requestString = null;
                    if (context.Request.ContentLength != 0) {
                        byte[] PostData = context.Request.BinaryRead(context.Request.ContentLength);
                        requestString = Encoding.UTF8.GetString(PostData);
                        log.Request = requestString;
                    }

                    Update upd = JsonConvert.DeserializeObject<Update>(requestString);

                    _handler.MakeUpdate(upd);

                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                } catch (Exception e) {
                    log.Exception = e.ToString();
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                } finally {
                    log.StatusCode = context.Response.StatusCode;
                    entities.SaveChanges();
                }
            }

        }
    }
}
