using FLChat.Core.Media;
using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.MediaType
{
    public class AvatarChanger
    {        
        private readonly IMediaTypeChecker _avatarChecker;
        private readonly IPictureCutter _avatarCutter;

        public AvatarChanger(IMediaTypeChecker avatarChecker = null, IPictureCutter cutter = null)
        {
            _avatarChecker = avatarChecker ?? new AvatarChecker();
            _avatarCutter = cutter ?? new PictureFoursquareCutter();
        }

        public int Change(ChatEntities entities, UserAvatar avatar)
        {
            if(avatar != null)
            {
                var data = avatar.Data;
                if (!data.GetImageSize(out int? width, out int? height))
                    return -1;
                if (width == height)
                    return 0;
                _avatarCutter.Cut(ref data);
                _avatarChecker.Check(entities, data, "", out int mediaTypeId, out int mediaTypeGroupId);
                data.GetImageSize(out width, out height);
                avatar.Data = data;
                avatar.MediaTypeId = mediaTypeId;
                avatar.Width = width.Value;
                avatar.Height = height.Value;
                return 1;
            }
            return 0;
        }
    }
}
