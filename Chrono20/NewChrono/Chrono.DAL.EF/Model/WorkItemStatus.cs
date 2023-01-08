using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class WorkItemStatus
    {
        public WorkItemStatus()
        {
            WorkItemStateHistories = new HashSet<WorkItemStateHistory>();
            WorkItemStates = new HashSet<WorkItemState>();
        }

        public int Id { get; set; }
        public string Status { get; set; } = null!;

        public virtual ICollection<WorkItemStateHistory> WorkItemStateHistories { get; set; }
        public virtual ICollection<WorkItemState> WorkItemStates { get; set; }
    }
}
