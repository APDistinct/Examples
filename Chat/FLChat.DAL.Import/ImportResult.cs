using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Import
{
    public class ImportResult
    {
        public int Inserted { get; set; }
        public int Updated { get; set; }
        public int ClearedPhone { get; set; }
        public int ClearedEmail { get; set; }
        public int OwnerUpdated { get; set; }
        public int MissedOwner { get; set; }

        public int TotalRows { get; set; }

        public void Add(ImportResult r) {
            Inserted += r.Inserted;
            Updated += r.Updated;
            ClearedPhone += r.ClearedPhone;
            ClearedEmail += r.ClearedEmail;
            OwnerUpdated += r.OwnerUpdated;
            MissedOwner += r.MissedOwner;

            TotalRows += r.TotalRows;
        }
    }

    public class ImportSegmentResult
    {
        public int Inserted { get; set; }
        public int Deleted { get; set; }

        public void Add(ImportSegmentResult res) {
            Inserted += res.Inserted;
            Deleted += res.Deleted;
        }
    }
}
