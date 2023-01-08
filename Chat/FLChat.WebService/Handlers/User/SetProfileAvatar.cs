using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.WebService.MediaType;
using System.Collections.Specialized;

namespace FLChat.WebService.Handlers
{
    public class SetProfileAvatar : SetUserAvatarBase, IByteArrayHandlerStrategy
    {
        public SetProfileAvatar(IMediaTypeChecker avatarChecker = null) : base(avatarChecker)
        {
        }

        public int ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, NameValueCollection parameters, 
            byte[] requestData, string requestContentType, 
            out byte[] responseData, out string responseContentType, out string fileName)
        {
            fileName = null;
            return ProcessRequest(entities, currUserInfo.UserId, requestData, requestContentType, 
                out responseData, out responseContentType, SizeLimit, false);
        }
    }
}
