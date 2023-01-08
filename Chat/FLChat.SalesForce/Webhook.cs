//using FLChat.PDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FLChat.SalesForce
{
    //public class Webhook : IHttpHandler
    //{
    //    public bool IsReusable => true;

    //    public void ProcessRequest(HttpContext context) {
    //        using (ProtEntities entities = new ProtEntities()) {
    //            TransportLog log = entities.TransportLog.Add(new TransportLog() {
    //                Outcome = false,
    //                Url = "Webhook",
    //                Method = context.Request.HttpMethod,
    //                TransportTypeId = (int)200
    //            });

    //            try {
    //                string requestString = null;
    //                if (context.Request.ContentLength != 0) {
    //                    byte[] PostData = context.Request.BinaryRead(context.Request.ContentLength);
    //                    requestString = Encoding.UTF8.GetString(PostData);
    //                    log.Request = requestString;
    //                }

    //                context.Response.StatusCode = (int)HttpStatusCode.OK;

    //                //if (responseObj != null) {
    //                //    string response = JsonConvert.SerializeObject(responseObj);
    //                //    context.Response.Write(response);
    //                //    context.Response.ContentType = "application/json";
    //                //    log.Response = response;
    //                //}

    //            } catch (Exception e) {
    //                log.Exception = e.ToString();
    //                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
    //            } finally {
    //                log.StatusCode = context.Response.StatusCode;
    //                entities.SaveChanges();
    //            }
    //        }


    //    }
    //}
}
