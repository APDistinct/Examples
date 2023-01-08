using FLChat.WebService.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.WebService.MediaType;
using FLChat.DAL.Model;

namespace FLChat.WebService.Handlers.User.Tests
{
    public class AvatarCheckerFake : IMediaTypeChecker
    {
        private int _mediaTypeId;
        private int _mediaTypeGroupId;
        public AvatarCheckerFake(int mediaTypeId, int mediaTypeGroupId = 1)
        {
            _mediaTypeId = mediaTypeId;
            _mediaTypeGroupId = mediaTypeGroupId;
        }

        public bool Check(ChatEntities entities, byte[] requestData, string requestContentType, out int mediaTypeId, out int mediaTypeGroupId)
        {
            mediaTypeId = _mediaTypeId;
            mediaTypeGroupId = _mediaTypeGroupId;
            return true;
        }
    }
}
