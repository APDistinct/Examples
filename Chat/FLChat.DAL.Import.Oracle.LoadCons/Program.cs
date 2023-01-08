using FLChat.DAL.Model;
using log4net;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace FLChat.DAL.Import.Oracle.LoadCons
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger("root");

        private const int RecordsLimit = 1000;
        private static OracleConnection _conn = null;

        static void Main(string[] args) {
            if (args.Length < 1) {
                Console.WriteLine("usage: LoadCons.exe <ConsultantNumber>");
                return;
            }
            if (!int.TryParse(args[0], out int consNumber)) {
                Console.WriteLine("Invalid consultant number: " + args[0]);
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["Partner"].ConnectionString;
            int batchRecords = int.Parse(ConfigurationManager.AppSettings["batch_records"]);

            log.Info("Start load structure for consultant #" + consNumber.ToString());
            Stopwatch total = Stopwatch.StartNew();
            try {                
                using (UserSource source = new UserSource()) {
                    source.OnFillError += Source_OnFillError;
                    log.Info($"loading from oracle all records");
                    Stopwatch partner = Stopwatch.StartNew();
                    source.Load(connString, ref _conn, consNumber, onException: OnLoadException);
                    source.RemoveUnnessesaryColumns();
                    source.DataTable.AdjustPhoneNumbers();
                    partner.Stop();

                    int totalRows = source.DataTable.Rows.Count;
                    log.Info($"complete loading: {totalRows.ToString()} rows");

                    Stopwatch wImport = new Stopwatch();
                    Stopwatch wCopy = new Stopwatch();
                    ImportResult totalResult = new ImportResult();
                    List<Tuple<int, string>> clearedPhone = new List<Tuple<int, string>>();
                    List<Tuple<int, string>> clearedEmail = new List<Tuple<int, string>>();
                    List<int> missedOwner = new List<int>();

                    for (int rn = 0; rn < totalRows; rn += batchRecords) {
                        using (DataTable workingTable = source.DataTable.Clone()) {
                            wCopy.Start();
                            source
                                .DataTable
                                .AsEnumerable()
                                .Skip(rn)
                                .Take(batchRecords)
                                .CopyToDataTable(workingTable, LoadOption.Upsert);
                            wCopy.Stop();
                            log.Info($"{rn.ToString()} - {(rn + batchRecords - 1).ToString()}: {workingTable.Rows.Count.ToString()} rows");

                            wImport.Start();
                            using (ChatEntities entities = new ChatEntities())
                            using (var trans = entities.Database.BeginTransaction(IsolationLevel.ReadCommitted)) {
                                //ImportResult result = entities.FullImport(source, new Listener());
                                //totalResult.Add(result);
                                var res = entities.Import(workingTable, clearedPhone, clearedEmail, missedOwner);
                                trans.Commit();
                                totalResult.Add(res);
                            }
                            wImport.Stop();
                        }
                    }


                    //bool exhausted = false;
                    //int iteration = 0;

                    //while (exhausted == false) {
                    //    log.Info($"loading from partner: {((iteration * RecordsLimit) + 1).ToString()}-{((iteration + 1) * RecordsLimit).ToString()} records");
                    //    partner.Start();
                    //    source.Load(
                    //        connString, ref _conn, 
                    //        consNumber, (iteration * RecordsLimit) + 1, (iteration + 1) * RecordsLimit,
                    //        OnLoadException);
                    //    source.RemoveUnnessesaryColumns();
                    //    source.DataTable.AdjustPhoneNumbers();
                    //    partner.Stop();

                    //    totalRows += source.DataTable.Rows.Count;

                    //    //for (int i = 0; i < source.DataTable.Columns.Count; ++i) {
                    //    //    log.Info($"{i.ToString()}: {source.DataTable.Columns[i].ColumnName}");
                    //    //}

                    //    log.Info($"row count: {source.RowCount.ToString()}");

                    //    if (source.RowCount > 0) {
                    //        wImport.Start();
                    //        using (ChatEntities entities = new ChatEntities())
                    //        using (var trans = entities.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted)) {
                    //            //ImportResult result = entities.FullImport(source, new Listener());
                    //            //totalResult.Add(result);
                    //            var res = entities.Import(source.DataTable, clearedPhone, clearedEmail, missedOwner);
                    //            trans.Commit();
                    //            totalResult.Updated += res.Updated;
                    //            totalResult.Inserted += res.Inserted;
                    //            totalResult.ClearedPhone += res.ClearedPhone;
                    //            totalResult.ClearedEmail += res.ClearedEmail;
                    //            totalResult.OwnerUpdated += res.OwnerUpdated;
                    //            totalResult.MissedOwner += res.MissedOwner;
                    //        }
                    //        wImport.Stop();
                    //    }
                    //    exhausted = source.RowCount < RecordsLimit;
                    //    iteration += 1;
                    //}

                    int absence = DisableAbsentedUsers(consNumber, 12);

                    Stopwatch wUpdateCash = Stopwatch.StartNew();
                    bool cashUpdated = UpdateDeepChildsCache(consNumber);
                    wUpdateCash.Stop();
                    if (cashUpdated)
                        log.Info($"update structure cache for {wUpdateCash.Elapsed.TotalMinutes.ToString()}");

                    log.Info("Complete");

                    //int ownerUpdated = 0;
                    //using (ChatEntities entities = new ChatEntities()) {
                    //    ownerUpdated = entities.UpdateOwners();
                    //}

                    if (missedOwner.Contains(consNumber)) {
                        totalResult.MissedOwner -= 1;
                        missedOwner.Remove(consNumber);
                    }

                    log.Info($"updated    : {totalResult.Updated.ToString()}");
                    //log.Info($"upd own: {totalResult.OwnerUpdated.ToString()}");
                    log.Info($"inserted   : {totalResult.Inserted.ToString()}");
                    log.Info($"clear phone: {totalResult.ClearedPhone.ToString()}");
                    log.Info($"clear email: {totalResult.ClearedEmail.ToString()}");
                    log.Info($"upd owner  : {totalResult.OwnerUpdated.ToString()}");
                    log.Info($"missed ownr: {totalResult.MissedOwner.ToString()}");
                    log.Info($"absented   : {absence.ToString()}");
                    log.Info($"total      : {totalRows.ToString()}");
                    //int deleted = entities.DisableUsers(342052, result.Users);
                    //log.Info($"deleted: {deleted.ToString()}");

                    if (clearedPhone.Count > 0) {
                        log.Info("phone was cleared:");
                        foreach (var item in clearedPhone)
                            log.Info($"   {item.Item1.ToString()} - {item.Item2}");
                    }

                    if (clearedEmail.Count > 0) {
                        log.Info("email was cleared:");
                        foreach (var item in clearedEmail)
                            log.Info($"   {item.Item1.ToString()} - {item.Item2}");
                    }

                    if (missedOwner.Count > 0) {
                        log.Info("missed owners (consultant numbers):");
                        foreach (var item in missedOwner)
                            log.Info($"   {item.ToString()}");
                    }


                    //User head = entities.User.Where(u => u.FLUserNumber == consNumber).SingleOrDefault();
                    //if (head == null || !totalResult.Users.Contains(head.Id))
                    //    log.Warn("Result set does not include root consultant");

                    //   entTrans.Commit();
                    //}
                    log.Info($"Loading from oracle for {partner.Elapsed.TotalMinutes.ToString()} minutes");
                    log.Info($"Coping data for {wCopy.Elapsed.TotalMinutes.ToString()} minutes");
                    log.Info($"Importing for {wImport.Elapsed.TotalMinutes.ToString()} minutes");
                }

                total.Stop();
                log.Info($"Complete for {total.Elapsed.TotalMinutes.ToString()} minutes");
            } catch (Exception e) {
                log.Error(e.ToString());
                //Console.ReadKey();
            } finally {
                _conn?.Dispose();
            }
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

        private static bool OnLoadException(OracleException e, int tryCount) {
            log.Error($"#{tryCount.ToString()} loading error: {e.Message}");
            return true;
        }

        private class Listener : IListener
        {
            public void OnCommit(int index) {
                log.Info($"Commit on {index.ToString()}");
            }

            public void Progress(int stage, int index, bool updated) {
                if (log.IsDebugEnabled)
                    log.Debug($"{stage.ToString()}: {index.ToString()} {(updated ? "upd" : "ins")}");
            }

            public void Warning(int index, string text) {
                log.Warn($"{index.ToString()}:  {text}");
            }
        }

        private static bool UpdateDeepChildsCache(int consNumber)
        {
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
                        entities.User_UpdateDeepChilds(head.Id);
                        trans.Commit();
                        return true;
                    }
                    else
                        return false;
                }
            }
        }

        private static int DisableAbsentedUsers(int number, int hours) {
            using (ChatEntities entities = new ChatEntities())
            {
                entities.Database.CommandTimeout = 900;
                using (var trans = entities.Database.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    int? result = entities.User_DisableNotImportedUsers(null, number, hours).First();
                    trans.Commit();
                    return result ?? 0;
                }
            }
        }
    }
}
