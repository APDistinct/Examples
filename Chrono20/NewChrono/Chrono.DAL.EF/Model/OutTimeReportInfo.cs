using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class OutTimeReportInfo
    {
        public OutTimeReportInfo()
        {
            OutTimeReportData = new HashSet<OutTimeReportDatum>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime LoadDate { get; set; }
        public string FileName { get; set; } = null!;

        public virtual User User { get; set; } = null!;
        public virtual ICollection<OutTimeReportDatum> OutTimeReportData { get; set; }
    }
}
