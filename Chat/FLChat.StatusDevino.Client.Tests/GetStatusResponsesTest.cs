using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Devino.Viber;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.StatusDevino.Client.Tests
{
   

    [TestClass]
    public class GetStatusResponsesTests
    {
        public class FakeStatusReader_TokenCancel : IStatusReader
        {
            const string OK = "ok";
            public CancellationTokenSource cts;            
            private int count = 0;
            private int _stop;
            public FakeStatusReader_TokenCancel(int stop = 3)
            {
                _stop = stop;
            }

            public Task<GetStatusResponse> GetInfo(string[] num)
            {
                var dic = new List<StatusResponse>();
                var rrr =  Task.Run(() =>
                {
                    count++;
                    foreach (var nn in num)
                    {
                        dic.Add(new StatusResponse() { Status =  ViberStatus.Enqueued, ProviderId = nn});
                    }
                    if (count == _stop)
                        cts.Cancel();
                    GetStatusResponse getStatus = new GetStatusResponse() { Status = OK, Messages = dic };
                    return getStatus;
                });

                return rrr;
            }

            public Task<GetStatusResponse> GetInfo(string[] num, CancellationToken ct)
            {
                return GetInfo(num);
            }
        }

        public class FakeStatusReader_NotAllOK : IStatusReader
        {
            const string OK = "ok";
            public CancellationTokenSource cts;
            private int count = 0;
            
            public Task<GetStatusResponse> GetInfo(string[] num)
            {
                var dic = new List<StatusResponse>();
                var rrr = Task.Run(() =>
                {
                    
                    foreach (var nn in num)
                    {
                        dic.Add(new StatusResponse() { Status = ViberStatus.Enqueued, ProviderId = nn });
                    }                    
                    GetStatusResponse getStatus = new GetStatusResponse() { Status = count % 2 == 0 ?  OK : "ccc", Messages = dic };
                    count++;
                    return getStatus;
                });

                return rrr;
            }

            public Task<GetStatusResponse> GetInfo(string[] num, CancellationToken ct)
            {
                return GetInfo(num);
            }
        }

        [TestMethod]
        public async Task TestMethod_NotAllOK()
        {            
            CancellationTokenSource cts = new CancellationTokenSource();
            IStatusReader fsr = new FakeStatusReader_NotAllOK();
            //fsr.cts = cts;
            GetStatusResponses gsr = new GetStatusResponses(fsr);
            List<string> list = new List<string>();
            for(int i = 0; i < 1000; ++i)
            {
                list.Add(i.ToString());
            }
            var ret = await gsr.Get(list.ToArray(), cts.Token);
            Assert.AreEqual(5 * 100, ret.Count);
        }

        [TestMethod]
        public async Task TestMethod_TokenCancel()
        {
            int stop = 2;
            CancellationTokenSource cts = new CancellationTokenSource();
            IStatusReader fsr = new FakeStatusReader_TokenCancel(stop)
            {
                cts = cts
            };
            GetStatusResponses gsr = new GetStatusResponses(fsr);
            List<string> list = new List<string>();
            for (int i = 0; i < 1000; ++i)
            {
                list.Add(i.ToString());
            }
            var ret = await gsr.Get(list.ToArray(), cts.Token);
            Assert.AreEqual(stop * 100, ret.Count);
        }
    }
}
