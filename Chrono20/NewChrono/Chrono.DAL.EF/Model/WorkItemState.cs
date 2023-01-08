using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class WorkItemState
    {
        public WorkItemState()
        {
            WorkItemStateHistories = new HashSet<WorkItemStateHistory>();
        }

        public Guid Id { get; set; }
        public Guid WorkItemId { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int WorkItemStatusId { get; set; }

        public virtual WorkItem WorkItem { get; set; } = null!;
        public virtual WorkItemStatus WorkItemStatus { get; set; } = null!;
        public virtual ICollection<WorkItemStateHistory> WorkItemStateHistories { get; set; }
    }
}
