using FLChat.Core.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace FLChat.WebService.MediaType
{
    public class PictureFoursquareCutter : IPictureCutter
    {
        public void Cut(ref byte[] data)
        {
            if (!data.GetImageSize(out int? width, out int? height))
            {
                throw new ErrorResponseException(
                   (int)HttpStatusCode.UnsupportedMediaType,
                   new ErrorResponse(ErrorResponse.Kind.not_support, $"File could not be an avatar or media data is invalid"));                
            }
            if (width == height)
                return;
            

            data = data.GetRectangle();
        }
    }
}
