using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class TimeReportHistory
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid WorkItemId { get; set; }
        public DateTime ReportDate { get; set; }
        public double Hours { get; set; }
        public double BillHours { get; set; }
        public int Type { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid UpdateUserId { get; set; }
        public Guid? ExternalBillId { get; set; }
        public Guid? InternalBillId { get; set; }

        public virtual User UpdateUser { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual WorkItem WorkItem { get; set; } = null!;
    }
}
