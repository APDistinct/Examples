using System;
using System.Collections.Generic;
using System.Linq;
using Devino.Viber;
using FLChat.ReportDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.ReportDB.Tests
{
    public class FakeDevinoStatsReaderNotOK : IDevinoStatsReader
    {
        public GetStatusResponse GetInfo(string[] num)
        {
            //var message = new GetStatusMessage() { Messages = new List<string>(num) };
            var ret = new GetStatusResponse()
            {
                Status = "NotOK",
            };
                //Sender.ViberGetStatus(message).ConfigureAwait(false).GetAwaiter().GetResult();
            return ret;
        }
    }

    public class FakeDevinoStatsReaderOK : IDevinoStatsReader
    {
        public GetStatusResponse GetInfo(string[] num)
        {
            //var message = new GetStatusMessage() { Messages = new List<string>(num) };
            var ret = new GetStatusResponse()
            {
                Status = "ok",
                Messages = num.Select(x=> new StatusResponse()
                {
                    ProviderId = x,
                    Status = ViberStatus.Delivered,
                })                
            };
            //Sender.ViberGetStatus(message).ConfigureAwait(false).GetAwaiter().GetResult();
            return ret;
        }
    }

    [TestClass]
    public class GetStatusResponsesTests
    {
        [TestMethod]
        public void TestMethod_GetOK()
        {
            List<string> list = new List<string>();
            int count = 3;
            for(int i = 0; i < count; ++i)
            {
                list.Add(Guid.NewGuid().ToString());
            }
            GetStatusResponses gsr = new GetStatusResponses(new FakeDevinoStatsReaderOK());
            var ret = gsr.Get(list.ToArray());
            Assert.AreEqual(list.Count, ret.Count);
            CollectionAssert.AreEquivalent(list, ret.Select(x => x.ProviderId).ToList());
        }

        [TestMethod]
        public void TestMethod_GetNotOK()
        {
            List<string> list = new List<string>();
            int count = 3;
            for (int i = 0; i < count; ++i)
            {
                list.Add(Guid.NewGuid().ToString());
            }
            GetStatusResponses gsr = new GetStatusResponses(new FakeDevinoStatsReaderNotOK());
            var ret = gsr.Get(list.ToArray());
            Assert.AreEqual(0, ret.Count);
        }
    }
}
