using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.WebService.MediaType
{
    public interface IMediaTypeChecker
    {
        /// <summary>
        /// Verify media type
        /// </summary>
        /// <param name="entities">database entities</param>
        /// <param name="requestData">media data</param>
        /// <param name="requestContentType">media data type</param>
        /// <param name="mediaTypeId">if returns true, that field has media type code</param>
        /// <returns>true if media type correct</returns>
        bool Check(ChatEntities entities, byte[] requestData, string requestContentType, out int mediaTypeId, out int mediaTypeGroupId);
    }
}
