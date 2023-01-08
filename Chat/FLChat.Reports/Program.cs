using Devino.Viber;
using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Reports
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("usage: Reports.exe <MessageId>");
                return;
            }
            if (!Guid.TryParse(args[0], out Guid messId))
            {
                Console.WriteLine("Invalid essage Id: " + args[0]);
                return;
            }

            //MakeReport(messId);

            var reporter = new ReportService();
            reporter.MakeReport(messId);
        }

        private static void MakeReport(Guid messId)
        {
            using (ChatEntities entities = new ChatEntities())
            {
                var list = MessageStatsReader.GetMessageSentStats(entities, messId);
                var stats = list.Where(z => z.ToTransportTypeId == 100).ToArray();
                Dictionary<string, GetStatusResponse> dic = new Dictionary<string, GetStatusResponse>();
                DevinoStatsReader statsReader = new DevinoStatsReader();

                var statsInfo = GetStatusResponses(stats);


                foreach (var stat in stats)
                {
                    if (stat.TransportTypeId != null)
                    {
                        string num = stat.TransportId;
                        dic[num] = statsReader.GetInfo(num);
                    }
                }

                var common = CommonInfoReader.GetMessageCommonInfo(entities, messId);
                if (common != null)
                {
                    ReportStat1 rep = new ReportStat1();
                    rep.MakeReport(list, dic, common);
                    rep.WriteReport(messId.ToString() + ".csv");
                }
                //WriteReport(stats, args[0]);
            }
        }

        private static Dictionary<string, StatusResponse> GetStatusResponses(MessageSentStats[] stats)
        {
            int i = 0;
            int count = 100;

            DevinoStatsReader statsReader = new DevinoStatsReader();
            Dictionary<string, StatusResponse> dic = new Dictionary<string, StatusResponse>();
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
                    Console.Write($"{i}(-) ");
                }

                i++;
            }

            Console.WriteLine();
            return dic;
        }

        private static void WriteReport(MessageSentStats[] stats, string name)
        {
            var writePath = name + ".csv";
            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
            {
                foreach (var stat in stats)
                {
                    string str = $"{stat.FullName};{stat.Phone};{stat.IsFailed};";
                    sw.WriteLine(str);
                }
            }
        }
    }
}
