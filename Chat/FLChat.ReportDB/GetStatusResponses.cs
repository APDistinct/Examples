using Devino.Viber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.ReportDB
{
    public interface IGetStatusResponses
    {
        List<StatusResponse> Get(string[] stats);
    }

    public class GetStatusResponses : IGetStatusResponses
    {
        const string OK = "ok";
        const int count = 100;
        private IDevinoStatsReader _statsReader;

        public GetStatusResponses(IDevinoStatsReader statsReader = null)
        {
            _statsReader = statsReader ?? new DevinoStatsReader();
        }

        public List<StatusResponse> Get(string[] stats)
        {
            int i = 0;        

            //var statsReader = new DevinoStatsReader();
            var dic = new List<StatusResponse>();
            //var notSend = new List<string>();

            //Console.WriteLine($"There are {stats.Length} messages");
            //Console.WriteLine($"Start! {count} messages per request");
            //Console.WriteLine();
            while (i * count < stats.Length)  // !ct.IsCancellationRequested
            {
                var sendStats = stats.Skip(i * count).Take(count).ToArray();
                try
                {
                    var response = _statsReader.GetInfo(sendStats);
                    if(response.Status == OK)
                    foreach (var message in response.Messages)
                    {
                        dic.Add(message);
                    }
                    //Console.Write($"{i}(+) ");
                }
                catch (Exception e)
                {
                    //notSend.AddRange(sendStats);
                    //Console.Write($"{i}(-) ");
                }
                i++;
            }

            //Console.WriteLine();

            return dic;
        }
    }    
}
