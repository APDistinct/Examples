using System;
using System.Linq;
using System.Net;
using FLChat.DAL;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.WebService.Handlers.User.Tests
{
    [TestClass]
    public class SetGetUserAvatarTest
    {
        ChatEntities entities;
        DAL.Model.User user;
        DAL.Model.MediaType media;
        int mediaTypeId;
        private string fileName1 = "Chat.png";
        private string fileName2 = "Chat.jpg";

        [TestInitialize]
        public void Init()
        {
            entities = new ChatEntities();
            //  Вопрос - а надо ли самим вставлять записи в справочник или не работать, если там нет ничего?
            media = entities.MediaType.First(); //FirstOrDefault();
            mediaTypeId = media.Id;

            user = entities.GetUser(
                u => u.Enabled && u.UserAvatar == null,
                u => { u.Enabled = true; u.UserAvatar = null; }
                );
        }

        [TestCleanup]
        public void Clean()
        {
            entities.Dispose();
        }

        private byte[] ReadFromFile(string fileName)
        {
            return System.IO.File.ReadAllBytes(".\\Image\\" + fileName);
        }

        [TestMethod]
        ///  Пользователь активен. Аватар есть. Записали проверили.
        public void SetAvatarForEnabledWithAvatarTest()
        {
            byte[] newData = ReadFromFile(fileName1);
            byte[] requestData = ReadFromFile(fileName2);
            string requestContentType = "11";
            byte[] responseData;
            string responseContentType;
            if (user.UserAvatar == null)
            {
                user.UserAvatar = new UserAvatar { UserId = user.Id, Data = newData, MediaTypeId = mediaTypeId};
                entities.SaveChanges();
            }
            SetUserAvatar setUserAvatar = new SetUserAvatar(new AvatarCheckerFake(mediaTypeId));

            int ret1 = setUserAvatar.ProcessRequest(entities, user.Id, requestData, requestContentType, out responseData, out responseContentType);
            Assert.AreEqual(ret1, (int)HttpStatusCode.OK);

            //CollectionAssert.AreEqual(requestData, user.UserAvatar.Data);
        }
        [TestMethod]
        ///  Пользователь активен. Аватара нет. Записали проверили.
        public void SetAvatarForEnabledWithoutAvatarTest()
        {
            byte[] requestData = ReadFromFile(fileName1);
            string requestContentType = "11";
            byte[] responseData;
            string responseContentType;
            if (user.UserAvatar != null)
            {
                user.UserAvatar = null;
                entities.SaveChanges();
            };

            SetUserAvatar setUserAvatar = new SetUserAvatar(new AvatarCheckerFake(mediaTypeId));
            int ret1 = setUserAvatar.ProcessRequest(entities, user.Id, requestData, requestContentType, out responseData, out responseContentType);
            Assert.AreEqual(ret1, (int)HttpStatusCode.OK);

            //CollectionAssert.AreEqual(requestData, user.UserAvatar.Data);
        }
        [TestMethod]
        ///  Пользователь активен. Аватар есть. Прочитали проверили.
        public void GetAvatarForEnabledWithAvatarTest()
        {
            byte[] newData = ReadFromFile(fileName1);
            byte[] requestData = null;// = new byte[] { 0, 1, 2, 3, 4 };
            string requestContentType = null;// = "11";
            byte[] responseData;
            string responseContentType;
            
            if(user.UserAvatar == null)
            {
                user.UserAvatar = new UserAvatar { UserId = user.Id, Data = newData, MediaTypeId = mediaTypeId };
                entities.SaveChanges();
            }
            int ret2 = GetUserAvatar.ProcessRequest(entities, user.Id, requestData, requestContentType, 
                out responseData, out responseContentType, out string fileName);
            Assert.AreEqual(ret2, (int)HttpStatusCode.OK);
            CollectionAssert.AreEqual(responseData, user.UserAvatar.Data);
        }
        [TestMethod]
        ///  Пользователь активен. Аватара нет. Прочитали проверять нечего.
        public void GetAvatarForEnabledWithoutAvatarTest()
        {
            byte[] requestData = null;// = new byte[] { 0, 1, 2, 3, 4 };
            string requestContentType = null;// = "11";
            byte[] responseData;
            string responseContentType;

            if (user.UserAvatar != null)
            {
                user.UserAvatar = null;
                entities.SaveChanges();
            };
            
            int ret2 = GetUserAvatar.ProcessRequest(entities, user.Id, requestData, requestContentType, 
                out responseData, out responseContentType, out string fileName);
            Assert.AreEqual(ret2, (int)HttpStatusCode.NoContent);
            //CollectionAssert.AreEqual(requestData, user.UserAvatar.Data);
        }
        [TestMethod]
        ///  Пользователь неактивен. Записали получили исключение.
        public void SetAvatarForNotEnabledTest()
        {
            byte[] requestData = ReadFromFile(fileName1);
            string requestContentType = "11";
            byte[] responseData;
            string responseContentType;
            user = entities.GetUser(null, null, enabled: false);

            SetUserAvatar setUserAvatar = new SetUserAvatar(new AvatarCheckerFake(mediaTypeId));
            try
            {
                int ret1 = setUserAvatar.ProcessRequest(entities, user.Id, requestData, requestContentType, out responseData, out responseContentType);
                Assert.Fail("Not throwing exception on getting avatar for non active user") ;
            }
            catch(ErrorResponseException ex)
            {
                Assert.AreEqual(ex.GetHttpCode(), (int)HttpStatusCode.NotFound);
            }
        }
        [TestMethod]
        ///  Пользователь неактивен. Прочитали получили исключение.
        public void GetAvatarForNotEnabledTest()
        {
            byte[] requestData = null; // new byte[] { 0, 1, 2, 3, 4 };
            string requestContentType = null;// "11";
            byte[] responseData;
            string responseContentType;
            user = entities.GetUser(null, null, enabled: false);
            
            try
            {
                int ret2 = GetUserAvatar.ProcessRequest(entities, user.Id, requestData, requestContentType, 
                    out responseData, out responseContentType, out string fileName);
                Assert.Fail("Not throwing exception on getting avatar for non active user");
            }
            catch (ErrorResponseException ex)
            {
                Assert.AreEqual(ex.GetHttpCode(), (int)HttpStatusCode.NotFound);
            }
        }

        [TestMethod]
        ///  Пользователь активен. Аватар есть. Удалили проверили.
        public void DelAvatarForEnabledWithAvatarTest()
        {
            byte[] newData = ReadFromFile(fileName1);
            if (user.UserAvatar == null)
            {
                user.UserAvatar = new UserAvatar { UserId = user.Id, Data = newData, MediaTypeId = mediaTypeId };
                entities.SaveChanges();
            }
            DelUserAvatar delUserAvatar = new DelUserAvatar();

            delUserAvatar.ProcessRequest(entities, user.Id);
            var avatar = entities.UserAvatar.Where(u => u.UserId == user.Id).FirstOrDefault();
            Assert.IsNull(avatar);            
        }
        [TestMethod]
        ///  Пользователь активен. Аватара нет. 
        public void DelAvatarForEnabledWithoutAvatarTest()
        {
            if (user.UserAvatar != null)
            {
                user.UserAvatar = null;
                entities.SaveChanges();
            };

            DelUserAvatar delUserAvatar = new DelUserAvatar();
            delUserAvatar.ProcessRequest(entities, user.Id);

            var avatar = entities.UserAvatar.Where(u => u.UserId == user.Id).FirstOrDefault();
            Assert.IsNull(avatar);
        }

        [TestMethod]
        ///  Пользователь неактивен или нет такого. Прочитали получили исключение.
        public void DelAvatarForNotEnabledTest()
        {
            user = entities.GetUser(null, null, enabled: false);

            try
            {
                DelUserAvatar delUserAvatar = new DelUserAvatar();
                delUserAvatar.ProcessRequest(entities, user.Id);
                Assert.Fail("Not throwing exception on getting avatar for non active user");
            }
            catch (ErrorResponseException ex)
            {
                Assert.AreEqual(ex.GetHttpCode(), (int)HttpStatusCode.NotFound);
            }
        }

    }
}
