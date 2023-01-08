using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class ReportStatus
    {
        public ReportStatus()
        {
            TimeReports = new HashSet<TimeReport>();
        }

        public int Id { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<TimeReport> TimeReports { get; set; }
    }
}
