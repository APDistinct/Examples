using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class WorkItem
    {
        public WorkItem()
        {
            OutWorkItems = new HashSet<OutWorkItem>();
            TimeReportHistories = new HashSet<TimeReportHistory>();
            TimeReports = new HashSet<TimeReport>();
            WorkItemStates = new HashSet<WorkItemState>();
        }

        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string OutId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int? WorkItemTypeId { get; set; }
        public double? Estimate { get; set; }

        public virtual WorkItemType? WorkItemType { get; set; }
        public virtual ICollection<OutWorkItem> OutWorkItems { get; set; }
        public virtual ICollection<TimeReportHistory> TimeReportHistories { get; set; }
        public virtual ICollection<TimeReport> TimeReports { get; set; }
        public virtual ICollection<WorkItemState> WorkItemStates { get; set; }
    }
}
