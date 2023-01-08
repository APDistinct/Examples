using System;
using FLChat.DAL;
//using FLChat.VkWidget;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.VkWidget.Tests
{
    [TestClass]
    public class AdapterTests
    {
        private Guid messId;
        VkWidgetCallbackData callbackData;

        [TestInitialize]
        public void Init()
        {
            messId = Guid.NewGuid();
            callbackData = new VkWidgetCallbackData()
            {
                DeepLink = "12345678",
                UserId = 1234567,
                Id = messId
            };
        }

        [TestCleanup]
        public void Clean()
        {            
        }

        [TestMethod]
        public void VkWidgetAdapterTest_AllOK_withId()
        {
            VKWidgetAdapter msg = new VKWidgetAdapter(callbackData);
            Assert.AreEqual(TransportKind.VK, msg.TransportKind);
            Assert.AreEqual(callbackData.UserId.ToString(), msg.FromId);
            Assert.AreEqual(callbackData.Id.ToString(), msg.MessageId);
            Assert.IsNull(msg.FromName);
            Assert.IsNotNull(msg.Text);            
            Assert.IsNull(msg.PhoneNumber);            
            Assert.IsNull(msg.ReplyToMessageId);
        }

        [TestMethod]
        public void VkWidgetAdapterTest_AllOK_withoutId()
        {
            callbackData.Id = null;
            VKWidgetAdapter msg = new VKWidgetAdapter(callbackData);
            Assert.IsNotNull(msg.MessageId);            
        }

        //[TestMethod]
        //public void VkWidgetAdapterTest_NotOK_DeepLink()
        //{
        //    callbackData.DeepLink = null;
        //    Assert.ThrowsException<Exception>(() => new VKWidgetAdapter(callbackData));
        //}
    }
}
