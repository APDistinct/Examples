using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using FLChat.SalesForce.Import;
using log4net;
using SalesForce;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace FLChat.SalesForce.FullImportUtil
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger("root");

        static void Main(string[] args) {
            string authUrl = ConfigurationManager.AppSettings["auth_url"] ?? throw new ArgumentNullException("auth_url");
            string clientId = ConfigurationManager.AppSettings["client_id"] ?? throw new ArgumentNullException("client_id");
            string secret = ConfigurationManager.AppSettings["secret"] ?? throw new ArgumentNullException("secret");
            string username = ConfigurationManager.AppSettings["username"] ?? throw new ArgumentNullException("username");
            string password = ConfigurationManager.AppSettings["password"] ?? throw new ArgumentNullException("password");

            try {

                Stopwatch wTotal = Stopwatch.StartNew();
                Stopwatch wQuery = new Stopwatch();
                Stopwatch wImport = new Stopwatch();
                Stopwatch wImportSegments = new Stopwatch();

                log.Info("authentication...");
                Stopwatch wAuth = Stopwatch.StartNew();
                Task<SalesForceClient> taskClient = SalesForceClient.CreateAndAuth(clientId, secret, username, password,
                    CancellationToken.None, authUrl);
                taskClient.Wait();
                wAuth.Stop();

                using (FullImport import = new FullImport(taskClient.Result)) {

                    import.OnRequest += (s, e) => {
                        log.Info("Requesting to sales force...");
                        wQuery.Start();
                    };
                    import.OnRequestComplete += (s, e) => {
                        wQuery.Stop();
                        log.Info($"Request complete: {e.TotalCount} rows");
                    };
                    import.OnImport += (s, e) => {
                        log.Info("Importing data...");
                        wImport.Start();
                    };
                    import.OnImportComplete += (s, e) => {
                        wImport.Stop();
                        log.Info("Import complete");
                    };
                    import.OnImportSegments += (s, e) => {
                        log.Info("Importing segments...");
                        wImportSegments.Start();
                    };
                    import.OnImportSegmentsComplete += (s, e) => {
                        wImportSegments.Stop();
                        log.Info("Import segments complete");
                    };

                    Task<FullImport.Result> taskResult = import.Import(CancellationToken.None);
                    taskResult.Wait();

                    wTotal.Stop();

                    ShowResults(taskResult.Result);

                    log.Info($"Auth               for {wAuth.Elapsed.TotalMinutes.ToString()} minutes");
                    log.Info($"Loading from SF    for {wQuery.Elapsed.TotalMinutes.ToString()} minutes");
                    log.Info($"Importing users    for {wImport.Elapsed.TotalMinutes.ToString()} minutes");
                    log.Info($"Importing segments for {wImportSegments.Elapsed.TotalMinutes.ToString()} minutes");
                    log.Info($"Complete           for {wTotal.Elapsed.TotalMinutes.ToString()} minutes");
                }
            } catch (Exception e) {
                log.Fatal(e.ToString());
            }
        }

        private static void ShowResults(FullImport.Result result) {
            log.Info("===== Users ====");
            log.Info($"updated    : {result.Totals.Updated.ToString()}");
            //log.Info($"upd own: {totalResult.OwnerUpdated.ToString()}");
            log.Info($"inserted   : {result.Totals.Inserted.ToString()}");
            log.Info($"clear phone: {result.Totals.ClearedPhone.ToString()}");
            log.Info($"clear email: {result.Totals.ClearedEmail.ToString()}");
            log.Info($"upd owner  : {result.Totals.OwnerUpdated.ToString()}");
            log.Info($"missed ownr: {result.Totals.MissedOwner.ToString()}");
            log.Info("===== Segments ====");
            log.Info($"inserted   : {result.Segments.Inserted.ToString()}");
            log.Info($"deleted    : {result.Segments.Deleted.ToString()}");
            log.Info("===== Total ====");
            //log.Info($"absented   : {absence.ToString()}");
            log.Info($"total      : {result.Totals.TotalRows.ToString()}");

            if (result.ClearedPhones.Count > 0) {
                log.Info("phone was cleared:");
                foreach (var item in result.ClearedPhones)
                    log.Info($"   {item.Item1.ToString()} - {item.Item2}");
            }

            if (result.ClearedEmails.Count > 0) {
                log.Info("email was cleared:");
                foreach (var item in result.ClearedEmails)
                    log.Info($"   {item.Item1.ToString()} - {item.Item2}");
            }

            if (result.MissedOwner.Count > 0) {
                log.Info("missed owners (consultant numbers):");
                foreach (var item in result.MissedOwner)
                    log.Info($"   {item.ToString()}");
            }

            if (result.CreatedSegments.Count > 0) {
                log.Info("Created segments:");
                foreach (var s in result.CreatedSegments)
                    log.Info($"   {s}");                
            }

        }
    }
}
