using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FLChat.Viber.Client.Types;
using FLChat.Viber.Bot.Exceptions;

namespace FLChat.Viber.Bot.Adapters.Tests
{
    [TestClass]
    public class ViberSubscribeAdapterTests
    {
        [TestMethod]
        public void ViberSubscribeAdapter_Test() {
            CallbackData data = Helper.ReadJson("callback_subscribed");
            ViberSubscribeAdapter adapter = new ViberSubscribeAdapter(data);
            Assert.IsNotNull(adapter.UserId);
            Assert.AreEqual(data.User.Id, adapter.UserId);
            Assert.AreEqual(data.User.Name, adapter.UserName);
        }

        [TestMethod]
        public void ViberSubscribeAdapter_OtherEvents() {
            foreach (CallbackData data in Helper.CreateCallbackDataForAllEvents(
                new CallbackEvent[] { CallbackEvent.Subscribed }))
                Assert.ThrowsException<ViberAdapterException>(() => new ViberSubscribeAdapter(data));
        }
    }
}
