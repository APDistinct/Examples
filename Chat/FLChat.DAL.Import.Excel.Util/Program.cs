using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace FLChat.DAL.Import.Excel.Util
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger("root");
        private const string CI_FIXED = "fixed";
        private const string CI_REPORT = "report";

        private static int TotalCount;

        static void Main(string[] args) {
            if (args.Length < 1) {
                log.Info($"USAGE: FLChat.DAL.Import.Excel.Util.exe <filename.xlsx>");
                return;
                //args = new string[] { "Мельникова Светлана Александровна.xlsx", "report" };
            }

            try {
                using (ChatEntities entities = new ChatEntities()) {
                    IColumnIndexes ind = CreateColumnIndexex(CI_FIXED);
                    using (ExcelUserEnumerable excel = new ExcelUserEnumerable(args[0], ind, 2)) {
                        TotalCount = excel.RowCount;
                        ImportResult result = entities.FullImport(excel, new Listener());
                        log.Info("Complete");
                        log.Info($"updated: {result.UpdatedCount.ToString()}");
                        log.Info($"new    : {result.NewbeCount.ToString()}");
                        log.Info($"total  : {result.TotalRows.ToString()}");
                        //Console.ReadKey();
                    }
                }
            } catch (Exception e) {
                log.Error(e.ToString());
                //Console.ReadKey();
            }
        }

        private static IColumnIndexes CreateColumnIndexex(string arg) {
            switch (arg) {
                case CI_FIXED:
                    return new ColumnIndexes.FixedColumnIndexes();
                case CI_REPORT:
                    return new ColumnIndexes.ReportColumnIndexes();
                default:
                    throw new Exception("Invalid second argument: type of column indexes");
            }
        }

        private class Listener : IListener
        {
            public void Progress(int stage, int index, bool updated) {
                log.Info($"{stage.ToString()}: {index.ToString()}/{TotalCount.ToString()} {(updated ? "upd" : "ins")}");
            }

            public void Warning(int index, string text) {
                log.Warn($"{index.ToString()}:  {text}");
            }
        }
    }
}
