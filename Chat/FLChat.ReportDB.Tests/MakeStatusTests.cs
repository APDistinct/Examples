using System;
using System.Collections.Generic;
using System.Linq;
using Devino.Viber;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.ReportDB.Tests
{
    [TestClass]
    public class MakeStatusTests
    {
        const string OK = "ok";

        [TestMethod]
        public void TestMethod1()
        {
            List<StatusResponse> statsInfo = new List<StatusResponse>();
            statsInfo.Add(new StatusResponse()
            {
                 ProviderId = Guid.NewGuid().ToString(),
                 Code = OK,
                Status = ViberStatus.Failed,
                StatusAt = DateTime.UtcNow,
                SmsStates = new List<SmsState>() { new SmsState() { Id = Guid.NewGuid().ToString(), Status = SmsStatus.Delivered} }
            });

            statsInfo.Add(new StatusResponse()
            {
                ProviderId = Guid.NewGuid().ToString(),
                Code = OK,
                Status = ViberStatus.Read,
                StatusAt = DateTime.UtcNow,
                //SmsStates { get; set; }
            });

            statsInfo.Add(new StatusResponse()
            {
                ProviderId = Guid.NewGuid().ToString(),
                Code = "error-instant-message-provider-id-unknown",
                Status = ViberStatus.Unknown,
                StatusAt = DateTime.UtcNow,
                //SmsStates { get; set; }
            });

            MakeStatus makeStatus = new MakeStatus();
            var ret = makeStatus.Make(statsInfo);
            Assert.AreEqual(ret.Count(), statsInfo.Count);            
            Assert.AreEqual(ret.Where(x => x.IsFinished).ToArray().Count(), statsInfo.Count - 1);
            Assert.IsTrue(ret[2].IsFinished);
        }

        [TestMethod]
        public void TestMethod2()
        {
            List<StatusResponse> statsInfo = new List<StatusResponse>();
            statsInfo.Add(new StatusResponse()
            {
                ProviderId = Guid.NewGuid().ToString(),
                Code = OK,
                Status = ViberStatus.Failed,
                StatusAt = DateTime.UtcNow,
                SmsStates = new List<SmsState>() { new SmsState() { Id = Guid.NewGuid().ToString(), Status = SmsStatus.Undelivered} }
            });

            statsInfo.Add(new StatusResponse()
            {
                ProviderId = Guid.NewGuid().ToString(),
                Code = OK,
                Status = ViberStatus.Read,
                StatusAt = DateTime.UtcNow,
                //SmsStates { get; set; }
            });

            statsInfo.Add(new StatusResponse()
            {
                ProviderId = Guid.NewGuid().ToString(),
                Code = "error-instant-message-provider-id-unknown",
                Status = ViberStatus.Unknown,
                StatusAt = DateTime.UtcNow,
                //SmsStates { get; set; }
            });

            MakeStatus makeStatus = new MakeStatus();
            var ret = makeStatus.Make(statsInfo);
            Assert.AreEqual(ret.Count(), statsInfo.Count);
            Assert.IsTrue(ret[0].IsFinished);
            Assert.IsTrue(ret[2].IsFinished);
            Assert.IsFalse(ret[2].Update);
        }
    }
}
