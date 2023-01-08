using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Newtonsoft.Json;

using FLChat.DAL;
using FLChat.PDAL.Model;
using FLChat.Viber.Client.Types;
using FLChat.Viber.Client.Utils;
using System.Configuration;

namespace FLChat.Viber.Bot
{
    public class Webhook : IHttpHandler
    {
        private readonly IViberUpdateHandler _handler;

        public Webhook() : this(new ViberUpdateHandler()) {
        }

        public Webhook(IViberUpdateHandler handler) {
            _handler = handler;
        }

        public bool IsReusable => true;

        public void ProcessRequest(HttpContext context) {
            using (ProtEntities entities = new ProtEntities()) {
                TransportLog log = entities.TransportLog.Add(new TransportLog() {
                    Outcome = false,
                    Url = "Webhook",
                    Method = context.Request.HttpMethod,
                    TransportTypeId = (int)TransportKind.Viber
                });

                try {
                    string requestString = null;
                    if (context.Request.ContentLength != 0) {
                        byte[] PostData = context.Request.BinaryRead(context.Request.ContentLength);
                        requestString = Encoding.UTF8.GetString(PostData);
                        log.Request = requestString;
                    }

                    CallbackData upd = JsonConvert.DeserializeObject<CallbackData>(requestString);

                    object responseObj = _handler.MakeUpdate(upd);

                    string token = ConfigurationManager.AppSettings["viber_token"] ?? throw new ConfigurationErrorsException("Configuration value for viber token must be present");
                    context.Response.Headers.Add("X-Viber-Content-Signature", requestString.HMACSHA256String(token));
                    context.Response.StatusCode = (int)HttpStatusCode.OK;

                    if (responseObj != null) {
                        string response = JsonConvert.SerializeObject(responseObj);
                        context.Response.Write(response);
                        context.Response.ContentType = "application/json";
                        log.Response = response;
                    }

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
