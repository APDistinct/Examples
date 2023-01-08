using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Import
{
    [Obsolete]
    public class ImportResultOld
    {
        public int NewbeCount { get; set; }
        public int UpdatedCount { get; set; }
        public int TotalRows { get; set; }
        //public int OwnerUpdated { get; set; }

        public List<Guid> Users { get; } = new List<Guid>();

        public void Add(ImportResultOld result) {
            NewbeCount += result.NewbeCount;
            UpdatedCount += result.UpdatedCount;
            TotalRows += result.TotalRows;
            //OwnerUpdated += result.OwnerUpdated;
            Users.AddRange(result.Users);
        }
    }
}
