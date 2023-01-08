using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;
using FLChat.Core.Media;

namespace FLChat.WebService.MediaType
{
    public class FileChecker : IMediaTypeChecker
    {
        public bool Check(ChatEntities entities, byte[] requestData, string requestContentType, out int mediaTypeId, out int mediaTypeGroupId)
        {
            bool ret = true;
            mediaTypeId = 0;
            mediaTypeGroupId = 0;
            string fileType = requestData.GetFileImageType();
            
            if (!FileDbChecker.Check(entities, fileType, out mediaTypeId, out mediaTypeGroupId))
                return false;

            return ret;
        }        
    }
}

