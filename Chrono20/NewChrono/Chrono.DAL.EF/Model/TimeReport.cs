using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class TimeReport
    {
        public TimeReport()
        {
            OutTimeReportData = new HashSet<OutTimeReportDatum>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid WorkItemId { get; set; }
        public DateTime ReportDate { get; set; }
        public double Hours { get; set; }
        public double BillHours { get; set; }
        public int ReportStatusId { get; set; }
        public int Type { get; set; }
        public bool IsSynchronized { get; set; }
        public DateTime LastUpdated { get; set; }
        public Guid? ExternalBillId { get; set; }
        public Guid? InternalBillId { get; set; }

        public virtual Bill? ExternalBill { get; set; }
        public virtual Bill? InternalBill { get; set; }
        public virtual ReportStatus ReportStatus { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual WorkItem WorkItem { get; set; } = null!;
        public virtual ICollection<OutTimeReportDatum> OutTimeReportData { get; set; }
    }
}
