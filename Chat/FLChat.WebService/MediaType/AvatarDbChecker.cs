using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.MediaType
{
    public static class AvatarDbChecker
    {
        public static bool Check(ChatEntities entities, string requestContentType, out int mediaTypeId, out int mediaTypeGroupId)
        {
            var media = entities.MediaType.Where(t => t.Name == requestContentType).SingleOrDefault();
            if(media == null || media.CanBeAvatar == false)
            {
                mediaTypeGroupId = 0;
                mediaTypeId = 0;
                return false;
            }
            mediaTypeGroupId = media.MediaTypeGroupId;
            mediaTypeId = media.Id;
            return true;
        }
    }
}
