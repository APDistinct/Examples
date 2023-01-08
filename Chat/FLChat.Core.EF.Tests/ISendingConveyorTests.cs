using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Core.Tests
{
    [TestClass]
    public class ISendingConveyorTests
    {
        private class ActionConv : ISendingConveyor
        {
            private Action _action;

            public ActionConv(Action action) {
                _action = action;
            }

            public void Send(CancellationToken ct) {
                _action.Invoke();
            }
        }

        [TestMethod]
        public void ISendingConveyorExtentions_SendEndlessly() {
            int cnt = 0;
            List<DateTime> calls = new List<DateTime>();
            const int delayms = 100;
            CancellationTokenSource cts = new CancellationTokenSource();
            ActionConv conv = new ActionConv(() => { calls.Add(DateTime.Now); if (++cnt == 3) cts.Cancel(); });

            conv.SendEndlessly(TimeSpan.FromMilliseconds(delayms), cts.Token, (s, e) => { Assert.Fail($"Exception {e} was thrown"); });

            Assert.AreEqual(3, calls.Count);
            for (int i = 0; i < calls.Count - 1; ++i) {
                double tm = (calls[i + 1] - calls[i]).TotalMilliseconds;
                Assert.IsTrue(80 < tm && tm < 150, $"Conveyor delay time {tm.ToString()} is out from interval");
            }
        }

        [TestMethod]
        public void ISendingConveyorExtentions_SendEndlessly_Exception() {
            int cnt = 0;
            int? thrownStep = null;
            List<DateTime> calls = new List<DateTime>();
            const int delayms = 100;
            CancellationTokenSource cts = new CancellationTokenSource();
            ActionConv conv = new ActionConv(() => {
                calls.Add(DateTime.Now);
                if (++cnt == 3) cts.Cancel();
                if (cnt == 1)
                    throw new Exception("test");
            });
            conv.SendEndlessly(TimeSpan.FromMilliseconds(delayms), cts.Token, (s, e) => {
                thrownStep = cnt;
            });

            Assert.AreEqual(1, thrownStep);
            Assert.AreEqual(3, calls.Count);
            //check throw exception has not influence to delay time
            for (int i = 0; i < calls.Count - 1; ++i) {
                double tm = (calls[i + 1] - calls[i]).TotalMilliseconds;
                Assert.IsTrue(80 < tm && tm < 130, $"Conveyor delay time {tm.ToString()} is out from interval");
            }
        }
    }
}
