using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.DAL.Tests
{
    [TestClass]
    public class EventTests
    {
        [TestMethod]
        public void EventKind_IsMessageLifeCicleEvent() {
            HashSet<EventKind> isTrue = new HashSet<EventKind>() {
                EventKind.MessageSent,
                EventKind.MessageDelivered,
                EventKind.MessageRead,
                EventKind.MessageFailed
            };

            int trueCount = 0;
            foreach (EventKind kind in Enum.GetValues(typeof(EventKind))) {
                if (kind.IsMessageLifeCicleEvent()) {
                    trueCount += 1;
                    Assert.IsTrue(isTrue.Contains(kind));
                } else
                    Assert.IsFalse(isTrue.Contains(kind));
            }

            Assert.AreEqual(isTrue.Count, trueCount);
        }
    }
}
