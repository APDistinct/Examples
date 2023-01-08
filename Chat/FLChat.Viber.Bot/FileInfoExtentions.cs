using FLChat.DAL.Model;
using FLChat.Viber.Client.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Viber.Bot
{
    public static class FileInfoExtentions
    {
        /// <summary>
        /// returns true if this file can be send as viber picture
        /// </summary>
        /// <param name="fi">database information about file</param>
        /// <returns>true </returns>
        public static bool IsViberPicture(this FileInfo fi) =>
            fi.MediaType.Name == "image/jpeg"
            && fi.FileLength <= SendPictureMessageRequest.MaxImageSize;
    }
}
