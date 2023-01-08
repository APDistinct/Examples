using FLChat.Core.Media;
using FLChat.VKBotClient.Types;
using FLChat.VKBotClient.Types.Attachments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.VKBot
{
    public interface IVKPhotoChecker
    {
        void PhotoCheck(AttachmentFile file);
        //void NameCheck(AttachmentFile file);
    }

    public class VKFileChecker : IVKPhotoChecker
    {
        private readonly int koef = 20;
        private readonly int MaxLength = 50*1024*1024;
        private readonly int MaxSize = 14000;

        //public void NameCheck(AttachmentFile file)
        //{
        //    if (file.Type == AttachmentType.AvatarUrl)
        //    {

        //        return;
        //    }

        //    return;
        //}

        public void PhotoCheck(AttachmentFile file)
        {
            //AttachmentType type = file.Type;
            if (file.Type != AttachmentType.Photo)
                return;
            var data = file.Bytes;
            if(data.GetImageSize(out int? width, out int? height))
            {
                if (Math.Max(width.Value, height.Value) > koef * Math.Min(width.Value, height.Value))
                {
                    file.Type = AttachmentType.Doc;
                    return;
                }
            }
            if(!( new FileFormat[] { FileFormat.Gif, FileFormat.Jpg, FileFormat.Png }).Contains(data.GetFormat()))
            {
                file.Type = AttachmentType.Doc;
                return;
            }
            if(data.Length > MaxLength)
            {
                file.Type = AttachmentType.Doc;
                return;
            }                
            if (width + height > MaxSize)
            {
                file.Type = AttachmentType.Doc;
                return;
            }            
        }
    }
}
