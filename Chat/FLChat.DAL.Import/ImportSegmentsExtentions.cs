using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;
using System.Data.SqlClient;
using System.Data.Entity;

namespace FLChat.DAL.Import
{
    public static class ImportSegmentsExtentions
    {
        public const int COMMIT_AFTER_EACH_ROWS = 50;

        public static void ImportSegments(this ChatEntities entities, IEnumerable<Tuple<string, IEnumerable<int>>> segments) {
            foreach (var item in segments) {
                Segment segment = entities.Segment.Where(s => s.Name == item.Item1).SingleOrDefault();
                if (segment == null) {
                    segment = new Segment() {
                        Name = item.Item1,
                        Descr = item.Item1,
                        IsDeleted = false
                    };
                    entities.Segment.Add(segment);
                    entities.SaveChanges();
                }

                entities.ExecuteSegmentUpdateMembers(segment.Id, item.Item2);
            }
        }

        /// <summary>
        /// Update segments for user
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="users">List of {user fl number, list of partner segments}</param>
        public static int ImportSegments(this ChatEntities entities, IImportSource<Tuple<int, IEnumerable<string>>> users, IListener listener = null) {
            int cnt = 0;
            Dictionary<string, Guid> dict = entities
                .Segment
                .Where(s => s.PartnerName != null)
                .ToDictionary(s => s.PartnerName, s => s.Id);

            DbContextTransaction trans = null;
            try {
                trans = entities.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

                foreach (var item in users) {
                    cnt += 1;
                    try {
                        entities.User_UpdateSegments(null, item.Item1, item.Item2.Select(s => dict[s]));
                    } catch (SqlException e) {
                        listener?.Warning(cnt, $"Error number {e.Number.ToString()}");
                        if (e.Number == 50001)
                            listener?.Warning(cnt, $"Missed user {item.Item1.ToString()}");
                        else
                            throw;
                    }
                    listener?.Progress(0, cnt, true);

                    if (cnt % COMMIT_AFTER_EACH_ROWS == 0) {
                        trans.Commit();
                        trans = entities.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                    }
                }

                trans.Commit();
                trans.Dispose();
                trans = null;
            } finally {
                if (trans != null) {
                    trans.Rollback();
                    trans.Dispose();
                }
            }
            return cnt;
        }

        /// <summary>
        /// Syncronize segments
        /// </summary>
        /// <param name="entities">database</param>
        /// <param name="segments">list of segment's partner name</param>
        /// <returns>list of inserted segments</returns>
        public static IEnumerable<string> SyncPartnerSegments(this ChatEntities entities, IEnumerable<string> segments) {
            List<string> inserted = new List<string>();
            foreach(string name in segments) {
                if (entities.Segment.Where(s => s.PartnerName == name).Any() == false) {
                    entities.Segment.Add(new Segment() {
                        IsDeleted = false,
                        Name = name,
                        Descr = name,
                        ShowInShortProfile = false,
                        PartnerName = name
                    });
                    inserted.Add(name);
                }
            }
            entities.SaveChanges();
            return inserted;
        }
    }
}
