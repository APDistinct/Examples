using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FLChat.DAL;
using FLChat.DAL.Model;

using Z.EntityFramework.Plus;

namespace FLChat.WebService.Handlers.Auth
{
    /// <summary>
    /// delete existed token from database
    /// </summary>
    public class Logout : CheckAuthHttpHandler.IHandlerStrategy
    {
        public bool IsReusable => true;

        public void ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, string token) {
            entities.AuthToken.Where(t => t.Token == token && t.UserId == currUserInfo.UserId).Delete();    
        }

        public void ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, HttpContext context) {
            ProcessRequest(entities, currUserInfo, context.Request.Headers["Authorization"].Substring(CheckAuthHttpHandler.Bearer.Length));
        }
    }
}
