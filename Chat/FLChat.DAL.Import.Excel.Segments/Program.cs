using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace FLChat.DAL.Import.Excel.Segments
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger("root");

        static void Main(string[] args) {
            if (args.Length < 1) {
                log.Info($"USAGE: FLChat.DAL.Import.Excel.Util.exe <filename.xlsx>");
                return;
                //args = new string[] { "Мельникова Светлана Александровна.xlsx", "report" };
            }

            try {
                using (ChatEntities entities = new ChatEntities()) {
                    ExcelReadSegments excel = new ExcelReadSegments(args[0], 2);
                    excel.OnError += Excel_OnError;

                    entities.SyncPartnerSegments(excel.LoadSegmentNames());

                    int cnt = entities.ImportSegments(excel.Load());
                    log.Info($"updated: {cnt.ToString()}");
                    log.Info("Complete");
                }
            } catch (Exception e) {
                log.Error(e.ToString());
                //Console.ReadKey();
            }
        }

        private static void Excel_OnError(object sender, ExcelReadSegments.MessageArg e) {
            log.Error($"{e.Row.ToString()}: {e.Message}");
        }
    }
}
