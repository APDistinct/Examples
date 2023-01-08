using System;
using System.Linq;
using FLChat.Core.Media;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.PDAL;

namespace FLChat.Core.Algorithms
{
    /// <summary>
    /// Gets avatarUrl from message and saves into our user profile
    /// </summary>
    public class AvatarLoader : IAvatarLoader
    {
        private readonly IAvatarProvider _avatarProvider;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public AvatarLoader(IAvatarProvider avatarProvider)
        {
            this._avatarProvider = avatarProvider;
        }

        public void TryLoadAvatar(ChatEntities entities, IOuterMessage message, Transport transport) {
            var user = transport.User;
            
            // Because we don't need to override existing avatar
            if (user?.AvatarUploadDate != null)
                return;

            try
            {
                // Convert avatar link to image

                int mediaTypeId = 0;
                int mediaTypeGroupId = 0;

                var avatarBytes = _avatarProvider.GetAvatarPicture(message.AvatarUrl, transport.TransportOuterId);

                if (avatarBytes == null || avatarBytes.Length == 0)
                    return;

                if (!IsAvatarFormatSupported(entities, avatarBytes, out mediaTypeId, out mediaTypeGroupId))
                {
                    return;
                }

                if (!avatarBytes.GetImageSize(out int? width, out int? height))
                {
                    return;
                }

                if (width != height)
                {
                    avatarBytes = avatarBytes.GetRectangle();
                }
                
                // Save avatar to contact
                user.UserAvatar = new UserAvatar();
                user.UserAvatar.MediaTypeId = mediaTypeId;
                user.UserAvatar.Data = avatarBytes;

                entities.SaveChanges();
            }
            catch (Exception e)
            {
              new RequestResponseLogWritter(false, TransportKind.FLChat).Exception(string.Empty, e);
            }
        }

        private bool IsAvatarFormatSupported(ChatEntities entities, byte[] requestData, out int mediaTypeId, out int mediaTypeGroupId)
        {
            mediaTypeId = 0;
            mediaTypeGroupId = 0;
            string fileType = requestData.GetFileImageType();

            var media = entities.MediaType
                .SingleOrDefault(t => t.Name == fileType);
            
            if (media == null)
            {
                mediaTypeId = 0;
                mediaTypeGroupId = 0;
                return false;
            }
            else
            {
                mediaTypeGroupId = media.MediaTypeGroupId;
                mediaTypeId = media.Id;
                return true;
            }
        }
    }
}