using Devino.Viber;
using FLChat.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FLChat.StatusDevino.Client
{
    public interface IGetStatusResponses
    {
        Task<List<StatusResponse>> Get(string[] stats, CancellationToken ct);
    }

    public class GetStatusResponses : IGetStatusResponses
    {
        const string OK = "ok";
        const int count = 100;
        private IStatusReader _statsReader;

        public GetStatusResponses(IStatusReader statsReader = null)
        {
            _statsReader = statsReader ?? new StatusReader();
        }

        public async Task<List<StatusResponse>> Get(string[] stats, CancellationToken ct)
        {
            int i = 0;
            
            var dic = new List<StatusResponse>();
            
            while (i * count < stats.Length && !ct.IsCancellationRequested)
            {
                var sendStats = stats.Skip(i * count).Take(count).ToArray();
                try
                {
                    var response = await _statsReader.GetInfo(sendStats, ct);
                    if (response.Status == OK)
                        foreach (var message in response.Messages)
                        {
                            dic.Add(message);
                        }
                }
                catch (Exception e)
                {
                    //  Протокол
                }
                i++;
            }

            return dic;
        }
    }
}
