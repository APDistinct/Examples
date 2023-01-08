using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;
using log4net;
using System.Data.SqlClient;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Diagnostics;
using System.Data;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace FLChat.DAL.Import.Oracle.LoadSegm
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger("root");
        //private static OracleConnection _conn = null;
        private static Stopwatch wPartner = new Stopwatch();

        static void Main(string[] args) {
            if (args.Length < 1)
                Console.WriteLine("usage: LoadCons.exe <ConsultantNumber>");
            if (!int.TryParse(args[0], out int consNumber))
                Console.WriteLine("Invalid consultant number: " + args[0]);

            string connString = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
            int batchRecords = int.Parse(ConfigurationManager.AppSettings["batch_records"]);
            OracleConnection conn = null;

            try {
                log.Info("Start load segments for consultant #" + consNumber.ToString());
                log.Info($"Batch import: {batchRecords.ToString()} rows");
                Stopwatch wtotal = Stopwatch.StartNew();
                using (SegmentSource source = new SegmentSource()) {
                    Stopwatch wPartner = Stopwatch.StartNew();
                    source.Load(connString, ref conn, consNumber);
                    wPartner.Stop();
                    log.Info($"Complete loading from oracle for {wPartner.Elapsed.TotalMinutes.ToString()} minutes");
                    Dictionary<string, Guid> segments = null;
                    int totalRecords = 0;

                    using (ChatEntities entities = new ChatEntities())
                    using (var trans = entities.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted)) {
                        segments = ImportSegmentsBatch.LoadSegmentsIds(
                            entities,
                            source.SegmentNames,
                            out List<string> inserted);
                        trans.Commit();
                        if (inserted != null)
                            Loader_OnNewSegments(source, inserted);

                    }

                    ImportSegmentsBatch import = null;
                    ImportSegmentsBatch.Result total = new ImportSegmentsBatch.Result();
                    int cnt = 0;
                    foreach (var item in source) {
                        if (import == null)
                            import = new ImportSegmentsBatch(segments);
                        import.Add(item.Item1, item.Item2);
                        totalRecords += 1;
                        cnt += 1;
                        if (cnt >= batchRecords) {
                            log.Info($"execute at {totalRecords.ToString()}");
                            using (ChatEntities entities = new ChatEntities())
                            using (var trans = entities.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted)) {
                                var result = import.Execute(entities);
                                trans.Commit();
                                total.Inserted += result.Inserted;
                                total.Deleted += result.Deleted;
                            }
                            import.Dispose();
                            import = null;
                            cnt = 0;
                        }
                    }

                    if (import != null) {
                        using (ChatEntities entities = new ChatEntities())
                        using (var trans = entities.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted)) {
                            log.Info($"final execute at {totalRecords.ToString()}");
                            var result = import.Execute(entities);
                            trans.Commit();
                            total.Inserted += result.Inserted;
                            total.Deleted += result.Deleted;
                        }
                        import.Dispose();
                        import = null;
                    }

                    Stopwatch wUpdateCash = Stopwatch.StartNew();
                    bool cashUpdated = UpdateCachedValues(consNumber);
                    wUpdateCash.Stop();
                    if (cashUpdated)
                        log.Info($"update cached values for {wUpdateCash.Elapsed.TotalMinutes.ToString()}");

                    wtotal.Stop();

                    log.Info($"Total time: {wtotal.Elapsed.TotalMinutes.ToString()} minutes");
                    log.Info($"Inserted: {total.Inserted.ToString()}");
                    log.Info($"Deleted : {total.Deleted.ToString()}");
                    log.Info($"Total   : {totalRecords.ToString()}");
                }
                /*using (PartialLoaderSegments loader = new PartialLoaderSegments(connString, consNumber)) {
                    loader.OnLoading += Loader_OnLoading;
                    loader.OnLoadComplete += Loader_OnLoadComplete;
                    loader.OnLoadError += Loader_OnLoadError;
                    loader.OnNewSegments += Loader_OnNewSegments;
                    loader.Load();

                    total.Stop();

                    log.Info($"Complete loading from oracle for {wPartner.Elapsed.TotalMinutes.ToString()} minutes");
                    log.Info($"Total time: {total.Elapsed.TotalMinutes.ToString()} minutes");
                    log.Info($"Inserted: {loader.Inserted}");
                    log.Info($"Deleted: {loader.Deleted}");
                }*/
            } catch (Exception e) {
                log.Error(e.ToString());
            }


            /*Stopwatch total = Stopwatch.StartNew();
            try {
                using (ChatEntities entities = new ChatEntities())
                //entities.
                using (SegmentSource source = new SegmentSource()) {
                    source.OnFillError += Source_OnFillError;
                    Stopwatch partner = Stopwatch.StartNew();
                    source.Load(connString, ref _conn, 
                        consNumber, null, null, 
                        OnLoadException);
                    partner.Stop();
                    log.Info(String.Concat(
                        "Complete loading from oracle for ",
                        TimeSpan.FromMilliseconds(partner.ElapsedMilliseconds).TotalMinutes.ToString(),
                        " minutes"));

                    entities.SyncPartnerSegments(source.LoadFields());

                    //using (var entTrans = entities.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted)) {
                    int cnt = entities.ImportSegments(source, new Listener());
                    log.Info($"updated: {cnt.ToString()}");
                    log.Info("Complete");

                    //    entTrans.Commit();
                    //}
                }

                total.Stop();
                log.Info(String.Concat(
                    "Complete for ",
                    TimeSpan.FromMilliseconds(total.ElapsedMilliseconds).TotalMinutes.ToString(),
                    " minutes"));
            } catch (Exception e) {
                log.Error(e.ToString());
                //Console.ReadKey();
            } finally {
                _conn?.Dispose();
            }*/
        }

        private static void Loader_OnLoading(object sender, PartialLoaderSegments.LoadingArg e) {
            PartialLoaderSegments loader = sender as PartialLoaderSegments;
            log.Info($"loading from partner: {((e.Iteration * loader.RecordsLimit) + 1).ToString()}-{((e.Iteration + 1) * loader.RecordsLimit).ToString()} records");
            wPartner.Start();
        }

        private static void Loader_OnLoadComplete(object sender, PartialLoaderSegments.LoadCompleteArg e) {
            wPartner.Stop();
            log.Info($"row count: {e.RowCount.ToString()}");
        }

        private static void Loader_OnLoadError(object sender, PartialLoaderSegments.LoadExceptionArg e) {
            log.Error($"#{e.TryCount.ToString()} loading error: {e.Exception.Message}");
        }

        private static void Loader_OnNewSegments(object sender, List<string> e) {
            foreach (string s in e)
                log.Info($"new segment: {s}");
        }

        /*private static bool OnLoadException(OracleException e, int tryCount) {
            log.Error($"#{tryCount.ToString()} loading error: {e.Message}");
            return true;
        }

        private static void Source_OnFillError(object sender, System.Data.FillErrorEventArgs e) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ERROR: " + e.Errors.ToString());
            sb.AppendLine("Values: ");
            for (int i = 0; i < e.Values.Length; ++i) {
                object o = e.Values[i];
                sb.AppendLine("\t" + ((o == null || o == DBNull.Value) ? "(null)" : o.ToString()));
            }
            log.Error(sb.ToString());
        }

        private class Listener : IListener
        {
            public void OnCommit(int index) {
                log.Info($"commit on {index.ToString()}");
            }

            public void Progress(int stage, int index, bool updated) {
                if (log.IsDebugEnabled)
                    log.Debug($"{stage.ToString()}: {index.ToString()} {(updated ? "upd" : "ins")}");
            }

            public void Warning(int index, string text) {
                log.Warn($"{index.ToString()}:  {text}");
            }
        }*/

        private static bool UpdateCachedValues(int consNumber) {
            using (ChatEntities entities = new ChatEntities())
            {
                entities.Database.CommandTimeout = 900;
                using (var trans = entities.Database.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    User head = entities
                        .User
                        .Where(u => u.FLUserNumber == consNumber)
                        .Single();
                    if (head.IsUseDeepChilds)
                    {
                        entities.Update_StructureNodeCount(head.Id);
                        trans.Commit();
                        return true;
                    }
                    else
                        return false;
                }
            }
        }
    }

}
