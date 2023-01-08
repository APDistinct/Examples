using FLChat.DAL;
using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace FLChat.WebService.Handlers
{
    public class GetProfileAvatar : GetUserAvatarBase, IByteArrayHandlerStrategy
    {
        public int ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, NameValueCollection parameters, 
            byte[] requestData, string requestContentType, 
            out byte[] responseData, out string responseContentType, out string fileName)
        {
            return ProcessRequest(entities, currUserInfo.UserId, requestData, requestContentType, 
                out responseData, out responseContentType, out fileName, false);
        }
    }
}
