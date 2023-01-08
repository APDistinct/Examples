using System;
using FLChat.DAL.Model;
using FLChat.WebService.MediaType;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.WebService.MediaType.Tests
{
    [TestClass]
    public class AvatarChangerTests
    {
        private class FakeMediaTypeChecker : IMediaTypeChecker
        {
            private int _mediaTypeId, _mediaTypeGroupId;

            public FakeMediaTypeChecker(int mediaTypeId, int mediaTypeGroupId)
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
        //FakeAvatarChecker
        [TestMethod]
        public void AvatarChangerTestNotChangeSquare()
        {
            int mediaTypeId = 1;
            string fileName = "jpg200_200.jpg";
            byte[] data = System.IO.File.ReadAllBytes(".\\Image\\" + fileName);
            byte[] ddata = new byte[data.Length];
            data.CopyTo(ddata, 0);

            UserAvatar avatar = new UserAvatar()
            {
                Data = data,
                MediaTypeId = 2,
            };
            AvatarChanger changer = new AvatarChanger(new FakeMediaTypeChecker(mediaTypeId, 1));
            int oldmediaTypeId = avatar.MediaTypeId;
            changer.Change(null,avatar);
            CollectionAssert.AreEqual(avatar.Data,ddata);
            Assert.AreEqual(avatar.MediaTypeId, oldmediaTypeId);
        }

        [TestMethod]
        public void AvatarChangerTestChangeNotSquare()
        {
            int mediaTypeId = 1;
            string fileName = "jpg200_100.jpg";
            byte[] data = System.IO.File.ReadAllBytes(".\\Image\\" + fileName);
            byte[] ddata = new byte[data.Length];
            data.CopyTo(ddata, 0);

            UserAvatar avatar = new UserAvatar()
            {
                Data = data,
                MediaTypeId = 2,
            };
            AvatarChanger changer = new AvatarChanger(new FakeMediaTypeChecker(mediaTypeId, 1));
            int oldmediaTypeId = avatar.MediaTypeId;
            changer.Change(null, avatar);
            CollectionAssert.AreNotEqual(avatar.Data, ddata);
            Assert.AreEqual(avatar.MediaTypeId, mediaTypeId);
            Assert.AreEqual(avatar.Width, avatar.Height);
        }
    }
}
