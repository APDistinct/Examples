using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FLChat.Viber.Client.Types;
using FLChat.Viber.Bot.Exceptions;

namespace FLChat.Viber.Bot.Adapters.Tests
{
    [TestClass]
    public class ViberUnsubscribeAdapterTests
    {
        [TestMethod]
        public void ViberUnsubscribeAdapter_Test() {
            CallbackData data = Helper.ReadJson("callback_unsubscribed");
            ViberUnsubscribeAdapter adapter = new ViberUnsubscribeAdapter(data);
            Assert.IsNotNull(adapter.UserId);
            Assert.AreEqual(data.UserId, adapter.UserId);
        }

        [TestMethod]
        public void ViberUnsubscribeAdapter_OtherEvents() {
            foreach (CallbackData data in Helper.CreateCallbackDataForAllEvents(
                new CallbackEvent[] { CallbackEvent.Unsubscribed }))
                Assert.ThrowsException<ViberAdapterException>(() => new ViberUnsubscribeAdapter(data));
        }
    }
}
