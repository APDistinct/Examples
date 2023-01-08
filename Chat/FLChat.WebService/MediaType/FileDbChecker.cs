using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.MediaType
{
    public static class FileDbChecker
    {
        public static bool Check(ChatEntities entities, string requestContentType, out int mediaTypeId, out int mediaTypeGroupId)
        {
            var media = entities.MediaType
                .Where(t => t.Name == requestContentType)
                //.Include(x => x.MediaTypeGroup)
                .SingleOrDefault();
            if (media == null)
            {
                mediaTypeId = 0;
                mediaTypeGroupId = 0;
                return false;
            }
            mediaTypeGroupId = media.MediaTypeGroupId;
            mediaTypeId = media.Id;
            return true;
        }
    }    
}
