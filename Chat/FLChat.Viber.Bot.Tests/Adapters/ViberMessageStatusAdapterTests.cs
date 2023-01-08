using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

using Newtonsoft.Json;

using FLChat.Viber.Client.Types;
using FLChat.Viber.Bot.Exceptions;
using System.Collections.Generic;
using FLChat.DAL;

namespace FLChat.Viber.Bot.Adapters.Tests
{
    [TestClass]
    public class ViberMessageStatusAdapterTests
    {
        [TestMethod]
        public void ViberMessageStatusAdapter_Failed() {
            ViberMessageStatusAdapter msg = new ViberMessageStatusAdapter(Helper.ReadJson("callback_failed"));
            Assert.AreEqual("4912661846655238145", msg.MessageId);
            Assert.AreEqual("01234567890A=", msg.UserId);
            Assert.IsTrue(msg.IsFailed);
            Assert.IsFalse(msg.IsDelivered);
            Assert.IsFalse(msg.IsRead);
            Assert.AreEqual("failure description", msg.FailureReason);
        }

        [TestMethod]
        public void ViberMessageStatusAdapter_Delivered() {
            ViberMessageStatusAdapter msg = new ViberMessageStatusAdapter(Helper.ReadJson("callback_delivered"));
            Assert.AreEqual("4912661846655238145", msg.MessageId);
            Assert.AreEqual("01234567890A=", msg.UserId);
            Assert.IsFalse(msg.IsFailed);
            Assert.IsTrue(msg.IsDelivered);
            Assert.IsFalse(msg.IsRead);
            Assert.IsNull(msg.FailureReason);
        }

        [TestMethod]
        public void ViberMessageStatusAdapter_Read() {
            ViberMessageStatusAdapter msg = new ViberMessageStatusAdapter(Helper.ReadJson("callback_seen"));
            Assert.AreEqual("4912661846655238145", msg.MessageId);
            Assert.AreEqual("01234567890A=", msg.UserId);
            Assert.IsFalse(msg.IsFailed);
            Assert.IsFalse(msg.IsDelivered);
            Assert.IsTrue(msg.IsRead);
            Assert.IsNull(msg.FailureReason);
        }

        [TestMethod]
        public void ViberMessageStatusAdapter_Other() {
            //IEnumerable<CallbackEvent> events = Enum
            //    .GetValues(typeof(CallbackEvent))
            //    .Cast<CallbackEvent>()
            //    .Except(new CallbackEvent[] { CallbackEvent.Delivered, CallbackEvent.Failed, CallbackEvent.Seen });
            //foreach (CallbackEvent e in events)
            //    Assert.ThrowsException<ViberAdapterException>(() => new ViberMessageStatusAdapter(new CallbackData() { Event = e }));
            foreach (CallbackData data in Helper.CreateCallbackDataForAllEvents(
                new CallbackEvent[] { CallbackEvent.Delivered, CallbackEvent.Failed, CallbackEvent.Seen }))
                Assert.ThrowsException<ViberAdapterException>(() => new ViberMessageStatusAdapter(data));
        }
    }
}
