using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;

namespace FLChat.DAL.Import
{
    public class ImportSegmentsBatch : IDisposable
    {
        /// <summary>
        /// imported segment's names
        /// </summary>
        private readonly Dictionary<string, Guid> _segNames;

        /// <summary>
        /// imported users. SQL data type is [dbo].[IntList]
        /// </summary>
        private readonly DataTable _users;

        /// <summary>
        /// imported users in segments. SQL data type is [dbo].[IntGuidList]
        /// </summary>
        private readonly DataTable _segments;

        public ImportSegmentsBatch(Dictionary<string, Guid> segNames) {
            _segNames = segNames;

            _users = new DataTable();
            _users.Columns.Add("[Value]", typeof(int));            

            _segments = new DataTable();
            _segments.Columns.Add("[Int]", typeof(int));
            _segments.Columns.Add("[Guid]", typeof(Guid));
        }

        /// <summary>
        /// Add user with segments to data
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="segments"></param>
        public void Add(int userNumber, IEnumerable<string> segments) {
            _users.Rows.Add(userNumber);
            foreach (string seg in segments) {
                _segments.Rows.Add(userNumber, _segNames[seg]);
            }
        }

        public struct Result
        {
            public int Inserted { get; set; }
            public int Deleted { get; set; }
        }

        /// <summary>
        /// Perform update 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public Result Execute(ChatEntities entities) {
            return entities.Database.SqlQuery<Result>(
                "exec [Usr].[UpdateSegmentsBatch] @users, @segments",
                new SqlParameter("@users", SqlDbType.Structured) {
                    TypeName = "[dbo].[IntList]",
                    Value = _users,
                    Direction = ParameterDirection.Input
                },
                new SqlParameter("@segments", SqlDbType.Structured) {
                    TypeName = "[dbo].[IntGuidList]",
                    Value = _segments,
                    Direction = ParameterDirection.Input
                }).Single();
        }

        /// <summary>
        /// Associate partner name and segment guid
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="partnerNames"></param>
        /// <returns></returns>
        static public Dictionary<string, Guid> LoadSegmentsIds(
            ChatEntities entities, 
            IEnumerable<string> partnerNames,
            out List<string> inserted) 
        {
            inserted = null;
            Dictionary<string, Guid> result = new Dictionary<string, Guid>();
            foreach (string name in partnerNames) {
                Segment seg = entities.Segment.Where(s => s.PartnerName == name).SingleOrDefault();
                if (seg == null) {
                    seg = entities.Segment.Add(new Segment() {
                        IsDeleted = false,
                        Name = name,
                        Descr = name,
                        ShowInShortProfile = false,
                        PartnerName = name
                    });

                    if (inserted == null)
                        inserted = new List<string>();
                    inserted.Add(name);
                    entities.SaveChanges();
                }
                result.Add(name, seg.Id);
            }

            return result;
        }

        public void Dispose() {
            _users.Dispose();
            _segments.Dispose();
        }
    }
}
