using System;
using System.Collections.Generic;
using System.Linq;
using Devino.Viber;
using FLChat.DAL.Model;

namespace FLChat.Reports
{
    public class ReportService
    {
        public void MakeReport(Guid messageId)
        {
            using (ChatEntities entities = new ChatEntities())
            {
                var list = MessageStatsReader.GetMessageSentStats(entities, messageId);
                var stats = list.Where(z => z.ToTransportTypeId == 100).ToArray();

                var statsInfo = GetStatusResponses(stats);

                var common = CommonInfoReader.GetMessageCommonInfo(entities, messageId);
                if (common != null)
                {
                    Console.WriteLine($"Writing to .csv ");

                    ReportStat1 rep = new ReportStat1();
                    rep.MakeReport(list, statsInfo, common);
                    rep.WriteReport(messageId.ToString() + ".csv");

                    Console.WriteLine($"Done");
                }
            }
        }

        private static Dictionary<string, StatusResponse> GetStatusResponses(MessageSentStats[] stats)
        {
            int i = 0;
            int count = 100;

            var statsReader = new DevinoStatsReader();
            var dic = new Dictionary<string, StatusResponse>();
            var notSend = new List<string>();

            Console.WriteLine($"There are {stats.Length} messages");
            Console.WriteLine($"Start! {count} messages per request");
            Console.WriteLine();
            while ((i + 1) * count < stats.Length)
            {
                var sendStats = stats.Skip(i * count).Take(count).Select(a => a.TransportId).ToArray();
                try
                {
                    var response = statsReader.GetInfo(sendStats);
                    foreach (var message in response.Messages)
                    {
                        dic.Add(message.ProviderId, message);
                    }

                    Console.Write($"{i}(+) ");
                }
                catch (Exception e)
                {
                    notSend.AddRange(sendStats);
                    Console.Write($"{i}(-) ");
                }

                i++;
            }

            Console.WriteLine();

            return dic;
        }
    }
}