using System;
using FLChat.Core.MsgCompilers;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class MessageIncomeInfoTests
    {
        private MessageToUser mtu;
        private User user;        

        [TestInitialize]
        public void Init()
        {            
            //mtu.ToTransport.User.FullName;
            //HashList[hash2] = mtu.ToTransport.User.City.Name;
            user = new User()
            {
                FullName = "someNsomeN",
                City = new City() { Id = 1, Name = "CityOfTulun", }
            };

            mtu = new MessageToUser()
            {
                ToTransport = new FLChat.DAL.Model.Transport()
                {
                    User = user,
                },
                Message = new Message()
                {
                    Text = "some text",
                    NeedToChangeText = true,
                },
            };
        }

        [TestCleanup]
        public void Clean()
        {
        }

        [TestMethod]
        public void MessageIncomeInfo_ReplaceText()
        {
            mtu.Message = new Message()
            {
                Text = "some text",
                NeedToChangeText = true,
            };
            var guid = Guid.NewGuid();
            MessageIncomeInfo mii = new MessageIncomeInfo(mtu, guid, null);
            Assert.AreEqual(mtu.Message.Text, mii.Text);

            mii = new MessageIncomeInfo(mtu, guid, "another");
            Assert.AreNotEqual(mtu.Message.Text, mii.Text);
            Assert.AreEqual("another", mii.Text);
        }
    }
}
