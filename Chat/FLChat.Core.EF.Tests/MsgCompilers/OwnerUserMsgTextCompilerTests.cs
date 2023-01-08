using System;
using System.Collections.Generic;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Core.MsgCompilers.Tests
{
    [TestClass]
    public class OwnerUserMsgTextCompilerTests
    {
        private OwnerUserMsgTextCompiler hrtc = null;
        private MessageToUser mtu;
        private User user;
        private readonly string refStr = "TeStT !";
        private readonly string searchStr = "OwnerUser";
        private readonly string textMess = "some text #OwnerUser";

        [TestInitialize]
        public void Init()
        {
            hrtc = new OwnerUserMsgTextCompiler();

            user = new User()
            {
                FullName = "someNsomeN",
                City = new City() { Id = 1, Name = "CityOfTulun", },
                
            };

            mtu = new MessageToUser()
            {
                ToTransport = new Transport()
                {
                    User = user,
                },
                Message = new Message()
                {
                    Text = textMess,
                    NeedToChangeText = true,
                },
            };
        }

        [TestCleanup]
        public void Clean()
        {
        }

        [TestMethod]
        /// <summary>
        /// Замена тэга на имя OwnerUser'а
        /// </summary>        
        public void SimpleReplaceTest()
        {            
            mtu.Message.NeedToChangeText = true;
            user.OwnerUser = new User() { FullName = refStr };
            var ret = hrtc.MakeText(mtu, textMess);
            
            Assert.IsTrue(ret.Contains(refStr));
        }

        [TestMethod]
        /// <summary>
        /// Замена тэга на имя OwnerUser'а, которого нет - подстановка пустой строки
        /// </summary>        
        public void SimpleReplaceTest_NULL()
        {
            mtu.Message.NeedToChangeText = true;
            var ret = hrtc.MakeText(mtu, textMess);

            Assert.IsFalse(ret.Contains(searchStr));
        }
    }
}
