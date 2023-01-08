using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SalesForce;
using SalesForce.Types;
using FLChat.SalesForce.DataTypes;
using FLChat.DAL.Model;
using FLChat.DAL.Import;

namespace FLChat.SalesForce.Import
{
    public class FullImport : IDisposable
    {
        private readonly SalesForceClient _client;
        private static readonly string _query;

        private readonly Dictionary<string, Guid> _segments;

        static FullImport() {
            Assembly assembly = Assembly.GetExecutingAssembly();
            _query = assembly.LoadResourceByFullName("FLChat.SalesForce.Resources.QueryContacts.txt");
        }

        public FullImport(SalesForceClient client) {
            _client = client;
            _segments = LoadAllSegments();
        }

        /// <summary>
        /// Call before performing query data request to Sales force
        /// </summary>
        public event EventHandler OnRequest;

        /// <summary>
        /// Call after request to Sales force was complete 
        /// </summary>
        public event EventHandler<Args.RequestCompleteArg> OnRequestComplete;

        /// <summary>
        /// Call before importing data
        /// </summary>
        public event EventHandler OnImport;

        /// <summary>
        /// Call after import data was complete
        /// </summary>
        public event EventHandler<ImportResult> OnImportComplete;

        /// <summary>
        /// Call before importing segments
        /// </summary>
        public event EventHandler OnImportSegments;

        public event EventHandler<ImportSegmentResult> OnImportSegmentsComplete;

        /// <summary>
        /// Data type for importing results
        /// </summary>
        public class Result
        {
            public ImportResult Totals { get; } = new ImportResult();
            public List<Tuple<string, string>> ClearedPhones { get; } = new List<Tuple<string, string>>();
            public List<Tuple<string, string>> ClearedEmails { get; } = new List<Tuple<string, string>>();
            public List<string> MissedOwner { get; } = new List<string>();

            public ImportSegmentResult Segments { get; } = new ImportSegmentResult();
            public List<string> CreatedSegments { get; } = new List<string>();
        }

        /// <summary>
        /// Perform full import
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<Result> Import(CancellationToken ct) {
            Result result = new Result();

            //perform initial request to sales force
            OnRequest?.Invoke(this, null);
            QueryResponse<Contact> response = await _client.Query<Contact>(_query, ct);
            OnRequestComplete?.Invoke(this, new Args.RequestCompleteArg() { TotalCount = response.TotalSize });

            do {
                if (!response.Done)
                    throw new Exception("SalesForce query status is not done");

                OnImport?.Invoke(this, null);
                //import received data to database
                using (ChatEntities entities = new ChatEntities())
                using (var trans = entities.Database.BeginTransaction(IsolationLevel.ReadCommitted)) {
                    ImportResult current = entities.Import(
                        response.Records.ToImportUsersSalesForceTable(),
                        result.ClearedPhones,
                        result.ClearedEmails,
                        result.MissedOwner);

                    trans.Commit();

                    result.Totals.TotalRows = response.TotalSize;
                    result.Totals.Add(current);
                    OnImportComplete?.Invoke(this, current);
                }

                OnImportSegments?.Invoke(this, null);
                //prepare segments data tables
                response.Records.CreateImportSegmentsTables(
                    s => {
                        if (!_segments.TryGetValue(s, out Guid id)) {
                            id = CreateNewSegment(s);
                            _segments[s] = id;
                            result.CreatedSegments.Add(s);
                        }

                        return id;
                    }, out DataTable users, out DataTable segments);

                //import segments
                using (ChatEntities entities = new ChatEntities())
                using (var trans = entities.Database.BeginTransaction(IsolationLevel.ReadCommitted)) {

                    ImportSegmentResult cur = entities.ImportSegments(users, segments);
                    result.Segments.Add(cur);
                    trans.Commit();
                    OnImportSegmentsComplete?.Invoke(this, cur);
                }

                //if has remaining results
                if (response.NextRecordsUrl == null)
                    return result;

                //request remeining results
                OnRequest?.Invoke(this, null);
                response = await _client.QueryNext<Contact>(response.NextRecordsUrl, ct);
                OnRequestComplete?.Invoke(this, new Args.RequestCompleteArg() { TotalCount = response.TotalSize });

            } while (true);
        }

        public void Dispose() {
            _client.Dispose();
        }

        /// <summary>
        /// Load all segments Id and PartnerName to dictionary
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, Guid> LoadAllSegments() {
            using (ChatEntities entities = new ChatEntities()) {
                return entities
                    .Segment
                    .Where(s => s.PartnerName != null)
                    .ToArray()
                    .ToDictionary(i => i.PartnerName, i => i.Id);
            }
        }

        /// <summary>
        /// Create new segment ans return it's Id
        /// </summary>
        /// <param name="partnerName"></param>
        /// <returns></returns>
        private Guid CreateNewSegment(string partnerName) {
            using (ChatEntities entities = new ChatEntities())
            using (var trans = entities.Database.BeginTransaction(IsolationLevel.ReadCommitted)) {
                Segment s = new Segment() {
                    Descr = partnerName,
                    Name = partnerName,
                    PartnerName = partnerName
                };
                entities.Segment.Add(s);
                entities.SaveChanges();
                trans.Commit();

                return s.Id;
            }
        }
    }
}
