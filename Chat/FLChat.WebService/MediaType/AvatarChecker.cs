using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FLChat.Core.Media;

namespace FLChat.WebService.MediaType
{
    public class AvatarChecker : IMediaTypeChecker
    {
        public bool Check(ChatEntities entities, byte[] requestData, string requestContentType, out int mediaTypeId, out int mediaTypeGroupId)
        {
            bool ret = true;
            mediaTypeId = 0;
            mediaTypeGroupId = 0;
            string fileImageType = requestData.GetFileImageType();
            if (fileImageType != requestContentType)
                ret = false;
            if (!AvatarDbChecker.Check(entities, fileImageType, out mediaTypeId, out mediaTypeGroupId))
                ret = false;

            return ret;
        }
    }
}
