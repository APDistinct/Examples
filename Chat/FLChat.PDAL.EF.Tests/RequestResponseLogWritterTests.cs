using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FLChat.PDAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL;

namespace FLChat.PDAL.Tests
{
    [TestClass]
    public class RequestResponseLogWritterTests
    {
        Random rnd = new Random();
        object _lock = new object();

        private void Run(RequestResponseLogWritter log, object reqObj, int i) {
            log.Request(reqObj, "someurl", "test", "Run #" + i.ToString());
            lock (_lock) {
                Thread.Sleep(rnd.Next(500));
            }
            log.Response(reqObj, 200, "End #" + i.ToString());
        }

        [TestMethod]
        public void RequestResponseLogWritterTests_test() {
            RequestResponseLogWritter log = new RequestResponseLogWritter(true, TransportKind.Test);
            List<Task> list = new List<Task>();
            const int Count = 10;
            for (int i = 0; i < Count; ++i) {
                int tmp = i;
                Task t = Task.Run(() => Run(log, new object(), tmp));
                list.Add(t);
            }

            Task.WaitAll(list.ToArray());

            using (ProtEntities entities = new ProtEntities()) {
                TransportLog []records = entities.TransportLog.Take(Count).OrderByDescending(l => l.Id).ToArray()
                    .OrderBy(l => l.Request).ToArray();
                for (int i = 0; i < Count; ++i) {
                    double tm = (DateTime.UtcNow - records[i].InsertedDate).TotalMinutes;
                    Assert.IsTrue( Math.Abs(tm) < 1, "Distance between created time and now: " + tm );
                    Assert.IsTrue(records[i].Outcome);
                    Assert.AreEqual("test", records[i].Method);
                    Assert.AreEqual("Run #" + i.ToString(), records[i].Request);
                    Assert.AreEqual("End #" + i.ToString(), records[i].Response);
                    Assert.AreEqual((int)TransportKind.Test, records[i].TransportTypeId);
                }
            }
        }
    }
}
