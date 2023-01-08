using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FLChat.Core.Media;
using FLChat.WebService.MediaType;

namespace FLChat.WebService.Handlers
{
    public class SetUserAvatarBase
    {
        
        protected readonly IMediaTypeChecker _avatarChecker;
        protected readonly IPictureCutter _avatarCutter;

        public int SizeLimit { get; set; } = 1024 * 1024;

        public SetUserAvatarBase(IMediaTypeChecker avatarChecker = null, IPictureCutter cutter = null)
        {            
            _avatarChecker = avatarChecker ?? new AvatarChecker();
            _avatarCutter = cutter ?? new PictureFoursquareCutter();
        }
        public bool IsReusable => true;

        
        public int ProcessRequest(ChatEntities entities, Guid id, byte[] requestData, string requestContentType, out byte[] responseData, out string responseContentType, int _sizeLimit = 1024 * 1024, bool getAll = false)
        {
            responseData = null;
            responseContentType = null;

            int mediaTypeId = 0;
            int mediaTypeGroupId = 0;

            if (!_avatarChecker.Check(entities, requestData, requestContentType, out mediaTypeId, out mediaTypeGroupId))
            {
                throw new ErrorResponseException(
                    (int)HttpStatusCode.UnsupportedMediaType,
                    new ErrorResponse(ErrorResponse.Kind.not_support, $"File {requestContentType} could not be an avatar or media data is invalid"));
            }

            //  Преобразование исходного к нужному

            _avatarCutter.Cut(ref requestData);
            _avatarChecker.Check(entities, requestData, requestContentType, out mediaTypeId, out mediaTypeGroupId);

            //  Проверка уже преобразованного
            if (requestData.Length > SizeLimit)
            {
                throw new ErrorResponseException(
                    (int)HttpStatusCode.UnsupportedMediaType,
                    new ErrorResponse(ErrorResponse.Kind.max_size_limit, $"Picture's size more then {SizeLimit} is not allowed"));
            }


            DAL.Model.User user = entities.User
                .Where(u => (u.Id == id) && (getAll || u.Enabled))
                .Include(t => t.UserAvatar)
                .SingleOrDefault();
            if (user != null)
            {
                if (user.UserAvatar == null)
                {
                    user.UserAvatar = new UserAvatar();
                }

                user.UserAvatar.Data = requestData;
                user.UserAvatar.MediaTypeId = mediaTypeId;
                entities.SaveChanges(); 
                
                return (int)HttpStatusCode.OK;
            }

            throw new ErrorResponseException(
                (int)HttpStatusCode.NotFound,
                new ErrorResponse(ErrorResponse.Kind.user_not_found, $"User with id {id} not found"));
        }
    }
}

